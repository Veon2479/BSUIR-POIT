using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace _3DViewer;

public class Renderer
{
    private Model _model, _warped;
    private int _bkg, _px;

    private int _stride, _pixelCount;
    private int[] _pixelData;
    
    private Matrix4x4 _cached;
    
    private float[] _zBuffer;

    private State _cached_state;
    
    public Renderer(ref Model model, in WriteableBitmap dest, in State state)
    {
        _model = model;
        _warped = new();
        _warped.Vs = Enumerable.Repeat(default(Vector4), _model.Vs.Count).ToList();
        _warped.Vns = Enumerable.Repeat(default(Vector3), _model.Vns.Count).ToList();

        _bkg = GetColor(255, 0, 0, 0);
        _px = GetColor(255, 255, 255, 255);

        using (var context = dest.Lock())
        {
            _stride = context.RowBytes / 4;
            _pixelCount = (int) Math.Round(dest.Size.Width * dest.Size.Height);
            _pixelData = new int[_pixelCount];
        }
        
        var view = new Matrix4x4
        {
            M11 = state.Width / 2,
            M41 = state.Width / 2,
            M22 = - state.Height / 2,
            M42 = state.Height / 2,
            M33 = 1,
            M44 = 1
        };
        var project = Matrix4x4.CreatePerspectiveFieldOfView(state.FOV, state.Aspect, state.ZNear, state.ZFar);
        var viewport = Matrix4x4.CreateLookAt(state.Eye, state.Target, state.Up);
        _cached = viewport * project * view;
        _cached_state = state;
        _zBuffer = new float[_pixelCount];
    }

    public static int GetColor(int a, int r, int g, int b)
    {
        return (a << 24) | (b << 16) | (g << 8) | r;
    }

    private int GetR(int color)
    {
        return color & 0xFF;
    }

    private int GetB(int color)
    {
        return (color >> 16) & 0xFF;
    }

    private int GetG(int color)
    {
        return (color >> 8) & 0xFF;
    }

    private int GetA(int color)
    {
        return (color >> 24) & 0xFF;
    }
    
    public static Matrix4x4 TransposeMatrix(Matrix4x4 matrix)
    {
        return new Matrix4x4(
            matrix.M11, matrix.M21, matrix.M31, matrix.M41,
            matrix.M12, matrix.M22, matrix.M32, matrix.M42,
            matrix.M13, matrix.M23, matrix.M33, matrix.M43,
            matrix.M14, matrix.M24, matrix.M34, matrix.M44
        );
    }
    
    private int GetShadedColor(int baseColor, double intensity)
    {
        var a = GetA(baseColor);
        var r = (int)(GetR(baseColor) * intensity);
        var g = (int)(GetG(baseColor) * intensity);
        var b = (int)(GetB(baseColor) * intensity);
        return GetColor(a, r, g, b);
    }
    
    private int GetPhongShadedColor(int baseColor, in Vector3 w_normal, in Vector3 p, in Vector3? vt)
    {
        double k_a = 0.1f;
        double k_d = 0.8f;
        double k_s = 0.5f;
        
        if (vt != null)
            k_s = _model.SpecularMap![(int)vt?.X!, (int)(_model.MapHeight! - vt?.Y - 1)!];

        double ambient = k_a;
        
        var eye = _cached_state.Eye;
        var light = _cached_state.LightDirection;
        
        double diffuse = k_d * Vector3.Dot(w_normal, Vector3.Normalize(light));
        diffuse = Math.Max(0, diffuse);


        double alpha = 16f;
        var L = Vector3.Normalize(light - p);
        var V = Vector3.Normalize(eye - p);
        var R = Vector3.Reflect(-L, w_normal);
        var scal = Vector3.Dot(R, V);
        scal = Math.Max(0, scal);
        double specular = k_s * Math.Pow(scal, alpha);
        
        
        double intensity = ambient + diffuse + specular;

        intensity = Math.Min(intensity, 1);

        if (specular <= k_s)
            return GetShadedColor(baseColor, intensity);
        else
            return GetShadedColor(GetColor(255, 255, 255, 255), intensity);
    }
    private void DrawPixel(float a, float b, float g, in (int, int, int) v1, in (int, int, int) v2, in (int, int, int) v3)
    {
        var vs1 = _warped.Vs[v1.Item1];
        var vs2 = _warped.Vs[v2.Item1];
        var vs3 = _warped.Vs[v3.Item1];

        var v = a * vs1 + b * vs2 + g * vs3;

        var x = (int)Math.Round(v.X);
        var y = (int)Math.Round(v.Y);
        
        var ind = y * _stride + x;
        if (ind >= 0 && ind < _pixelCount)
        {
            var z = v.Z;
            if (z < _zBuffer[ind])
            {
                _zBuffer[ind] = z;

                Vector3? vt = null;
                if (_model.MapHeight != null && v1.Item2 != -1 && v2.Item2 != -1 && v3.Item2 != -1)
                {
                    var iz1 = 1 / ((vs1.Z + 1)/2);
                    var iz2 = 1 / ((vs2.Z + 1)/2);
                    var iz3 = 1 / ((vs3.Z + 1)/2);

                    var vt1 = _model.Vts[v1.Item2];
                    var vt2 = _model.Vts[v2.Item2];
                    var vt3 = _model.Vts[v3.Item2];
                    
                    // vt = (a*vt1*iz1 + b*vt2*iz2 + g*vt3*iz3) / (a*iz1 + b*iz2 + g*iz3);
                    
                    var pca = a * iz1;
                    var pcb = b * iz2;
                    var pcg = g * iz3;
                    var sum = pca + pcb + pcg;
                    pca /= sum;
                    pcb /= sum;
                    pcg /= sum;
                    vt = pca * vt1 + pcb * vt2 + pcg * vt3;
                }
                
                Vector3 w_normal;
                if (_model.NormalMap != null)
                {
                    w_normal = _model.NormalMap[(int)vt?.X!, (int)(_model.MapHeight! - vt?.Y - 1)!];
                    w_normal = Vector3.Transform(w_normal, _rot);
                    w_normal = Vector3.Normalize(w_normal);
                }
                else
                {
                    var wvn1 = _warped.Vns[v1.Item3];
                    var wvn2 = _warped.Vns[v2.Item3];
                    var wvn3 = _warped.Vns[v3.Item3];
                    w_normal = a * wvn1 + b * wvn2 + g * wvn3;
                    w_normal = Vector3.Normalize(w_normal);
                }
                
                var vm1 = _model.Vs[v1.Item1];
                var vm2 = _model.Vs[v2.Item1];
                var vm3 = _model.Vs[v3.Item1];
                var p4 = a * vm1 + b * vm2 + g * vm3;
                p4 = Vector4.Transform(p4, _world);
                p4 /= p4.W;
                var p = new Vector3(p4.X, p4.Y, p4.Z);

                if (_model.DiffuseMap != null)
                {
                    var clr = _model.DiffuseMap[(int)vt?.X!, (int)(_model.MapHeight! - vt?.Y - 1)!];
                    _pixelData[ind] = GetPhongShadedColor(clr, w_normal, p, vt);
                }
                else
                {
                    _pixelData[ind] = GetPhongShadedColor(_px, w_normal, p, vt);
                }
            }
        }
    }

    public static float FastInverseSqrt(float number)
    {
        const float threehalfs = 1.5f;

        float x2 = number * 0.5f;
        float y = number;

        int i = BitConverter.ToInt32(BitConverter.GetBytes(y), 0);
        i = 0x5f3759df - (i >> 1);
        y = BitConverter.ToSingle(BitConverter.GetBytes(i), 0);

        y = y * (threehalfs - (x2 * y * y));

        return y;
    }

    private float[,] abgs = { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };

    private void RasterizeTriangle((int, int, int) v1, (int, int, int) v2, (int, int, int) v3)
    {
        var vs1 = _warped.Vs[v1.Item1];
        var vs2 = _warped.Vs[v2.Item1];
        var vs3 = _warped.Vs[v3.Item1];
        
        var x1 = (vs1.X);
        var x2 = (vs2.X);
        var x3 = (vs3.X);
        
        var y1 = (vs1.Y);
        var y2 = (vs2.Y);
        var y3 = (vs3.Y);
 

        float [] l2s =
        {
            ((x3 - x2)*(x3 - x2) + (y3 - y2)*(y3 - y2)),
            ((x1 - x3)*(x1 - x3) + (y1 - y3)*(y1 - y3)),
            ((x2 - x1)*(x2 - x1) + (y2 - y1)*(y2 - y1)),
        };
        
        int baseInd = 0;
        for (int i = 0; i < l2s.Length; i++)
        {
            if (l2s[i] >= l2s[baseInd])
                baseInd = i;
        }
        
        var a = abgs[baseInd, 0];
        var b = abgs[baseInd, 1];
        var g = abgs[baseInd, 2];
        
        // var len = (float)Math.Sqrt(l2s[baseInd]);
        // var step = 1 / len;

        var step = FastInverseSqrt(l2s[baseInd]);
        var len = 1 / step;
        
        float da = step, db = step;
        if (a >= 0.999)
            da = -da;

        for (int i = 0; i < (int)len + 1; i++)
        {
            b = 0;
            for (int j = 0; j < (int)(len * (1 - a)) + 1; j++)
            {
                g = 1 - a - b;
                DrawPixel(a, b, g, v1, v2, v3);
                b += db;
            }
            a += da;
        }
    }

    
    private void RasterizePolygon(in List<(int, int, int)> pol)
    {
        var v1 = _warped.Vs[pol[0].Item1];
        var v2 = _warped.Vs[pol[1].Item1];
        var v3 = _warped.Vs[pol[2].Item1];

        var e1 = v1 - v2;
        var e2 = v2 - v3;

        var normal = Vector3.Cross(new Vector3(e1.X, e1.Y, e1.Z), new Vector3(e2.X, e2.Y, e2.Z));
        normal = Vector3.Normalize(-normal);
        if (Vector3.Dot(normal, _cached_state.Eye) < 0)
            return;

        for (int i = 1; i < pol.Count() - 1; i++)
        {
            RasterizeTriangle(pol[0], pol[i], pol[i+1]);
        }
    }
    private bool IsPolygonInScreen(List<(int, int, int)> pol)
    {
        foreach (var vertex in pol)
        {
            int vertexX = (int)Math.Round(_warped.Vs[vertex.Item1].X);
            int vertexY = (int)Math.Round(_warped.Vs[vertex.Item1].Y);
            if (vertexX >= 10 && vertexX < _cached_state.Width - 50 && vertexY >= 50 && vertexY < _cached_state.Height)
            {
                return true;
            }
        }
        return false;
    }
    
    private bool IsPolygonTooClose(List<(int, int, int)> pol, float minZDistance)
    {
        foreach (var vertex in pol)
        {
            float vertexZ = _warped.Vs[vertex.Item1].Z;
            if (vertexZ > minZDistance)
            {
                return false;
            }
        }
        return true;
    }

    
    private Matrix4x4 _rule, _rot, _world;
    public void Render(ref WriteableBitmap dest, in State state)
    {
        _cached_state = state;
        
        var move = Matrix4x4.CreateTranslation(state.Position);
        var rotX = Matrix4x4.CreateRotationX(state.Rotate.X);
        var rotY = Matrix4x4.CreateRotationY(state.Rotate.Y);
        var rotZ = Matrix4x4.CreateRotationZ(state.Rotate.Z);
        _rot = rotX * rotY * rotZ;
        var scale = Matrix4x4.CreateScale(state.Scale);
        var rule = scale * move * _rot * _cached;
        _world = scale * move * _rot;
        _rule = rule;
        
        Parallel.For(0, _model.Vs.Count, i =>
        {
            _warped.Vs[i] = Vector4.Transform(_model.Vs[i], rule);
            _warped.Vs[i] /= _warped.Vs[i].W;
        });

        Parallel.For(0, _model.Vns.Count, i =>
        {
            var n = Vector3.Transform(_model.Vns[i], _rot);
            _warped.Vns[i] = Vector3.Normalize(n);
            
        });
  
        // fast background filling
        Parallel.For(0, _pixelCount, i => _pixelData[i] = _bkg);

        // clear z-buffer
        Array.Fill(_zBuffer, float.MaxValue);
        
        Parallel.ForEach(_model.Pols, (pol) =>
        {
            if (IsPolygonInScreen(pol) && !IsPolygonTooClose(pol, -0.5f))
            {
                RasterizePolygon(pol);
            }
        });
        
        using (var context = dest.Lock())
        {
            // sending data to the image in the GUI
            Marshal.Copy(_pixelData, 0, context.Address, _pixelCount);
        }
        
    }
}
