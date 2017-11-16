using Newtonsoft.Json;

namespace Adeptik.AlgorithmExecutorContracts
{
    /// <summary>
    /// Информация об исполнителе
    /// </summary>
    public class AboutExecutor
    {
        /// <summary>
        /// Имя исполнителя
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Версия реализации исполнителя
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Имена поддерживаемых данным исполнителем сред исполнения
        /// </summary>
        [JsonProperty("supportedRuntimes")]
        public string[] SupportedRuntimes { get; set; }
    }
}
