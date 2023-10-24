namespace lab3.Models.Interfaces;

public interface IMovableShape : ISelectableShape
{
    public void Move(ref Point p1, ref Point p2, float dx, float dy)
    {
        Point dP = new Point(dx, dy);
        p1 += dP;
        p2 += dP;
    }
}