using Newtonsoft.Json;

namespace Adeptik.AlgorithmExecutorContracts
{
    /// <summary>
    /// Настройки файлового хранилища решений
    /// </summary>
    public class FileSolutionStoreSettings : SolutionStoreSettings
    {
        public const string Type = "FileSolutionStore";

        public FileSolutionStoreSettings()
        {
            this.TypeName = Type;
        }

        /// <summary>
        /// Относительный либо абсолютный путь к папке, в которую должны сохраняться решения задачи
        /// </summary>
        [JsonProperty("solutionsDir")]
        public string SolutionsDir { get; set; }
    }
}
