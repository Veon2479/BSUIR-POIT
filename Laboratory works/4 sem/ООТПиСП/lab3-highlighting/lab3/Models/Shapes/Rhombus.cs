using lab3.Models.Interfaces;

namespace lab3.Models.Shapes;

public class Rhombus : Shape, ISelectableShape
{
    public Rhombus( Point coord1, Point coord2, Color color, int thickness) : base(coord1, coord2, color, thickness)
    {
        
    }

    protected override void ComputePoints(Point coord1, Point coord2)
    {
        this.dotNum = 4;
        this.dotList.Add( new Point( ( coord1.X + coord2.X ) / 2, coord1.Y ) );
        this.dotList.Add( new Point( coord1.X, ( coord1.Y + coord2.Y ) / 2 ) );
        this.dotList.Add( new Point( ( coord1.X + coord2.X ) / 2, coord2.Y ) );
        this.dotList.Add( new Point( coord2.X, ( coord1.Y + coord2.Y ) / 2 ) );    
    }
    public static Rhombus create(Point coord1, Point coord2, Color color, int thickness)
    {
        return new Rhombus(coord1, coord2, color, thickness);
    }
}