using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Expressions
{
    class Program
    {
        public static Func<int, bool> CreateFunctionFromCSharp()
        {
            Expression<Func<int, bool>> lambda = num => num < 5;
            Console.WriteLine(lambda.ToString());
            Func<int, bool> function = lambda.Compile();
            return function;
        }

        public static Func<int, bool> CreateFunctionUsingExpressionsAPI()
        {
            ParameterExpression numParam = Expression.Parameter(typeof(int), "num");
            ConstantExpression five = Expression.Constant(5, typeof(int));
            BinaryExpression numLessThanFive = Expression.LessThan(numParam, five);
            Expression<Func<int, bool>> lambda = Expression.Lambda<Func<int, bool>>(
                numLessThanFive, numParam);
            Console.WriteLine(lambda.ToString());
            Func<int, bool> function = lambda.Compile();
            return function;
        }

        public static Action<int> CreateProcedureFromCSharp()
        {
            Expression<Action<int>> lambda = (arg) => Console.WriteLine(arg);
            Console.WriteLine(lambda.ToString());
            Action<int> procedure = lambda.Compile();
            return procedure;
        }

        public static Action<int> CreateProcedureUsingExpressionsAPI()
        {
            ParameterExpression param = Expression.Parameter(
                typeof(int), "arg");
            MethodCallExpression methodCall = Expression.Call(
                typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) }), param);
            Expression<Action<int>> lambda = Expression.Lambda<Action<int>>(
                methodCall, param);
            Console.WriteLine(lambda.ToString());
            Action<int> procedure = lambda.Compile();
            return procedure;
        }


        static int Factorial(int value)
        {
            int result = 1;
            while (value > 1)
            {
                result *= value--;
            }
            return result;
        }

        public static Func<int, int> CreateFactorialProcedure()
        {
            ParameterExpression value =
                Expression.Parameter(typeof(int), "value");
            ParameterExpression result =
                Expression.Parameter(typeof(int), "result");
            LabelTarget label = Expression.Label(typeof(int));
            BlockExpression block = Expression.Block(
                new[] { result },
                Expression.Assign(result, Expression.Constant(1)),
                Expression.Loop(
                    Expression.IfThenElse(
                        Expression.GreaterThan(value,
                            Expression.Constant(1)),
                        Expression.MultiplyAssign(result,
                            Expression.PostDecrementAssign(value)),
                        Expression.Break(label, result)
                    ),
                    label)
            );

            Expression<Func<int, int>> lambda = 
                Expression.Lambda<Func<int, int>>(block, value);
            Func<int, int> procedure = lambda.Compile();
            return procedure;
        }

        static void Main()
        {
            Func<int, bool> function1 = CreateFunctionFromCSharp();
            Console.WriteLine("Результат функции: " + function1(4));
            Func<int, bool> function2 = CreateFunctionUsingExpressionsAPI();
            Console.WriteLine("Результат функции: " + function2(4));
            Action<int> procedure1 = CreateProcedureFromCSharp();
            Console.WriteLine("Вызов процедуры:");
            procedure1(4);
            Action<int> procedure2 = CreateProcedureUsingExpressionsAPI();
            Console.WriteLine("Вызов процедуры:");
            procedure2(4);
            Func<int, int> factorial = CreateFactorialProcedure();
            Console.WriteLine("Факториал числа 6 = {0}", factorial(6));
            Console.ReadLine();
        }
    }
}
