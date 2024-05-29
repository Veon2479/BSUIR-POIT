using System.IO;
using System;
using System.Numerics;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace _3DViewer;

public class ObjParser
{
    public Model ParseObjFile(string path, string file)
    {
        Model model = new();

        string[] lines = File.ReadAllLines(path + file);

        float max = -1.0f;
        float min = float.PositiveInfinity;
        float sumx = 0, sumy = 0, sumz = 0;
        float sum = 0;

        string? mtlfile = null;
        
        foreach (var line in lines)
        {
            string[] parts = line.Split(' ');

            if (parts.Length > 0)
            {
                switch (parts[0])
                {
                    case "v":
                        var vx = float.Parse(parts[1]);
                        var vy = float.Parse(parts[2]);
                        var vz = float.Parse(parts[3]);
                        float vw = 1.0f;
                        if (parts.Length == 5) vw = float.Parse(parts[4]);
                        model.Vs.Add(new Vector4(vx, vy, vz, vw));

                        sumx += vx;
                        sumy += vy;
                        sumz += vz;

                        sum += vx + vy + vz;
                        
                        if (vx > max) max = vx;
                        if (vy > max) max = vy;
                        if (vz > max) max = vz;

                        if (vx < min) min = vx;
                        if (vy < min) min = vy;
                        if (vz < min) min = vz;
                        
                        break;

                    case "vt":
                        var tu = float.Parse(parts[1]);
                        float tv = 0, tw = 0;
                        if (parts.Length >= 3) tv = float.Parse(parts[2]);
                        if (parts.Length == 4) tw = float.Parse(parts[3]);
                        model.Vts.Add(new Vector3(tu, tv, tw));
                        break;

                    case "vn":
                        var ni = float.Parse(parts[1]);
                        var nj = float.Parse(parts[2]);
                        var nk = float.Parse(parts[3]);
                        model.Vns.Add(Vector3.Normalize(new Vector3(ni, nj, nk)));
                        break;

                    case "f":
                        model.Pols.Add(new());
                        foreach (var part in parts)
                        {
                            if (part != "f")
                            {
                                var inds = part.Split('/');
                                var vind = int.Parse(inds[0]) - 1;
                                int tind = -1, nind = -1;
                                if (inds.Length == 2) tind = int.Parse(inds[^1]) - 1;
                                else
                                {
                                    if (inds[1] != "")
                                        tind = int.Parse(inds[1]) - 1;
                                    nind = int.Parse(inds[^1]) - 1;
                                }
                                model.Pols[^1].Add((vind, tind, nind));
                            }
                        }
                        break;
                    
                    case "mtllib":
                        mtlfile = parts[1];
                        break;
                }
            }
        }

        sum /= 3;
        sum /= model.Vs.Count;
        if (sum == 0)
            sum = 1;
        Parallel.For(0, model.Vs.Count, i =>
        {
            var vs = model.Vs[i];
            var w = vs.W;
            vs /= sum;
            vs.W = w;
            model.Vs[i] = vs;
        });
        
        Console.WriteLine($"Loaded {model.Vs.Count} vertices, {model.Pols.Count} polygons");

        if (mtlfile != null)
        {
            lines = File.ReadAllLines(path + mtlfile);
            foreach (var line in lines)
            {
                string[] parts = line.Split(' ');
                if (parts.Length > 0)
                {
                    switch (parts[0])
                    {
                        case "norm":
                            using (var image = Image.Load<Rgba32>(path + parts[1]))
                            {
                                model.NormalMap = new Vector3[image.Width, image.Height];
                                Parallel.For(0, image.Width, x =>
                                {
                                    for (int y = 0; y < image.Height; y++)
                                    {
                                        Rgba32 pixel = image[x, y];
                                        Vector3 normal = new Vector3(
                                            pixel.R / 255f * 2 - 1,
                                            pixel.G / 255f * 2 - 1,
                                            pixel.B / 255f * 2 - 1
                                        );
                                        model.NormalMap[x, y] = normal;
                                    }
                                });
                                model.MapHeight = image.Height;
                                model.MapWidth = image.Width;
                                Console.WriteLine("Loaded map of normales");
                            }
                            break;
                        case "map_Kd":
                            using (var image = Image.Load<Rgba32>(path + parts[1]))
                            {
                                model.DiffuseMap = new int[image.Width, image.Height];
                                Parallel.For(0, image.Width, x =>
                                {
                                    for (int y = 0; y < image.Height; y++)
                                    {
                                        Rgba32 pixel = image[x, y];
                                        model.DiffuseMap[x, y] = Renderer.GetColor(pixel.A, pixel.R, pixel.G, pixel.B);
                                    }
                                });
                                model.MapHeight = image.Height;
                                model.MapWidth = image.Width;
                                Console.WriteLine("Loaded diffuse map");
                            }
                            break;
                        
                        case "map_Ks":
                            using (var image = Image.Load<Rgba32>(path + parts[1]))
                            {
                                model.SpecularMap = new float[image.Width, image.Height];
                                Parallel.For(0, image.Width, x =>
                                {
                                    for (int y = 0; y < image.Height; y++)
                                    {
                                        model.SpecularMap[x, y] = image[x, y].B / 255f;
                                    }
                                });
                                model.MapHeight = image.Height;
                                model.MapWidth = image.Width;
                            }
                            Console.WriteLine("Loaded specular map");
                            break;
                    }
                }
            }

            if (model.MapHeight != null && model.MapWidth != null)
            {
                Parallel.For(0, model.Vts.Count, i =>
                {
                    var vt = model.Vts[i];
                    if (vt.X > 1)
                        vt.X -= (int)vt.X;
                    if (vt.Y > 1)
                        vt.Y -= (int)vt.Y;
                    vt.X *= (int)model.MapWidth;
                    if (vt.X > 0)
                        vt.X -= 1; 
                    vt.Y *= (int)model.MapHeight;
                    if (vt.Y > 0)
                        vt.Y -= 1;
                    model.Vts[i] = vt;
                });
            }
 
        }
        
        return model;
    }
}