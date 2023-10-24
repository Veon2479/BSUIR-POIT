using System;
using System.Reflection;
using lab3.Models.Shapes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace lab3.Models;

public class ShapeFactory
{
    public delegate Shape TCreate(Point crd1, Point crd2, Color color, int thickness);
    struct LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct
    {
        public String name;
        public LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct(string name, TCreate cr)
        {
            this.name = name;
            this.del = cr;
        }
        public TCreate del;
    }
    
    private LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct[] _info = new LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct[10];
    private int _typeCount;
    
    private ShapeFactory()
    {
       
            _typeCount = 0;
            RegisterType("Dot", Dot.create);
            RegisterType("Segment", Segment.create);
            RegisterType("Triangle", Triangle.create);
            RegisterType("Rectangle", Rectangle.create);
            RegisterType("Rhombus", Rhombus.create);
            RegisterType("Ellipse", Ellipse.create); 

        
        

    }

    private static ShapeFactory? _entity;

    public static ShapeFactory GetShapeFactory()
    {
        if (_entity == null)
            _entity = new ShapeFactory();
        return _entity;

    }

    public Shape GetShapeByNumber(int i, Point p1, Point p2, Color color, int thickness)
    {
       if (i >= 0 && i < _typeCount)
           return _info[i].del(p1, p2, color, thickness);
       Random rd = new Random();
       return _info[rd.NextInt64(0, _typeCount - 1)].del(p1, p2, color, thickness);
    }

    public Shape GetShapeByName(string name, Point p1, Point p2, Color color, int thickness)
    {
        Console.WriteLine(name);
        Shape result = null;
        for (int i = 0; i < _info.Length; i++)
        {
            Console.WriteLine(_info[i].name);
            if (_info[i].name == name)
                result = _info[i].del(p1, p2, color, thickness);
        }

        return result;
        /*Random rd = new Random();
        return _info[rd.NextInt64(0, _typeCount - 1)].del;*/
    }

    public int GetTypesCount()
    {
        return _typeCount;
    }

    public bool RegisterType(string name, TCreate constructor)
    {
        if (_typeCount <= 10)
        {
            _info[_typeCount] = new LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct(name, constructor);
            _typeCount++;
            return true;
        }
        return false;
    }

}