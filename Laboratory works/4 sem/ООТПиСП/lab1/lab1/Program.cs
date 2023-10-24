
Console.WriteLine("Started..");
Obj.StaticFunc();
Obj myObj = new Obj(2, 6);
myObj.PrintSmth();
myObj.Field2 = 4;
myObj.PrintSmth(myObj.Field);
myObj.Field = 7;
myObj.PrintSmth();



class Obj
{
    private int _field1 = 0;
    public int Field2 = 0;
    public int Field
    {
        get { return _field1; }
        set { _field1 = value; }
    }
    public void PrintSmth()
    {
        Console.WriteLine("\nSomething is: "+(this._field1+this.Field2));
        Console.WriteLine("But field1 = "+_field1+", and field2 = "+Field2);
    }
    public void PrintSmth(int param)
    {
        Console.WriteLine("\nAnother one something is: "+(this._field1+this.Field2)*param);
    } 
    public Obj(int f1, int f2)
    {
        _field1 = f1;
        Field2 = f2;
    }
    ~Obj()
    {
        Console.WriteLine("\nMr *, i don't feel so good..\nPress enter..");
       // Console.Read();
    }
    public static void StaticFunc()
    {
        Console.WriteLine("This method is static..");
    }
}

