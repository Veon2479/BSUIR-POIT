namespace Scanner.Types;

public class MutableScannerResult
{
    public List<MutableNode> Children = new List<MutableNode>();
    public long Size = 0;
    public string Path = "";

    public ScannerResult GetScannerResult()
    {
        return new ScannerResult(this);
    }
}