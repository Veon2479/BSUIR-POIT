namespace Faker.Tests.Classes;

public struct TestStruct
{
    public int Field;
    public int F1, F2, F3, F4;

    public TestStruct(int field)
    {
        Field = field;
        F1 = 0;
        F2 = 0;
        F3 = 0;
        F4 = 0;
    }
    
    public TestStruct(int field, int f1, int f2)
    {
        Field = field;
        F1 = f1;
        F2 = f2;
        F3 = 0;
        F4 = 0;
    }
    
    

}