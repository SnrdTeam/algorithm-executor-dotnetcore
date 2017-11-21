using Adeptik.AlgorithmRuntime;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Adeptik.AplusBAlgorithm
{
    public class AplusBAlgorithm
    {
        const int RetryPostSolutionMilliSeconds = 1000;
        public static void Run(IContext context)
        {
            Console.WriteLine("Running 'a+b' algorithm");
            using (var reader = new StreamReader(context.InputManager.OpenInput("input.txt")))
            {
                var line = reader.ReadLine().Trim();
                var numbersString = line.Split(new[] { ' ' });
                if (numbersString.Length != 2 || numbersString.Any(x => !decimal.TryParse(x, out decimal number)))
                {
                    Console.WriteLine($"Invalid args: {line}");
                    throw new ArgumentException($"Invalid arguments for algorithm: {line}");
                }
                var numbers = numbersString.Select(x => decimal.Parse(x)).ToList();
                var result = numbers[0] + numbers[1];
                Console.WriteLine($"Result: {result}");
                while(true)
                {
                    try
                    {
                        context.SolutionManger.Post(SolutionStatus.Final, stream =>
                        {
                            var serializer = new JsonSerializer();
                            using (var writer = new StreamWriter(stream))
                            {
                                serializer.Serialize(writer, result);
                            }
                        });
                        break;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Exception thrown: {e.Message}");
                        Thread.Sleep(RetryPostSolutionMilliSeconds);
                    }
                }
                Console.WriteLine("Algorithm finished");
            }
        }
    }
}
