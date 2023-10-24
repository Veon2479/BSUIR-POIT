namespace lab2;

public class MyRealClass : MyAbsClass
{
    public override void ChangeName(String newName)
    {
        Name = newName + Name + newName;
    }

    public sealed override void PrintName()
    {
        Console.WriteLine("My name is here: " + Name);
    }

    public readonly int SomeValue = 42;
}