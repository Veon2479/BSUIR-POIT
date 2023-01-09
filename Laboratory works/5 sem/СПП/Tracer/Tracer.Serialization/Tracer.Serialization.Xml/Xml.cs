using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Serialization.Abstractions;

namespace Tracer.Serialization.Xml;

public class Xml : ITraceResultSerializer
{
    public Xml()
    {
        Format = "Xml";
    }

    public string Format { get; }
    public void Serialize(TraceResult traceResult, Stream to)
    {
        XmlDocument doc = new();
        doc.PreserveWhitespace = true;
        var xmlDec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
        doc.AppendChild(xmlDec);

        var node = doc.CreateElement("MethodTrees");
        foreach (var i in traceResult.Children)
        {
            MakeTree(i, ref node, doc);
        }
        doc.AppendChild(node);
        //doc.Save(to);


        using (StreamWriter writer = new(to))
        {
            //writer.WriteLine(doc.OuterXml);
            var xtw = new XmlTextWriter(writer);
            xtw.Formatting = Formatting.Indented;
            doc.WriteTo(xtw);
        }
    }

    private void MakeTree(TraceResult treeNode, ref XmlElement xmlNode, XmlDocument doc)
    {
        var newNode = doc.CreateElement("Method");

        var prIsReady = doc.CreateElement("IsReady");
        prIsReady.InnerText = treeNode.IsReady.ToString();
        newNode.AppendChild(prIsReady);
        
        var prPid = doc.CreateElement("TID");
        prPid.InnerText = treeNode.Tid.ToString();
        newNode.AppendChild(prPid);
        
        var prTime = doc.CreateElement("Time");
        prTime.InnerText = treeNode.Time.ToString();
        newNode.AppendChild(prTime);
        
        var prMethod = doc.CreateElement("Name");
        prMethod.InnerText = treeNode.Method;
        newNode.AppendChild(prMethod);
        
        var prClassName = doc.CreateElement("ClassName");
        prClassName.InnerText = treeNode.ClassName;
        newNode.AppendChild(prClassName);

        var prChildren = doc.CreateElement("Children");
        newNode.AppendChild(prChildren);
        
        foreach (var i in treeNode.Children)
        {
            MakeTree(i, ref prChildren, doc);
        }
        xmlNode.AppendChild(newNode);
    }
    

}
