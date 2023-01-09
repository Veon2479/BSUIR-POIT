namespace TestGenerator.Core;

public class AsyncFileReader
{
    public async Task<string> ReadAsync(string filePath) 
    {
        string result = string.Empty;
        using(StreamReader reader = new StreamReader(filePath)) 
        {
            result = await reader.ReadToEndAsync();
        }
        return result;
    }
}