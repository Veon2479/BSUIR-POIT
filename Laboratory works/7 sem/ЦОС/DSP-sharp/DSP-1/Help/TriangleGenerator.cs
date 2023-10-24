namespace DSP_1.Help;

public class TriangleGenerator : Generator
{
    public double CurY;
    private double _dy;
    
    public TriangleGenerator(double dx, double dy, double ymin, double ymax) : base(dx, dy, ymin, ymax)
    {
        _dy = Dy;
    }
    
    public TriangleGenerator(double dx, double dy, double ymin, double ymax, double p) : this(dx, dy, ymin, ymax)
    {
        CurY += Amp * p;
    }

    public override double Generate(double x)
    {
        CurY += _dy;
        if (CurY >= Ymax || CurY <= Ymin)
        {
            _dy = -_dy;
            CurY += _dy;
        }

        return CurY;
    }
}