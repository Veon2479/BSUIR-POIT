using System;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace AsyncAwaitStateMachine
{
    public sealed class _OutputPageTextAsync : IAsyncStateMachine
	{
		public string url;
		private HttpClient httpClient;
		private string result;

		public int _state; // состояние конечного автомата
		public AsyncTaskMethodBuilder _builder; // хранит объект Task, возвращаемый из async-метода OutputPageText
		private TaskAwaiter<string> _awaiter;

		public void MoveNext()
		{
			try
			{
				TaskAwaiter<string> awaiter;
				if (_state != 0)
				{
					// Код, запускаемый до await:
					httpClient = new HttpClient();
					// Код, соответствующий оператору await:
					awaiter = httpClient.GetStringAsync(url).GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						_state = 0;
						_awaiter = awaiter;
                        _OutputPageTextAsync stateMachine = this;
						// В очередь к задаче ставится вызов этого же метода (MoveNext):
						_builder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
						return;
					}
				}
				else
				{
					awaiter = _awaiter;
					_awaiter = new TaskAwaiter<string>();
					_state = -1;
				}
				result = awaiter.GetResult();
				// Код, запускаемый после оператора await:
				Console.WriteLine(result);
			}
			catch (Exception exception)
			{
				_state = -2;
				httpClient = null;
				result = null;
				// Завершение задачи с ошибкой
				_builder.SetException(exception);
				return;
			}
			_state = -2;
			httpClient = null;
			result = null;
			// Успешное завершение задачи:
			_builder.SetResult();
		}

		public void SetStateMachine(IAsyncStateMachine stateMachine)
		{
		}
	}
}
