using System;
using System.Collections.Generic;

namespace lab3.Models.Shapes;

public abstract class Shape
{
    protected Point[] area = new Point[2];
    protected List<Point> dotList = new List<Point>(0);
    protected int dotNum = 0;
    protected Color color;
    protected int thickness;

    public Shape(Point coord1, Point coord2, Color color, int thickness)
    {
        area[0] = coord1;
        area[1] = coord2;
        SetColor( color );
        SetThickness(thickness);
        ComputePoints( coord1, coord2 );
    }

    public bool IsNearCollision(Point p)
    {
        bool result = false;
        int i = 0;
        double accuracy = thickness / 2 + 4;
        while (result == false && i < dotNum)
        {
            Point p1 = dotList[i];
            Point p2 = dotList[(i + 1) % dotNum];
            if ((p1.X >= p2.X ? p1.X : p2.X) + accuracy >= p.X )
                if ((p1.X < p2.X ? p1.X : p2.X) - accuracy <= p.X )
                    if ((p1.Y >= p2.Y ? p1.Y : p2.Y) + accuracy >= p.Y )
                        if ((p1.Y < p2.Y ? p1.Y : p2.Y) - accuracy <= p.Y)
                        {
                            double a = p1.Y - p2.Y;
                            double b = -(p1.X - p2.X);
                            double c = p1.X * p2.Y - p2.X * p1.Y;
                            double d = Math.Abs(a * p.X + b * p.Y + c) / Math.Sqrt(a * a + b * b);
                            if (d <= accuracy)
                                result = true;
                        }

            i++;
        }
        
        return result;
    }
    
    protected abstract void ComputePoints(Point coord1, Point coord2);
    
    public List<Point> GetDotList() { return this.dotList; }

    public int GetDotNum() { return this.dotNum; }

    public void SetColor(Color color) { this.color = color; }
    
    public Color GetColor() { return this.color; }

    public void SetThickness(int thickness) { this.thickness = thickness; }

    public int GetThickness() { return this.thickness; }
    
    public Point[] getArea() { return this.area; }
}