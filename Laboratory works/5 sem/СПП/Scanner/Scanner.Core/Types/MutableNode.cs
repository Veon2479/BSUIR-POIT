namespace Scanner.Types;

public class MutableNode
{

    public FileType FileType = FileType.File;
    public long AbsoluteSize = 0;
    public float RelativeSize = 0;
    public string Name = "";
    public string Path = "";
    public List<MutableNode> Children = new List<MutableNode>();

    public Node GetImmutableNode()
    {
        return new Node(this);
    }

    public MutableNode(FileType fileType, long absoluteSize, float relativeSize, string name, string path)
    {
        FileType = fileType;
        AbsoluteSize = absoluteSize;
        RelativeSize = relativeSize;
        Name = name;
        Path = path;
    }
}