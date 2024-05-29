using System;

namespace DSP_1.Help;

public class  SinGenerator : Generator
{
    public SinGenerator(double dx, double dy, double ymin, double ymax) : base(dx, dy, ymin, ymax)
    {
        Amp /= 2;
    }

    public SinGenerator(double dx, double dy, double ymin, double ymax, double p) : base(dx, dy, ymin, ymax, p)
    {
    }

    public override double Generate(double x)
    {
        return Math.Abs(Ymax - Ymin) / 2 * Math.Sin(x / Freq + P) + (Ymin + Math.Abs(Ymax - Ymin) / 2);
    }
}