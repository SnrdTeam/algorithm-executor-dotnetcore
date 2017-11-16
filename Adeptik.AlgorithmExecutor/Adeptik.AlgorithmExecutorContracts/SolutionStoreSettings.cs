using Newtonsoft.Json;

namespace Adeptik.AlgorithmExecutorContracts
{
    /// <summary>
    /// Описание хранилища решений
    /// </summary>
    public abstract class SolutionStoreSettings
    {
        /// <summary>
        /// Тип хранилища решений
        /// </summary>
        [JsonProperty("_type")]
        public string TypeName { get; set; }
    }
}