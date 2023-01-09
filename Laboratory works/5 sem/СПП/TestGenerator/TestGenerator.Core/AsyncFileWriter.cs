namespace TestGenerator.Core;

public class AsyncFileWriter
{
    public async Task WriteAsync(string filePath, string content)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            await writer.WriteAsync(content);
        }
    }
}