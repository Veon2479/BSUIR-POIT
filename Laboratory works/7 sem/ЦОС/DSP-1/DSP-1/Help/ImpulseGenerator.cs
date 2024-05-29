namespace DSP_1.Help;

public class ImpulseGenerator : Generator
{
    public int PercImp { get; set; }
    private int _curState;
    
    public ImpulseGenerator(double dx, double dy, double ymin, double ymax, int percImp) : base(dx, dy, ymin, ymax)
    {
        PercImp = percImp;
        _curState = 0;
    }

    public ImpulseGenerator(double dx, double dy, double ymin, double ymax, int percImp, int p) : this(dx, dy, ymin,
        ymax, percImp)
    {
        _curState += (p % 100);
    }

    public void SetPercImp(int val)
    {
        PercImp = val % 100;
        _curState = 0;
    }
    public override double Generate(double x)
    {
        double res = Ymin;
        if (_curState < PercImp)
            res = Ymax;
        _curState++;
        if (_curState == 100)
            _curState = 0;
        return res;
    }
}