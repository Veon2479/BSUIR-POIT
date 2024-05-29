namespace DSP_1.Help;

public class SawGenerator : Generator
{
    private double _curY;
    
    public SawGenerator(double dx, double dy, double ymin, double ymax) : base(dx, dy, ymin, ymax)
    {
        _curY = Ymin;
    }

    public SawGenerator(double dx, double dy, double ymin, double ymax, double p) : base(dx, dy, ymin, ymax)
    {
        _curY += Amp * p;
    }

    public override double Generate(double x)
    {
        _curY += Dy;
        if (_curY > Ymax)
            _curY = Ymin;
        return _curY;
    }
}