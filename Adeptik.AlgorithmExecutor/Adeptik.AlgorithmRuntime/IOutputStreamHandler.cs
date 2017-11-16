using System.IO;

namespace Adeptik.AlgorithmRuntime
{
    /// <summary>
    /// Интерфейс для сохранения данных в поток
    /// </summary>
    public interface IOutputStreamHandler
    {
        /// <summary>
        /// Обработка сохранения данных в поток
        /// </summary>
        /// <param name="output">Поток, в который сохраняются данные</param>
        void Handle(Stream output);
    }
}
