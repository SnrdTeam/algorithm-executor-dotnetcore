using System;

namespace Adeptik.AlgorithmRuntime
{
    /// <summary>
    /// Ошибка выполнения операции, исправление которой возможно, если повторить операцию позднее
    /// </summary>
    public class RetryException: Exception
    {
        public RetryException(string message) : base(message) { }

        public RetryException(string message, Exception inner) : base(message, inner) { }

        public RetryException() : base() { }
    }
}
