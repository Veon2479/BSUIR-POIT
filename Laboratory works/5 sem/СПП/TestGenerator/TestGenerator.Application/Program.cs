if (args.Length == 1)
{
    Console.WriteLine("LOG: specified path is: {0}", args[0]);
    if (Directory.Exists(args[0]))
    {
        Console.WriteLine("LOG: full path is: {0}", Path.GetFullPath(args[0]));
        
        var generator = new TestGenerator.Core.TestGenerator(2, 2, 2);
        if (Directory.Exists("res"))
        {
            Console.WriteLine("LOG: cleaning previous data..");
            Directory.Delete("res", recursive: true);
        }
        Directory.CreateDirectory("res");

        var res = await generator.GenerateAsync(args[0], "res");
        Console.WriteLine("LOG: tests generated for:");
        foreach (var file in res)
        {
            Console.WriteLine("     {0}", file);
        }
    }
    else
    {
        Console.WriteLine("ERR: no such directory!");
    }
}
else
{
    Console.WriteLine("ERR: Incorrect number of parameters!");
}