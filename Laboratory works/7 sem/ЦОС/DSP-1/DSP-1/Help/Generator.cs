using System;

namespace DSP_1.Help;

public abstract class Generator
{
    public double Dx;
    public double Ymin { get; set; }
    public double Ymax { get; set; }
    public double Dy { get; set; }

    public Generator(double dx, double dy, double ymin, double ymax)
    {
        Dx = dx;
        Dy = dy;
        Ymax = ymax;
        Ymin = ymin;
        
        Amp = Math.Abs(ymax - ymin);
        Freq = 1;
        P = 0;
    }

    public double Amp, Freq, P;
    
    public Generator(double dx, double dy, double ymin, double ymax, double p) : this(dx, dy, ymin, ymax)
    {
        P = p;
    }

    public void setMin(double val)
    {
        Ymin = val;
        Amp = Math.Abs(Ymax - Ymin);
    }

    public void setMax(double val)
    {
        Ymax = val;
        Amp = Math.Abs(Ymax - Ymin);
    }

    public abstract double Generate(double x);
}