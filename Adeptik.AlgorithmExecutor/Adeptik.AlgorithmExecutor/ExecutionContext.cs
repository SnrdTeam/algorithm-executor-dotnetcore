using Adeptik.AlgorithmExecutorContracts;
using Adeptik.AlgorithmRuntime;
using System;

namespace Adeptik.AlgorithmExecutor
{
    /// <summary>
    /// Реализация контекста запуска Алгоритма для решения определенной Задачи
    /// </summary>
    public class ExecutionContext : IContext
    {
        private readonly IInputManager _inputManager;
        private readonly ISolutionManager _solutionManager;

        public ExecutionContext(ExecutionSettings executionSettings)
        {
            if (executionSettings == null)
                throw new ArgumentNullException(nameof(executionSettings));
            if (executionSettings.ProblemDir == null)
                throw new ArgumentNullException("Problem directory is not specified");

            _inputManager = new InputManagerImpl(executionSettings.ProblemDir);

            if (executionSettings.SolutionStore == null)
                throw new ArgumentNullException("Solution Store Server is not specified");
            if (executionSettings.SolutionStore is FileSolutionStoreSettings settings)
                _solutionManager = new SolutionManagerFileImpl(settings.SolutionsDir);
            else if (executionSettings.SolutionStore is HttpServiceSolutionStoreSettings httpSettings)
            {
                _solutionManager = new SolutionManagerHttpImpl(httpSettings.ServerUrl, httpSettings.Authrozation);
            }
            else
                throw new ArgumentException("Unknown type of solution store");
        }

        public IInputManager InputManager => _inputManager;

        public ISolutionManager SolutionManger => _solutionManager;
    }
}
