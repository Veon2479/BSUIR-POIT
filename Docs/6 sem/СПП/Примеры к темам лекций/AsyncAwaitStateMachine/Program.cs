using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AsyncAwaitStateMachine
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Загрузка страницы example.com");
            OutputPageText("http://example.com").GetAwaiter().GetResult();
            OutputPageTextAsync("http://example.com").GetAwaiter().GetResult();
            OutputPageTextAsync_Compiled("http://example.com").GetAwaiter().GetResult();
        }

        static Task OutputPageText(string url)
        {
            var httpClient = new HttpClient();
            return httpClient.GetStringAsync(url).ContinueWith(task =>
                {
                    Console.WriteLine(task.Result);
                });
        }

        static async Task OutputPageTextAsync(string url)
        {
            var httpClient = new HttpClient();
            string result = await httpClient.GetStringAsync(url);
            Console.WriteLine(result);
        }

        static Task OutputPageTextAsync_Compiled(string url)
        {
            var _stateMachine = new _OutputPageTextAsync();
            _stateMachine._builder = AsyncTaskMethodBuilder.Create();
            _stateMachine.url = url;
            _stateMachine._state = -1;
            _stateMachine._builder.Start(ref _stateMachine);
            return _stateMachine._builder.Task;
        }
    }
}
