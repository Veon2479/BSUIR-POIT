using System;
using System.Numerics;

namespace _3DViewer;

public class State
{
    public float Scale;
    public Vector3 Position;
    public Vector3 Rotate;
    
    public readonly float Width, Height, Aspect, FOV, ZFar, ZNear;
    public readonly Vector3 Eye, Target, Up;

    private readonly Vector3 Defpos;
    private readonly Vector3 LightPos;
    public Vector3 LightDirection { get; set; }
    public State(float width, float height, Vector3 defpos)
    {
        Width = width;
        Height = height;
        Aspect = Width / Height;
        
        FOV = (float) Math.PI / 4;
        ZFar = 100;
        ZNear = 1;
        
        Eye = Vector3.Normalize(Vector3.UnitZ);
        Target = Vector3.Zero;
        Up = Vector3.UnitY;

        Defpos = defpos;

        LightPos = (new Vector3(1.0f, 1.0f, 1.0f));
        this.Reset();
    }

    public void Reset()
    {
        Scale = 0.025f;
        Position = Defpos;
        Rotate = Vector3.Zero;
        LightDirection = LightPos;
    }
}