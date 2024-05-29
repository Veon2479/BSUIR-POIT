using System;

namespace DSP_1.Help;

public class RandomGenerator : Generator
{
    public RandomGenerator(double dx, double dy, double ymin, double ymax) : base(dx, dy, ymin, ymax)
    {
    }
    
    public override double Generate(double x)
    {
        return Ymin + Math.Abs(Ymax - Ymin) * Random.Shared.NextDouble();
    }
}