using Adeptik.AlgorithmRuntime;
using System;
using System.IO;

namespace Adeptik.AlgorithmExecutor
{
    /// <summary>
    /// Реализация файлового хранения решения задачи
    /// </summary>
    internal class SolutionManagerFileImpl : ISolutionManager
    {
        private readonly string _solutionsDir;

        /// <summary>
        /// Создние экземпляра <seealso cref="SolutionManagerFileImpl"/>
        /// </summary>
        /// <param name="solutionsDir">Путь к папке. где будет сохранены решения</param>
        public SolutionManagerFileImpl(string solutionsDir)
        {
            if (solutionsDir == null)
                throw new ArgumentNullException(nameof(solutionsDir));
            if (!Directory.Exists(solutionsDir))
                throw new ArgumentException($"Directory not found: {solutionsDir}", nameof(solutionsDir));

            _solutionsDir = solutionsDir;
        }

        /// <summary>
        /// Сохранение решения задачи
        /// </summary>
        /// <param name="solutionStatus">Статус решения</param>
        /// <param name="handleSolutionStream">Обработчик потока решения</param>
        public void Post(SolutionStatus solutionStatus, Action<Stream> handleSolutionStream)
        {
            var solutionFileFormat = $"{solutionStatus}_{DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss")}";

            var number = 0;
            string fileName = "";
            do
            {
                fileName = Path.Combine(_solutionsDir, solutionFileFormat + (number > 0 ? $".{++number}" : ""));
            } while (File.Exists(fileName));
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                handleSolutionStream(fileStream);
            }
        }
    }
}
