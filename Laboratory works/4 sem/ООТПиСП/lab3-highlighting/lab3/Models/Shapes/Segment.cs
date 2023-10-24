using lab3.Models.Interfaces;

namespace lab3.Models.Shapes;

public class Segment : Shape, IChangeableShape, IMovableShape
{
    public Segment( Point coord1, Point coord2, Color color, int thickness) : base(coord1, coord2, color, thickness)
    {
        
    }

    protected override void ComputePoints(Point coord1, Point coord2)
    {
        this.dotNum = 2;
        this.dotList.Add( coord1 );
        this.dotList.Add( coord2 );
    }
    
    public static Segment create(Point coord1, Point coord2, Color color, int thickness)
    {
        return new Segment(coord1, coord2, color, thickness);
    }
}