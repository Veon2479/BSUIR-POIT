namespace StringFormatter.Tests;

public class Tests
{
    [Test]
    public void EmptyStringTest()
    {
        const string str = "ASDFGHjkErtyuioKJHGFDfg4567iJHG";
        var res = "";
        try
        {
            res = StringFormatter.Shared.Format(str, new object());
        }
        catch (Exception e)
        {
            Assert.Fail();
        }
        Assert.That(res, Is.EqualTo(str));
    }
    
    [Test]
    public void RealEmptyStringTest()
    {
        const string str = "";
        var res = "";
        try
        {
            res = StringFormatter.Shared.Format(str, new object());
        }
        catch (Exception e)
        {
            Assert.Fail();
        }
        Assert.That(res, Is.EqualTo(str));
    }
    
    [Test]
    public void SimpleShieldedTest()
    {
        const string str = "hello there {{general}} kenobi";
        var res = "";
        try
        {
            res = StringFormatter.Shared.Format(str, new object());
        }
        catch (Exception e)
        {
            Assert.Fail();
        }
        Assert.That(res, Is.EqualTo("hello there {general} kenobi"));
    }
    
    [Test]
    public void IncorrectSyntaxTest()
    {
        const string str = "{ {{ } }}";
        var res = "";
        try
        {
            res = StringFormatter.Shared.Format(str, new object());
        }
        catch (Exception e)
        {
            Assert.That(e.Message, Is.EqualTo("Incorrect template syntax"));
            return;
        }
        Assert.Fail();
    }
    
    [Test]
    public void EmptyFieldTest()
    {
        const string str = "{Name}";
        var res = "";
        try
        {
            res = StringFormatter.Shared.Format(str, new object());
        }
        catch (Exception e)
        {
            Assert.Fail();
        }
        Assert.That(res, Is.EqualTo("{Name}"));
    }
    
    [Test]
    public void PseudoFieldTest()
    {
        const string str = "{{Name}}";
        var res = "";
        try
        {
            res = StringFormatter.Shared.Format(str, new object());
        }
        catch (Exception e)
        {
            Assert.Fail();
        }
        Assert.That(res, Is.EqualTo("{Name}"));
    }
    
    [Test]
    public void SimpleCorrectFieldTest()
    {
        const string str = "{IsIdentifier} - {Content} - {Type}";
        var res = "";
        var tk = new Token(TokenType.Symbols, "{");
        try
        {
            res = StringFormatter.Shared.Format(str, tk);
        }
        catch (Exception e)
        {
            Assert.Fail();
        }
        Assert.That(res, Is.EqualTo($"{tk.IsIdentifier} - {tk.Content} - {tk.Type}"));
    }
    
    [Test]
    public void ComplexCorrectFieldTest()
    {
        const string str = "{{{{{IsIdentifier}}} - {Content}}} - {Type}";
        var res = "";
        var tk = new Token(TokenType.Symbols, "{");
        try
        {
            res = StringFormatter.Shared.Format(str, tk);
        }
        catch (Exception e)
        {
            Assert.Fail();
        }
        Assert.That(res, Is.EqualTo($"{{{{{tk.IsIdentifier}}} - {tk.Content}}} - {tk.Type}"));
    }

    [Test]
    public void ArrayAccessTest()
    {
        const string str = "{{  {{  [  }}   {Array[0]} {Array[1]} {Array[2]}}}";
        var res = "";
        var tk = new TestingClass();
        try
        {
            res = StringFormatter.Shared.Format(str, tk);
        }
        catch (Exception e)
        {
            Assert.Fail();
        }
        Assert.That(res, Is.EqualTo($"{{  {{  [  }}   {tk.Array[0]} {tk.Array[1]} {tk.Array[2]}}}"));
    }
    
    [Test]
    public void AnotherArrayAccessTest()
    {
        const string str = "{List[0]} {List[1]}gh, {List[2]}!";
        var res = "";
        var tk = new TestingClass();
        try
        {
            res = StringFormatter.Shared.Format(str, tk);
        }
        catch (Exception e)
        {
            Assert.Fail();
        }
        Assert.That(res, Is.EqualTo($"{tk.List[0]} {tk.List[1]}gh, {tk.List[2]}!"));
    }

    [Test]
    public async Task MultiThreadAccessTest()
    {
        const string str = "{IsIdentifier} - {Content} - {Type}";
        
        var res = "";
        var tk = new Token(TokenType.Symbols, "{");

        var action = () =>
        {
            try
            {
                res = StringFormatter.Shared.Format(str, tk);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }

            Assert.That(res, Is.EqualTo($"{tk.IsIdentifier} - {tk.Content} - {tk.Type}"));
        };
        

        var i = Random.Shared.Next() % 100 + 3;
        var tasks = new Task[i];
        
        for (var index = 0; index < tasks.Length; index++)
        {
            tasks[index] = Task.Run(action);
        }

        foreach (var task in tasks)
        {
            // task.Wait();
            await task;
        }
        
    }
}