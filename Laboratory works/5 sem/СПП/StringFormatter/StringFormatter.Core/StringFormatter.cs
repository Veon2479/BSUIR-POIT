using System.Collections.Concurrent;
using System.Linq.Expressions;
using StringFormatter.Abstractions;

namespace StringFormatter;

public class StringFormatter : IStringFormatter
{
    public static readonly StringFormatter Shared = new StringFormatter();

    private bool _isValid = true;
    
    private List<Token>? GetTokens(string s)
    {
        _isValid = true;
        var list = new List<Token>();
        var i = 0;
        var isWaitingForSingleClose = new Stack<bool>();
        while (_isValid && i < s.Length)
        {
            switch (s[i])
            {
                case '{':
                    if (i + 1 < s.Length)
                    {
                        if (s[i + 1] == '{')
                        {
                            list.Add(new Token(TokenType.DoubleOpen, "{{"));
                            isWaitingForSingleClose.Push(false);
                            i += 1;
                        }
                        else
                        {
                            list.Add(new Token(TokenType.Open, "{"));
                            isWaitingForSingleClose.Push(true);
                        }
                    }
                    else
                        _isValid = false;
                    break;
                case '}':
                    if (isWaitingForSingleClose.Count == 0)
                        _isValid = false;
                    else
                    {
                        var f = isWaitingForSingleClose.Pop();
                        if (f)
                        {
                            list.Add(new Token(TokenType.Close, "}"));
                        }
                        else
                        {
                            if (i + 1 < s.Length)
                            {
                                if (s[i + 1] == '}')
                                {
                                    list.Add(new Token(TokenType.DoubleClose, "}}"));
                                    i += 1;
                                }
                                else
                                {
                                    _isValid = false;
                                }
                            }
                            else
                            {
                                _isValid = false;
                            }
                        }
                    }
                    break;
                default:
                    var j = i;
                    while (j < s.Length && s[j] != '{' && s[j] != '}')
                        j++;
                    list.Add(new Token(TokenType.Symbols, s.Substring(i, j-i)));
                    i = j - 1;
                    break;
            }
            i++;
        }

        if (isWaitingForSingleClose.Count != 0)
            _isValid = false;
        
        return _isValid ? list : null;
    }

    private void ParsingWorker(List<Token> tokens, int l, int r)
    {
        var i = l;
        while (_isValid && i <= r)
        {
            switch (tokens[i].Type)
            {
                case TokenType.Open:
                    if (i <= r - 2)
                    {
                        if (tokens[i + 1].Type == TokenType.Symbols && tokens[i + 2].Type == TokenType.Close)
                            tokens[i + 1].IsIdentifier = true;
                        else
                            _isValid = false;
                    }
                    else
                    {
                        _isValid = false;
                    }

                    i += 3;
                    break;

                case TokenType.DoubleOpen:
                case TokenType.Close:
                case TokenType.DoubleClose:
                case TokenType.Symbols:
                default:
                    i++;
                    break;
            }
        }
        
    }
    
    private void TransformTokens(List<Token> tokens, object target)
    {
        ParsingWorker(tokens, 0, tokens.Count - 1);
        if (_isValid)
            for (var i = 0; i < tokens.Count; i++)
            {
                var item = tokens[i];
                switch (item.Type)
                {
                    case TokenType.Open or TokenType.Close:
                        break;

                    case TokenType.DoubleOpen:
                        item.Content = "{";
                        break;

                    case TokenType.DoubleClose:
                        item.Content = "}";
                        break;

                    case TokenType.Symbols:
                    {
                        if (item.IsIdentifier)
                        {
                            var s = GetSubstitution(target, item.Content);
                            if (s != null)
                            {
                                item.Content = s;
                                tokens[i - 1].Content = "";
                                tokens[i + 1].Content = "";
                            }
                        }

                        break;
                    }
                }
            }
    }
    
    public string Format(string template, object target)
    {
        _isValid = true;
        
        var tokens = GetTokens(template);
        if (!_isValid)
            throw new Exception("Incorrect template syntax");
        
        TransformTokens(tokens!, target);
        if (!_isValid)
            throw new Exception("Incorrect template syntax");

        return tokens!.Aggregate("", (current, token) => current + token.Content);
    }
    
    private readonly ConcurrentDictionary<string, Func<object, string>> Cache = new();

    private string? GetSubstitution(object target, string identifier)
    {
        var key = $"{target.GetType()}.{identifier}";
        if (Cache.TryGetValue(key, out var result))
            return result(target);
        else
        {
            var id = identifier.Split(new char[2] { '[', ']' });

            var fields = target.GetType().GetFields();
            var properties = target.GetType().GetProperties();

            if (fields.Any(field => field.Name == id[0]) ||
                properties.Any(property => property.Name == id[0]))
            {
                var objParam = Expression.Parameter(typeof(object), "obj");
                Expression propertyOrField;
                if (id.Length > 1)
                {
                    var array = Expression.PropertyOrField(Expression.TypeAs(objParam, target.GetType()), id[0]);
                    propertyOrField = Expression.ArrayAccess(array, Expression.Constant(int.Parse(id[1]), typeof(int)));
                }
                else
                {
                    propertyOrField = Expression.PropertyOrField(Expression.TypeAs(objParam, target.GetType()), id[0]);
                }

                var toString = Expression.Call(propertyOrField, "ToString", null, null);
                var func = Expression.Lambda<Func<object, string>>(toString, objParam).Compile();

                result = Cache.GetOrAdd(key, func);

                return result(target);
            }
            else
                return null;
        }
    }
}