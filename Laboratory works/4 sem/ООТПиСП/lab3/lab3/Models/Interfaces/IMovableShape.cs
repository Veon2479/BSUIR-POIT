using lab3.Models.Shapes;

namespace lab3.Models.Interfaces;

public interface IMovableShape : ISelectableShape
{
    public static void Move( Shape shape, double dx, double dy)
    {
        Point dP = new Point(dx, dy);
        Point[] area = shape.getArea();
        area[0] += dP;
        area[1] += dP;
        if (shape is IMovableShape) //check for fools           (-_-) *he is staring at them
            shape.SetArea(area);
    }
}