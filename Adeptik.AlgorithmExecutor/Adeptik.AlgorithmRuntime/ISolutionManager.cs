using System;
using System.IO;

namespace Adeptik.AlgorithmRuntime
{
    /// <summary>
    /// Интерфейс для публикации решения
    /// </summary>
    public interface ISolutionManager
    {
        /// <summary>
        /// Сохранение решения задачи
        /// </summary>
        /// <param name="solutionStatus">Статус решения</param>
        /// <param name="handleSolutionStream">Обработчик потока решения</param>
        /// <exception cref="RetryException">При возникновении данного исключения следует повторить попытку</exception>
        void Post(SolutionStatus solutionStatus, Action<Stream> handleSolutionStream);
    }
}
