namespace lab3.Models.Interfaces;

public interface IChangeableShape : ISelectableShape
{
    public static void Change(ref Point point, Point newPoint)
    {
        point = newPoint;
    }
}