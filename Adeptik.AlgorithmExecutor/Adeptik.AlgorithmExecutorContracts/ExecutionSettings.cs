using Newtonsoft.Json;

namespace Adeptik.AlgorithmExecutorContracts
{
    /// <summary>
    /// Параметры запуска исполнения алгоритма для решения конкретной задачи
    /// </summary>
    public class ExecutionSettings
    {
        /// <summary>
        /// Относительный либо абсолютный путь к папке, в которой находится содержимое Определения алгоритма
        /// </summary>
        [JsonProperty("algorithmDir")]
        public string AlgorithmDir { get; set; }

        /// <summary>
        /// Относительный либо абсолютный путь к папке, в которой находится содержимое Определения задачи
        /// </summary>
        [JsonProperty("problemDir")]
        public string ProblemDir { get; set; }

        /// <summary>
        /// Параметры Хранилища решений
        /// </summary>
        [JsonProperty("solutionStore")]
        public SolutionStoreSettings SolutionStore { get; set; }
    }
}
