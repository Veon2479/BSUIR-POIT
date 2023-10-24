using System;
using System.IO;
using System.Text;
using Avalonia.Animation;
using HarfBuzzSharp;
using lab3.Models.Shapes;
using Microsoft.CodeAnalysis;

namespace lab3.Models;

public class ShapeListSerializer
{

    public void Serialize(ShapeList list, Stream fout)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string type = list[i].GetType().ToString();
            string itemStr = "type:" + ( type.Remove(0, type.LastIndexOf(".") + 1) ) + ";";
            var area = list[i].getArea();
            itemStr += $"corner1:({area[0].X}|{area[0].Y});";
            itemStr += $"corner2:({area[1].X}|{area[1].Y});";
            itemStr += $"color:{list[i].GetColor().ToUint32()};";
            itemStr += $"thickness:{list[i].GetThickness()};";
            itemStr += "\n";
            fout.Write(Encoding.ASCII.GetBytes(itemStr));
        }
    }

    public ShapeList Deserialize(Stream fin)
    {
        var factory = ShapeFactory.GetShapeFactory();
        var list =  new ShapeList();
        string data = "";
        byte[] buffer = new byte[fin.Length];
        while (fin.Read(buffer) != 0)
        {
            data += Encoding.ASCII.GetString(buffer);
        }
        
        while (data.Length != 0)
        {
            int pos = data.IndexOf('\n');
            string itemStr = data.Substring(0, pos + 1);
            data = data.Remove(0, pos + 1);

            //extract type
            pos = itemStr.IndexOf(';');
            string tmp = itemStr.Substring(0, pos + 1);
            itemStr = itemStr.Remove(0, pos + 1);
            tmp = tmp.Replace("type:", "");
            tmp = tmp.Replace(";", "");
           // tmp = tmp.Remove(0, tmp.LastIndexOf('.') + 1);
            string type = tmp;
            
            //extract corner1 point
            pos = itemStr.IndexOf(';');
            tmp = itemStr.Substring(0, pos + 1);
            itemStr = itemStr.Remove(0, pos + 1);
            tmp = tmp.Replace("corner1:(", "");
            tmp = tmp.Replace(");", "");
            pos = tmp.IndexOf('|');
            double x1 = Convert.ToDouble(tmp.Substring(0, pos));
            double y1 = Convert.ToDouble(tmp.Substring(pos + 1));
            var corner1 = new Point(x1, y1);
            
            //extract corner2 point
            pos = itemStr.IndexOf(';');
            tmp = itemStr.Substring(0, pos + 1);
            itemStr = itemStr.Remove(0, pos + 1);
            tmp = tmp.Replace("corner2:(", "");
            tmp = tmp.Replace(");", "");
            pos = tmp.IndexOf('|');
            double x2 = Convert.ToDouble(tmp.Substring(0, pos));
            double y2 = Convert.ToDouble(tmp.Substring(pos + 1));
            var corner2 = new Point(x2, y2);
            
            //extract color
            pos = itemStr.IndexOf(';');
            tmp = itemStr.Substring(0, pos + 1);
            itemStr = itemStr.Remove(0, pos + 1);
            tmp = tmp.Replace("color:", "");
            tmp = tmp.Replace(";", "");
            Color color = Color.FromUInt32( UInt32.Parse(tmp) );
            
            //extract thickness
            pos = itemStr.IndexOf(';');
            tmp = itemStr.Substring(0, pos + 1);
            tmp = tmp.Replace(";", "");
            tmp = tmp.Replace("thickness:", "");
            int thickness = (int) Int32.Parse(tmp);
            
            var newShape = factory.GetShapeByName(type);
            list.Add(newShape(corner1, corner2, color, thickness));

        }

        return list;
    }
}