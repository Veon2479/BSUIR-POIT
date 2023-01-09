namespace StringFormatter;

public class Token
{
    public TokenType Type;
    public string Content;
    public bool IsIdentifier;

    public Token(TokenType type, string content)
    {
        Type = type;
        Content = content;
        IsIdentifier = false;
    }
}