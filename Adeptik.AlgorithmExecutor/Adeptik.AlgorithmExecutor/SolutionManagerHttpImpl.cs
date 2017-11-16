using Adeptik.AlgorithmExecutor.Exceptions;
using Adeptik.AlgorithmRuntime;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace Adeptik.AlgorithmExecutor
{
    /// <summary>
    /// Реализация хранения решения задачи в виде HTTP-сервиса
    /// </summary>
    public class SolutionManagerHttpImpl : ISolutionManager
    {
        private readonly string _url;
        private readonly string _authorizationHeader;

        /// <summary>
        /// Создание экземпляра класса <seealso cref="SolutionManagerHttpImpl"/>
        /// </summary>
        /// <param name="url">Адрес сервера хранения решения</param>
        /// <param name="authorizationHeader">Значение заголовка HTTP-запроса Authorization</param>
        public SolutionManagerHttpImpl(string url, string authorizationHeader)
        {
            _url = url;
            _authorizationHeader = authorizationHeader;
        }

        /// <summary>
        /// Сохранение решения задачи
        /// </summary>
        /// <param name="solutionStatus">Статус решения</param>
        /// <param name="outputStreamHandler">Объект для осуществления сохранения решения</param>
        /// <exception cref="HttpException">Если при сохранении решения сервер вернет код отличный от 200</exception>
        public void Post(SolutionStatus solutionStatus, IOutputStreamHandler outputStreamHandler)
        {
            using (var client = new HttpClient())
            {
                if(!string.IsNullOrEmpty(_authorizationHeader))
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _authorizationHeader);
                //client.BaseAddress = new Uri(_baseUrl);
                var stream = new MemoryStream();
                outputStreamHandler.Handle(stream);
                using (var multipartFormDataContent = new MultipartFormDataContent())
                {
                    multipartFormDataContent.Add(new StringContent(solutionStatus.ToString()), '"' + "SolutionStatus" + '"');
                    multipartFormDataContent.Add( new StreamContent(stream), '"' + "Solution" + '"', '"' + $"solution" + '"');
                    var res = client.PostAsync(_url, multipartFormDataContent).GetAwaiter().GetResult();

                    if (!res.IsSuccessStatusCode)
                    {
                        throw new HttpException($"Invalid status code for sending request to {_url} with authorization{_authorizationHeader}",
                            (int)res.StatusCode, res.ReasonPhrase, res.Content.ReadAsStringAsync().GetAwaiter().GetResult(), res.Headers.ToDictionary(x => x.Key, x => x.Value));
                    }

                }

            }
        }
    }
}
