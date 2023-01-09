using System.Collections.ObjectModel;

namespace Scanner.Types;

public class Node
{
    public FileType FileType { get; }
    public long AbsoluteSize { get; }
    public  float RelativeSize { get; }
    public  string Name { get; }
    public  string Path { get; }
    
    public bool IsFile { get; }
    
    public ObservableCollection<Node> Children { get; }

    public Node(MutableNode mNode)
    {
        FileType = mNode.FileType;
        AbsoluteSize = mNode.AbsoluteSize;
        RelativeSize = mNode.RelativeSize;
        Name = mNode.Name;
        Path = mNode.Path;
        Children = new ObservableCollection<Node>(mNode.Children.Select(i => i.GetImmutableNode()).ToList());
        if (FileType == FileType.File || FileType == FileType.Link)
            IsFile = true;
        else
            IsFile = false;
    }
}