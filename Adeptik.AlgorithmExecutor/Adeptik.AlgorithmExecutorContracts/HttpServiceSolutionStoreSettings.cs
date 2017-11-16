using Newtonsoft.Json;

namespace Adeptik.AlgorithmExecutorContracts
{
    /// <summary>
    /// Настройки Сервиса сбора и хранения решений
    /// </summary>
    public class HttpServiceSolutionStoreSettings: SolutionStoreSettings
    {
        public const string Type = "FileSolutionStore";

        public HttpServiceSolutionStoreSettings()
        {
            this.TypeName = Type;
        }

        /// <summary>
        /// Url cервиса сбора и хранения решений
        /// </summary>
        [JsonProperty("serverUrl")]
        public string ServerUrl { get; set; }

        /// <summary>
        /// Заголовок авторизации, который будет передаваться с каждым запросом при сохранении решения
        /// </summary>
        [JsonProperty("Authorization")]
        public string Authrozation { get; set; }
    }
}
