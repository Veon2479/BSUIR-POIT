namespace StringFormatter.Abstractions;

public interface IStringFormatter
{
    string Format(string template, object target);
}