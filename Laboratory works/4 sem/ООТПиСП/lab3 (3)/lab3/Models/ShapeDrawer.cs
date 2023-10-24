using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using lab3.Models.Shapes;
using Shape = lab3.Models.Shapes.Shape;

namespace lab3.Models;

public class ShapeDrawer
{

    public void Draw( ShapeList shapeList, Canvas canvas)
    {
        foreach( var shape in shapeList)
            DrawShape(shape, canvas);
    }

    private void DrawShape(Shape shape, Canvas canvas)
    {
        var pen = new Pen( new SolidColorBrush(shape.GetColor() ), shape.GetThickness() );
        var list = shape.GetDotList();
        if (shape.GetDotNum() > 1)
        {
            canvas.Children.Add( getLine( pen, list[0], list[1]));
        }
        for ( int i = 1; i < shape.GetDotNum(); i++ )
            canvas.Children.Add( getLine( pen, list[i-1], list[i]));
        canvas.Children.Add( getLine( pen, list[0], list[shape.GetDotNum() - 1]));
        Line line = new Line();
        canvas.Children.Add(line);
    }

    private Line getLine(Pen pen, Point p1, Point p2)
    {
        Line line = new Line();
        line.StartPoint = p1;
        line.EndPoint = p2;
        line.Stroke = pen.Brush;
        line.StrokeThickness = pen.Thickness;
        return line;
    }
    
}