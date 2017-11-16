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
        /// <param name="outputStreamHandler">Объект для осуществления сохранения решения</param>
        void Post(SolutionStatus solutionStatus, IOutputStreamHandler outputStreamHandler);
    }
}
