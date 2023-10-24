using System;
using System.ComponentModel;
using lab3.Models.Interfaces;

namespace lab3.Models.Shapes;

public class Ellipse : Shape, IMovableShape
{
    protected double a, b;
    protected double x0, y0;
    
    public Ellipse( Point coord1, Point coord2, Color color, int thickness) : base(coord1, coord2, color, thickness)
    {
        
    }
    protected override void ComputePoints(Point coord1, Point coord2)
    {
        x0 = (coord1.X + coord2.X) / 2;
        y0 = (coord1.Y + coord2.Y) / 2;
        a = Math.Abs( (coord1.X - coord2.X) / 2 );
        b = Math.Abs( (coord1.Y - coord2.Y) / 2 );
        double d = 0.1, angle = 0;
        while (angle < 2* Math.PI)
        {
            this.dotList.Add( new Point( x0 + a * Math.Cos( angle ), y0 + b * Math.Sin( angle ) ) );
            this.dotNum++;
            angle += d;
        }
    }

    public static Ellipse create(Point coord1, Point coord2, Color color, int thickness)
    {
        return new Ellipse(coord1, coord2, color, thickness);
    }
}