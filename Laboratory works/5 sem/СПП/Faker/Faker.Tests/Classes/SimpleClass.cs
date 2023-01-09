namespace Faker.Tests.Classes;

public class SimpleClass
{
    public int intField;
    public string stringField;
    private bool _boolField;
    public bool BoolField
    {
        get => _boolField;
        set => _boolField = value;
    }

    public int Code;
    public char Symbol;

    public SimpleClass(int code)
    {
        Code = code;
        Symbol = 'F';
    }
}