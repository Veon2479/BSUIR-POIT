using lab3.Models.Interfaces;

namespace lab3.Models.Shapes;

public class Rectangle : Shape, IChangeableShape
{
    public Rectangle( Point coord1, Point coord2, Color color, int thickness) : base(coord1, coord2, color, thickness)
    {
    }

    protected override void ComputePoints(Point coord1, Point coord2)
    {
        dotNum = 4;
        dotList.Add( coord1 );
        dotList.Add(new Point(coord1.X, coord2.Y));
        dotList.Add( coord2 );
        dotList.Add(new Point(coord2.X, coord1.Y));
    }
    public static Rectangle create(Point coord1, Point coord2, Color color, int thickness)
    {
        return new Rectangle(coord1, coord2, color, thickness);
    }
}