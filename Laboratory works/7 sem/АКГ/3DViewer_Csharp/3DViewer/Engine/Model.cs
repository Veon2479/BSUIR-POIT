using System.Collections.Generic;
using System.Numerics;

namespace _3DViewer;

public class Model
{
    public List<Vector4> Vs = new();
    public List<Vector3> Vts = new();
    public List<Vector3> Vns = new();
    public List<List<(int, int, int)>> Pols = new();

    public Vector3[,]? NormalMap = null;
    public int[,]? DiffuseMap = null;
    public float[,]? SpecularMap = null;
    public int? MapWidth = null, MapHeight = null;
}