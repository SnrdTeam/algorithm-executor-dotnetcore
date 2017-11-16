namespace Adeptik.AlgorithmRuntime
{
    /// <summary>
    /// Статус решения задачи
    /// </summary>
    public enum SolutionStatus
    {
        /// <summary>
        /// Решение является окончательным. Поиск решения завершен
        /// </summary>
        Final = 0,

        /// <summary>
        /// Решение является промежуточным. Поиск решения продолжается, решение может быть обновлено на более лучшее
        /// </summary>
        Intermediate = 1
    }
}
