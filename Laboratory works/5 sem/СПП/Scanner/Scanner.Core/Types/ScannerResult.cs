using System.Collections.ObjectModel;

namespace Scanner.Types;

public class ScannerResult
{
    public ObservableCollection<Node> Children { get; }
    public readonly long Size;
    public readonly string Path;
    public ScannerResult(MutableScannerResult sr)
    {
        Children = new ObservableCollection<Node>(sr.Children.Select(i => i.GetImmutableNode()).ToList());
        Size = sr.Size;
        Path = sr.Path;
    }
}