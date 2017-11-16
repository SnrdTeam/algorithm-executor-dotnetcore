using Newtonsoft.Json;

namespace Adeptik.AlgorithmExecutorContracts
{
    /// <summary>
    /// Результат проверки Определения алгоритма на совместимость и корректность
    /// </summary>
    public class AlgorithmCheckResult
    {
        /// <summary>
        /// Корректен алгоритм да/нет
        /// </summary>
        [JsonProperty("validAlgorithm")]
        public bool ValidAlgorithm { get; set; }
    }
}
