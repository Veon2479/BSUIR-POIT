using System.Threading.Tasks.Dataflow;

namespace TestGenerator.Core;

public class TestGenerator
{
    
    private readonly uint _maxReadingFiles, _maxGeneratingFiles, _maxWritingFiles;
    private readonly AsyncFileReader _reader;
    private readonly AsyncFileWriter _writer;
    private readonly GeneratorEngine _engine;
    
    public TestGenerator(uint maxReadingFiles, uint maxGeneratingFiles, uint maxWritingFiles)
    {
        _maxReadingFiles = (maxReadingFiles > 1) ? maxReadingFiles : 1;
        _maxGeneratingFiles = (maxGeneratingFiles > 1) ? maxGeneratingFiles : 1;
        _maxWritingFiles = (maxWritingFiles > 1) ? maxWritingFiles : 1;

        _reader = new AsyncFileReader();
        _writer = new AsyncFileWriter();

        _engine = new GeneratorEngine();
    }
    
    public async Task<List<string>> GenerateAsync(List<string> srcPaths, string destPath)
    {
        var readingOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = (int)_maxReadingFiles };
        var writingOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = (int)_maxWritingFiles };
        var generatorOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = (int)_maxGeneratingFiles };
        
        var filesToProcess = new List<string>();

        if(!Directory.Exists(destPath))
        {
            Directory.CreateDirectory(destPath);
        }
        foreach(var sourceFilePath in srcPaths)
        {
            if(File.Exists(sourceFilePath))
            {
                filesToProcess.Add(sourceFilePath);
            }
        }

        var readFile = new TransformBlock<string, string>(async filePath =>
            await _reader.ReadAsync(filePath), readingOptions);

        var generateTest = new TransformManyBlock<string, KeyValuePair<string, string>>(sourceFile =>
            ComposeResultTestFiles(sourceFile, destPath), generatorOptions);

        var writeFile = new ActionBlock<KeyValuePair<string, string>>(async pathContent =>
            await _writer.WriteAsync(pathContent.Key, pathContent.Value), writingOptions);

        var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

        readFile.LinkTo(generateTest, linkOptions);
        generateTest.LinkTo(writeFile, linkOptions);

        foreach(var filePath in filesToProcess)
        {
            readFile.Post(filePath);
        }
        readFile.Complete();

        await writeFile.Completion;

        return filesToProcess;

    }
    
    private Dictionary<string, string> ComposeResultTestFiles(string sourceContent, string destPath)
    {
        var pathContent = new Dictionary<string, string>();
        var testClasses = _engine.Generate(sourceContent);
        foreach (var test in testClasses)
        {
            var filePath = Path.Combine(destPath, test.ClassName + "Tests");
            while (pathContent.ContainsKey(filePath + ".cs"))
            {
                filePath = $"{filePath}_1";
            }
            filePath += ".cs";
            pathContent.Add(filePath, test.Content);
        }
        return pathContent;
    }
    
    public async Task<List<string>> GenerateAsync(string srcDirPath, string destPath)
    {
        var filePaths = new List<string>();
        var files = Directory.GetFiles(srcDirPath, "*.cs");
        if (files != null)
        {
            filePaths.AddRange(files);
        }
        return await GenerateAsync(filePaths, destPath);
    }

}