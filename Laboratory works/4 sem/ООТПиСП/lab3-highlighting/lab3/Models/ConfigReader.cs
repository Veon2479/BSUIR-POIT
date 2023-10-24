using System;
using lab3.Models.Shapes;

namespace lab3.Models;

public class ConfigReader
{

    public delegate Shape create(Point crd1, Point crd2, Color color, int thickness);
    struct lkstrjlfsjlkfsdkjlfdsUNFRIENDLYjgfjjkStruct
    {
        public String name;
        

        public lkstrjlfsjlkfsdkjlfdsUNFRIENDLYjgfjjkStruct(string name, create cr)
        {
            this.name = name;
            this.del = cr;
        }
        public create del;
    }
    
    private lkstrjlfsjlkfsdkjlfdsUNFRIENDLYjgfjjkStruct[] info = new lkstrjlfsjlkfsdkjlfdsUNFRIENDLYjgfjjkStruct[6];
    
    public ConfigReader()
    {
        //create del = Dot.create;
        info[0] = new lkstrjlfsjlkfsdkjlfdsUNFRIENDLYjgfjjkStruct("Dot", Dot.create);
        info[1] = new lkstrjlfsjlkfsdkjlfdsUNFRIENDLYjgfjjkStruct("Segment", Segment.create);
        info[2] = new lkstrjlfsjlkfsdkjlfdsUNFRIENDLYjgfjjkStruct("Triangle", Triangle.create);
        info[3] = new lkstrjlfsjlkfsdkjlfdsUNFRIENDLYjgfjjkStruct("Rectangle", Rectangle.create);
        info[4] = new lkstrjlfsjlkfsdkjlfdsUNFRIENDLYjgfjjkStruct("Rhombus", Rhombus.create);
        info[5] = new lkstrjlfsjlkfsdkjlfdsUNFRIENDLYjgfjjkStruct("Ellipse", Ellipse.create);
        

    }

    public create getCreate(int i)
    {
        return info[i].del;
    }

   
}