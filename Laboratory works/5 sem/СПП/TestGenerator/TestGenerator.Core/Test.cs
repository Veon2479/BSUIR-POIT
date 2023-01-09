namespace TestGenerator.Core;

public class Test
{
    public string ClassName { get; }
    public string Content { get; }

    public Test(string className, string content)
    {
        ClassName = className;
        Content = content;
    }
}