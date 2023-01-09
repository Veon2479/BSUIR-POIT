using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestGenerator.Core.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        _engine = new GeneratorEngine();
    }

    private GeneratorEngine _engine;

    private List<Test> TryGenerate(string source)
    {
        var result = new List<Test>();
        var isDropped = false;

        try
        {
            result = _engine.Generate(source);
        }
        catch (Exception e)
        {
            isDropped = true;
        }

        if (isDropped)
            _engine = new GeneratorEngine();

        return result;
    }

    private List<List<string>> GetMethods(string source)
    {
        var methods = new List<List<string>>();
        CompilationUnitSyntax? root = null;
        try
        {
            root = CSharpSyntaxTree.ParseText(source).GetCompilationUnitRoot();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (root != null)
        {
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where(classItem => classItem.Modifiers.Any(SyntaxKind.PublicKeyword));

            foreach (var classItem in classes)
            {
                // tests.Add(GenerateTest(classItem, usings, isScopedNamespacesExist, scopedNamespace));
                var sourceMethods = classItem.DescendantNodes().OfType<MethodDeclarationSyntax>()
                    .Where(sourceMethod => sourceMethod.Modifiers.Any(SyntaxKind.PublicKeyword)).ToList();
                var list = sourceMethods.Select(i => (string)i.Identifier.ToString()).ToList();
                methods.Add(list);
            }
        }
        
        return methods;
    }
    
    private List<string> GetClasses(string source)
    {
        var sourceClasses = new List<string>();
        CompilationUnitSyntax? root = null;
        try
        {
            root = CSharpSyntaxTree.ParseText(source).GetCompilationUnitRoot();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (root != null)
        {
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .Where(classItem => classItem.Modifiers.Any(SyntaxKind.PublicKeyword));

            foreach (var classItem in classes)
            {
                // tests.Add(GenerateTest(classItem, usings, isScopedNamespacesExist, scopedNamespace));
                sourceClasses.Add(classItem.Identifier.ToString());
            }
        }
        
        return sourceClasses;
    }
    
    [Test]
    public void IncorrectInputTest()
    {
        var source = @"
            namespace TestGenerator.Core;
            FGHJHGFGHJKJGFGHJKJGFGHJKJHG";
        var res = TryGenerate(source);
        Assert.That(res.Count, Is.EqualTo(0));
    }
    
    [Test]
    public void SimpleClassTest()
    {
        var source = @"public class SimpleClass
            {
                public string StaticMethodToTest(int number)
                {
                    return ""aa"";
                }
            }";
        var res = TryGenerate(source);
        
        Assert.Multiple(() =>
        {
            Assert.That(res.Count(), Is.EqualTo(1));
            
            var tests = GetMethods(res[0].Content);
            Assert.That(tests[0].Count, Is.EqualTo(1));
        });
        
    }
    
    [Test]
    public void StaticClassTest()
    {
        var source = @"public static class StaticClass
            {
                public static string StaticMethodToTest(int number)
                {
                    return ""aa"";
                }
            }";
        var res = TryGenerate(source);
        
        Assert.Multiple(() =>
        {
            Assert.That(res.Count(), Is.EqualTo(1));
            
            var tests = GetMethods(res[0].Content);
            Assert.That(tests[0].Count, Is.EqualTo(1));
        });
        
    }
    
    [Test]
    public void MultipleClassTest()
    {
        var source = @"public static class StaticClass
            {
                public static string StaticMethodToTest(int number)
                {
                    return ""aa"";
                }
            }

public class SimpleClass
            {
                public string StaticMethodToTest(int number)
                {
                    return ""aa"";
                }
            }";
        var res = TryGenerate(source);
        
        Assert.Multiple(() =>
        {
            Assert.That(res.Count(), Is.EqualTo(2));
            
            var tests = GetMethods(res[0].Content);
            Assert.That(tests[0].Count, Is.EqualTo(1));
            
            tests = GetMethods(res[1].Content);
            Assert.That(tests[0].Count, Is.EqualTo(1));
        });
        
    }
    
    [Test]
    public void OverridedMethodsTest()
    {
        var source = @"public class SimpleClass
            {
                public string StaticMethodToTest(int number)
                {
                    return ""aa"";
                }

public string StaticMethodToTest(int number, string str)
                {
                    return ""aa"";
                }

public string StaticMethodToTest()
                {
                    return ""aa"";
                }
            }";
        var res = TryGenerate(source);
        
        Assert.Multiple(() =>
        {
            Assert.That(res.Count(), Is.EqualTo(1));
            
            var tests = GetMethods(res[0].Content);
            Assert.That(tests[0].Count, Is.EqualTo(3));
        });
        
    }
    
    [Test]
    public void NoPublicMethodsTest()
    {
        var source = @"public class SimpleClass
            {
                private string StaticMethodToTest(int number)
                {
                    return ""aa"";
                }
            }";
        var res = TryGenerate(source);
        
        Assert.Multiple(() =>
        {
            Assert.That(res.Count(), Is.EqualTo(1));
            
            var tests = GetMethods(res[0].Content);
            Assert.That(tests[0].Count, Is.EqualTo(0));
        });
    }
    
}