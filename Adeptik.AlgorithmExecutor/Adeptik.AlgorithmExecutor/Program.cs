using Adeptik.AlgorithmExecutorContracts;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace Adeptik.AlgorithmExecutor
{
    class Program
    {
        const string ExecutorName = "DotNetCoreExecutor";
        static readonly string[] SupportedVersions = new string[] { ".netcore2.0" };

        static void Main(string[] args)
        {

            if (args.Length == 1 && args[0] == "--hepp")
                PrintHelp();
            else if (args.Length == 1 && args[0] == "--about")
                PrintAbout();
            else if (args.Length == 2 && args[0] == "--check")
                CheckAlgorithm(args[1]);
            else if (args.Length == 2 && args[0] == "--start")
                StartAlgorithm(args[1]);
            else
                Console.WriteLine($"Invalid command line args: {string.Join(",", args)}");
        }

        private static void StartAlgorithm(string executionSettingsPath)
        {
            throw new NotImplementedException();
        }

        private static void CheckAlgorithm(string algorithmPath)
        {
            throw new NotImplementedException();
        }

        private static void PrintAbout()
        {
            var about = new AboutExecutor
            {
                Version = typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version,
                Name = ExecutorName,
                SupportedRuntimes = SupportedVersions
            };
            Console.WriteLine(JsonConvert.SerializeObject(about));
        }

        private static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Usage: --about");
            Console.WriteLine("Usage: --check <algorithm folder>");
            Console.WriteLine("Usage: --start <execution settings file>");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --help    prints this help");
            Console.WriteLine("  --about   prints information about this executor in json");
            Console.WriteLine("  --check   checks algorithm execution possibility in <algorithm folder>");
            Console.WriteLine("  --start   starts algorithm execution with settings stored in <execution settings file>");
        }
    }
}
