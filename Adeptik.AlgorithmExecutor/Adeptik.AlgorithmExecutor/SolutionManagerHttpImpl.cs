using Adeptik.AlgorithmExecutor.Exceptions;
using Adeptik.AlgorithmRuntime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Adeptik.AlgorithmExecutor
{
    /// <summary>
    /// Реализация хранения решения задачи в виде HTTP-сервиса
    /// </summary>
    internal class SolutionManagerHttpImpl : ISolutionManager
    {
        private readonly string _baseUrl;
        private readonly string _authorizationHeader;

        private static readonly IList<int> _statusCodesToThrow = new List<int> { 400, 403, 410  };

        /// <summary>
        /// Создание экземпляра класса <seealso cref="SolutionManagerHttpImpl"/>
        /// </summary>
        /// <param name="baseUrl">Адрес сервера хранения решения</param>
        /// <param name="authorizationHeader">Значение заголовка HTTP-запроса Authorization</param>
        public SolutionManagerHttpImpl(string baseUrl, string authorizationHeader)
        {
            _baseUrl = baseUrl;
            _authorizationHeader = authorizationHeader;
        }

        /// <summary>
        /// Сохранение решения задачи
        /// </summary>
        /// <param name="solutionStatus">Статус решения</param>
        /// <param name="handleSolutionStream">Обработчик потока решения</param>
        /// <exception cref="RetryException">ри возникновении данной ошибки возможно повторения выхова через некоторое время</exception>
        /// <exception cref="HttpException">Если при сохранении решения возникнет сетевая ошибка</exception>
        public void Post(SolutionStatus solutionStatus, Action<Stream> handleSolutionStream)
        {
            using (var client = new HttpClient())
            {
                if(!string.IsNullOrEmpty(_authorizationHeader))
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _authorizationHeader);
                client.BaseAddress = new Uri(_baseUrl);
                using (var stream = new MemoryStream())
                {
                    handleSolutionStream(stream);
                    //stream.Get
                    using (var multipartFormDataContent = new MultipartFormDataContent())
                    {
                        multipartFormDataContent.Add(new StringContent(solutionStatus.ToString()), '"' + "SolutionStatus" + '"');
                        multipartFormDataContent.Add(new ByteArrayContent(stream.ToArray()), '"' + "Solution" + '"', '"' + $"solution" + '"');
                        try
                        {
                            var res = client.PutAsync("api/problem/solution", multipartFormDataContent).GetAwaiter().GetResult();
                            if (res.IsSuccessStatusCode)
                                return;
                            if (!res.IsSuccessStatusCode && !_statusCodesToThrow.Contains((int)res.StatusCode))
                                throw new RetryException($"Invalid status code for sending request to {_baseUrl} with authorization{_authorizationHeader}",
                                    new HttpException($"Invalid status code for sending request to {_baseUrl} with authorization{_authorizationHeader}",
                                    (int)res.StatusCode,
                                    res.ReasonPhrase,
                                    res.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                                    res.Headers.ToDictionary(x => x.Key, x => x.Value)));

                            throw new HttpException($"Invalid status code for sending request to {_baseUrl} with authorization{_authorizationHeader}",
                                    (int)res.StatusCode,
                                    res.ReasonPhrase,
                                    res.Content.ReadAsStringAsync().GetAwaiter().GetResult(),
                                    res.Headers.ToDictionary(x => x.Key, x => x.Value));
                        }
                        catch(HttpRequestException e)
                        {
                            throw new RetryException($"Invalid status code for sending request to {_baseUrl} with authorization{_authorizationHeader}", e);
                        }
                        

                    }
                }
                

            }
        }
    }
}
