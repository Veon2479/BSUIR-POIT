using System;
using System.Reflection;
using lab3.Models.Shapes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace lab3.Models;

public class ShapeFactory
{

    public delegate Shape Create(Point crd1, Point crd2, Color color, int thickness);
    struct LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct
    {
        public String name;
        public LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct(string name, Create cr)
        {
            this.name = name;
            this.del = cr;
        }
        public Create del;
    }
    
    private LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct[] _info = new LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct[10];
    private int _typeCount;
    
    private ShapeFactory()
    {
        //create del = Dot.create;
        /*_info[0] = new LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct(Dot.GetStaticShapeType(), Dot.create);
        _info[1] = new LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct(Segment.GetStaticShapeType(), Segment.create);
        _info[2] = new LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct(Triangle.GetStaticShapeType(), Triangle.create);
        _info[3] = new LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct(Rectangle.GetStaticShapeType(), Rectangle.create);
        _info[4] = new LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct(Rhombus.GetStaticShapeType(), Rhombus.create);
        _info[5] = new LkstrjlfsjlkfsdkjlfdsUnfriendlYjgfjjkStruct(Ellipse.GetStaticShapeType(), Ellipse.create);
        _typeCount = 6;*/
        _typeCount = 0;
        RegisterType(Dot.GetStaticShapeType(), Dot.create);
        RegisterType(Segment.GetStaticShapeType(), Segment.create);
        RegisterType(Triangle.GetStaticShapeType(), Triangle.create);
        RegisterType(Rectangle.GetStaticShapeType(), Rectangle.create);
        RegisterType(Rhombus.GetStaticShapeType(), Rhombus.create);
        RegisterType(Ellipse.GetStaticShapeType(), Ellipse.create); 


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
        for (int i = 0; i < _info.Length; i++)
        {
            if (_info[i].name == name)
                return _info[i].del(p1, p2, color, thickness);
        }
        Random rd = new Random();
        return _info[rd.NextInt64(0, _typeCount - 1)].del(p1, p2, color, thickness);
    }

    public int GetTypesCount()
    {
        return _typeCount;
    }

    public bool RegisterType(string name, Create constructor)
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