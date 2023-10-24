namespace lab3.Models.Interfaces;

public interface IChangeableShape : ISelectableShape
{
    public void Change(ref Point point, Point newPoint)
    {
        point = newPoint;
    }
}