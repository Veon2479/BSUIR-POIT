using lab3.Models.Shapes;

namespace lab3.Models.Interfaces;

public interface ISelectableShape
{
    public Rectangle GetHighlighting(Point[] area, Color color)
    {
        return new Rectangle(area[0], area[1], 
            Color.FromArgb(color.A, (byte) ~(color.B),
            (byte) ~(color.G), (byte) ~(color.R)),
            1);
    }
}