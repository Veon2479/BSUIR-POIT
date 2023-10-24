using System;

namespace lab3.Models.Shapes;

[Serializable]
public class Dot : Ellipse
{
    
    public Dot( Point coord, Color color, int thickness) : base(coord, coord + new Point(1 , 1), color, thickness)
    {
        typename = "Dot";
    }

    public static Dot create(Point coord, Point coord0, Color color, int thickness)
    {
        return new Dot(coord, color, thickness);
    }
    
  
}