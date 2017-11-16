using System.IO;

namespace Adeptik.AlgorithmRuntime
{
    /// <summary>
    /// Интерфейс для чтения данных из потока
    /// </summary>
    public interface IInputManager
    {
        /// <summary>
        /// Открывает для чтения входных данных из определения задачи по имени ресурса
        /// </summary>
        /// <param name="name">Имя ресурса</param>
        /// <returns>Поток для чтения содержимого входных данных</returns>
        Stream OpenInput(string name);
    }
}
