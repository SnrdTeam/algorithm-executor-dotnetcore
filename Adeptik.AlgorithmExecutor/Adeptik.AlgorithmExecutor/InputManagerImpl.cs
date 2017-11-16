using Adeptik.AlgorithmRuntime;
using System;
using System.IO;

namespace Adeptik.AlgorithmExecutor
{
    /// <summary>
    /// Реализация класса для манипулирования входными данными задачи
    /// </summary>
    public class InputManagerImpl : IInputManager
    {
        private readonly string _problemDir;

        /// <summary>
        /// Создание экземпляра <seealso cref="InputManagerImpl"/>
        /// </summary>
        /// <param name="problemDir">Путь к папке с задачей</param>
        public InputManagerImpl(string problemDir)
        {
            if (problemDir == null)
                throw new ArgumentNullException(nameof(problemDir));
            if (!Directory.Exists(problemDir))
                throw new ArgumentException("Directory not exists", nameof(problemDir));
            _problemDir = problemDir;
        }
        public Stream OpenInput(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            var fullPath = Path.Combine(_problemDir, name);
            if (!File.Exists(fullPath))
                throw new ArgumentException("FIle not found", nameof(fullPath));

            return new FileStream(fullPath, FileMode.Open);
        }
    }
}
