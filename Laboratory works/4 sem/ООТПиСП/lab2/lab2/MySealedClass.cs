namespace lab2;

public sealed class MySealedClass : MyRealClass     //cannot be inherited
{
    MySealedClass()
    {
        //SomeValue = 63;  //error due to readonly
    }
    public override void ChangeName(String newName)
    {
        Name = "AAAAAAAAA" + newName + Name + newName; 
    }
    
    /*public sealed override void PrintName()  //cannot do it due to sealed
    {
        Console.WriteLine("My name is here: " + Name);
    }*/
    
}