using Adeptik.AlgorithmExecutorContracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Adeptik.AlgorithmExecutor
{
    class Program
    {
        const string ExecutorName = "DotNetCoreExecutor";
        static readonly string[] SupportedVersions = new string[] { "2.0.3" };
        const string EntryPointFileName = "entryPoint";
        const string RuntimeVersionPrefix = ".netcore";
        const string RuntimesFolderName = "runtimes";
        const string LibsFolderName = "libs";

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
                Console.WriteLine($"Invalid command line args: {string.Join(",", args)}. For help enter --help");
        }


        /// <summary>
        /// Вывод информации об исполнителе
        /// <seealso cref="AboutExecutor"/>
        /// </summary>
        private static void PrintAbout()
        {
            var about = new AboutExecutor
            {
                Version = typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version,
                Name = ExecutorName,
                SupportedRuntimes = SupportedVersions.Select(x => RuntimeVersionPrefix + x).ToArray()
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

        /// <summary>
        /// Запуск алгоритма
        /// </summary>
        /// <param name="executionSettingsPath">Путь json файлу настроеек запуска алгоритма
        /// <seealso cref="ExecutionSettings"/>
        /// </param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        private static void StartAlgorithm(string executionSettingsPath)
        {
            if (executionSettingsPath == null)
                throw new ArgumentNullException(nameof(executionSettingsPath));

            if (!File.Exists(executionSettingsPath))
                throw new ArgumentException($"File: {executionSettingsPath} not found");

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.Converters.Add(new SolutionStoreSettingsJsonConverter());
            var executionSettings = JsonConvert.DeserializeObject<ExecutionSettings>(File.ReadAllText(executionSettingsPath), jsonSettings);
            PrepareAlgorithmForRun(executionSettings).Invoke(null, new object[] { new ExecutionContext(executionSettings) });
        }

        private static MethodInfo PrepareAlgorithmForRun(ExecutionSettings executionSettings)
        {
            if (executionSettings == null)
                throw new ArgumentNullException(nameof(executionSettings));

            var entryPoint = ValidateAlgorithm(executionSettings.AlgorithmDir);
            var assembly = Assembly.LoadFrom(entryPoint.Assembly);
            var type = assembly.GetType(entryPoint.ClassName);
            return type.GetMethod(entryPoint.MethodName);

        }



        /// <summary>
        /// Вывод результата проверки корректности опеределения алгоритма
        /// <seealso cref="AlgorithmCheckResult"/>
        /// </summary>
        /// <param name="algorithmPath">Путь к разархивированной папке определения алгоритма</param>
        private static void CheckAlgorithm(string algorithmPath)
        {
            if (algorithmPath == null)
                throw new ArgumentNullException(nameof(algorithmPath));
            var valid = false;
            try
            {
                ValidateAlgorithm(algorithmPath);
                valid = true;
            }
            catch(Exception) { }
            var check = new AlgorithmCheckResult { ValidAlgorithm = valid };
            Console.WriteLine(JsonConvert.SerializeObject(check));
        }

        /// <summary>
        /// Проверка корректности определения алгоритма
        /// </summary>
        /// <param name="baseAlgorithmPath">Путь к папке определения</param>
        /// <returns>Точка входа</returns>
        /// <exception cref="ArgumentException">В случае ошибки генерируется исключение</exception>
        private static EntryPoint ValidateAlgorithm(string baseAlgorithmPath)
        {
            if (!Directory.Exists(baseAlgorithmPath))
                throw new ArgumentException($"Algorithm folder: {baseAlgorithmPath} not found");
            var runtimesPath = Path.Combine(baseAlgorithmPath, "runtimes");
            if (!Directory.Exists(runtimesPath))
                throw new ArgumentException($"Runtimes folder: { runtimesPath } not found");


            var algorithmRuntimeVersions = (new DirectoryInfo(runtimesPath)).GetDirectories()
                .Where(x => x.Name.StartsWith(RuntimeVersionPrefix))
                .Select(x => x.Name.Substring(RuntimeVersionPrefix.Length)).ToList();
            if (algorithmRuntimeVersions.Count == 0)
                throw new ArgumentException($"Runtimes folder is empty or has invalid runtime directories. Directories should start with: {RuntimeVersionPrefix}");

            var lastValidVersion = algorithmRuntimeVersions.Where(x => SupportedVersions.Contains(x)).OrderByDescending(x => x).FirstOrDefault();
            if(lastValidVersion == null)
                throw new ArgumentException($"Runtime versions: {string.Join(", ", algorithmRuntimeVersions)} does not match supported runtime versions: {string.Join(", ", SupportedVersions)}");

            return GetAlgorithmEntryPoint(Path.Combine(baseAlgorithmPath, RuntimesFolderName, RuntimeVersionPrefix + lastValidVersion));
        }





        /// <summary>
        /// Получение информации о точке входа в алгоритм
        /// </summary>
        /// <param name="algorithmPath">Путь к директории, содержащей сборку алгоритма и все необходимые сборки</param>
        /// <returns>Описание точки входа</returns>
        /// <exception cref="ArgumentException">В случае ошибки генерируется исключение</exception>
        private static EntryPoint GetAlgorithmEntryPoint(string algorithmPath)
        {
            var entryPointFilePath = Path.Combine(algorithmPath, EntryPointFileName);
            if (!File.Exists(entryPointFilePath))
                throw new ArgumentException($"EntryPoint file:{EntryPointFileName} not found");

            var lines = File.ReadAllLines(entryPointFilePath);
            if(lines.Length < 2)
                throw new ArgumentException($"Invalid format of EntryPoint file:{EntryPointFileName}");

            var assemblyName = lines[0];

            var libsPath = Path.Combine(algorithmPath, LibsFolderName);
            if (!Directory.Exists(libsPath))
                throw new ArgumentException($"LibsFolder:{libsPath} not found");

            var assemblyPath = Path.Combine(libsPath, assemblyName);
            if (!File.Exists(Path.Combine(assemblyPath)))
                throw new ArgumentException($"Assembly file:{assemblyPath} not found");

            var methodWithClass = lines[1];

            var index = methodWithClass.LastIndexOf(".");
            if (index == -1 || index == 0 || index == methodWithClass.Length - 1)
                throw new ArgumentException($"Invalid format of EntryPoint file: {EntryPointFileName} for class/method");            

            var className = methodWithClass.Substring(0, index);
            var method = methodWithClass.Substring(index + 1);

            return new EntryPoint { Assembly = Path.Combine(algorithmPath, libsPath, assemblyName), ClassName = className, MethodName = method };

        }

        /// <summary>
        /// Описание точки входа в алгоритм
        /// </summary>
        private class EntryPoint
        {
            /// <summary>
            /// Имя сборки с алгоритмом, содержащей алгоритм
            /// </summary>
            public string Assembly { get; set; }

            /// <summary>
            /// Имя класса
            /// </summary>
            public string ClassName { get; set; }

            /// <summary>
            /// Имя статического метода, инициирующего запуск алгоритма
            /// </summary>
            public string MethodName { get; set; }
        }

        /// <summary>
        /// Json-конвертер для абстрактного класса <see cref="SolutionStoreSettings"/>
        /// </summary>
        private class SolutionStoreSettingsJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return typeof(SolutionStoreSettings) == objectType;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var jsonObject = JObject.Load(reader);
                switch(jsonObject["_type"].Value<string>())
                {
                    case FileSolutionStoreSettings.Type:
                        return JsonConvert.DeserializeObject<FileSolutionStoreSettings>(jsonObject.ToString());
                    case HttpServiceSolutionStoreSettings.Type:
                        return JsonConvert.DeserializeObject<HttpServiceSolutionStoreSettings>(jsonObject.ToString());
                    default:
                        throw new ArgumentOutOfRangeException("_type", jsonObject["_type"].Value<string>(), "Unknown type");
                }
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException(); // won't be called because CanWrite returns false
            }
        }
    }
}
