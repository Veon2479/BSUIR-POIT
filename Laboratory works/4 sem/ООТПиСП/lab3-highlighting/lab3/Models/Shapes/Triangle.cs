using lab3.Models.Interfaces;

namespace lab3.Models.Shapes;

public class Triangle : Shape, IMovableShape
{
    public Triangle( Point coord1, Point coord2, Color color, int thickness) : base(coord1, coord2, color, thickness)
    {
       
       
    }

    protected override void ComputePoints(Point coord1, Point coord2)
    {
        this.dotNum = 3;
        this.dotList.Add(new Point(coord1.X, coord2.Y));
        this.dotList.Add( coord2 );
        this.dotList.Add(new Point((coord1.X + coord2.X) / 2, coord1.Y));
    }
    public static Triangle create(Point coord1, Point coord2, Color color, int thickness)
    {
        return new Triangle(coord1, coord2, color, thickness);
    }
}