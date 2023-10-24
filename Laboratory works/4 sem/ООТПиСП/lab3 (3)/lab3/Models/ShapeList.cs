using System;
using System.Collections.Generic;
using lab3.Models.Interfaces;

namespace lab3.Models.Shapes;

[Serializable]
public class ShapeList : List<Shape>
{
    public Shape? FindNearCollision(Point p)
    {
        Shape? result = null;
        int i = 0;
        while (i < this.Count && result == null)
        {
            if (this[i] is ISelectableShape && this[i].IsNearCollision(p))
                result = this[i];
            i++;
        }
        return result;
    }

    public void WriteShapes()
    {
        Console.WriteLine("\nItems:");
        for (int i = 0; i < this.Count; i++)
            Console.WriteLine("     "+i+" "+this[i].GetShapeType());
    }
}