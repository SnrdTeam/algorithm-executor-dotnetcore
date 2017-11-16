namespace Adeptik.AlgorithmRuntime
{
    /// <summary>
    /// Контекст запуска алгоритма для решения определенной задачи
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Объект для манипулирования входными данными задачи
        /// </summary>
        IInputManager InputManager { get; }

        /// <summary>
        /// Объект для манипулирования решением задачи
        /// </summary>
        ISolutionManager SolutionManger { get; }
    }
}
