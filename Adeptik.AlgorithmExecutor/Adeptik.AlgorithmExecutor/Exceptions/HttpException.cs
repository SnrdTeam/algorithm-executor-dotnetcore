using System;
using System.Collections.Generic;

namespace Adeptik.AlgorithmExecutor.Exceptions
{
    /// <summary>
    /// Ошибка при выполнениии http-запроса
    /// </summary>
    public class HttpException : Exception
    {
        private readonly int _code;
        private readonly string _reason;
        private readonly string _body;
        private readonly Dictionary<string, IEnumerable<string>> _headers;

        public HttpException(string message, int code, string reason, string body, Dictionary<string, IEnumerable<string>> headers): base(message)
        {
            _code = code;
            _reason = reason;
            _body = body;
            _headers = headers;
        }

        /// <summary>
        /// Код ответа
        /// </summary>
        public int Code => _code;

        /// <summary>
        /// reason phrase
        /// </summary>
        public string Reason => _reason;

        /// <summary>
        /// Тело ответа
        /// </summary>
        public string Body => _body;

        /// <summary>
        /// Загловки ответа
        /// </summary>
        public Dictionary<string, IEnumerable<string>> Headers => _headers;

    }
}
