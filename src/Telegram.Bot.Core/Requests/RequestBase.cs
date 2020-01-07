﻿using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Requests.Abstractions;

namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Represents a API request
    /// </summary>
    /// <typeparam name="TResponse">Type of result expected in result</typeparam>
    public abstract class RequestBase<TResponse> : IRequest<TResponse>
    {
        public HttpMethod Method { get; }
        public string MethodName { get; }
        public ITelegramBotJsonConverter JsonConverter { get; set; }

        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">Bot API method</param>
        protected RequestBase(string methodName)
            : this(methodName, HttpMethod.Post)
        { }

        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">Bot API method</param>
        /// <param name="method">HTTP method to use</param>
        protected RequestBase(string methodName, HttpMethod method)
        {
            MethodName = methodName;
            Method = method;
        }

        /// <summary>
        /// Generate content of HTTP message
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Content of HTTP request</returns>
        public virtual async ValueTask<HttpContent> ToHttpContentAsync(CancellationToken cancellationToken)
        {
            CheckJsonConverter();

            var stream = new MemoryStream();
            await JsonConverter.SerializeAsync(stream, this, GetType(), cancellationToken);
            stream.Position = 0L;
            return new StreamContent(stream)
            {
                Headers =
                {
                    ContentType = MediaTypeHeaderValue.Parse("application/json")
                }
            };
        }

        private protected void CheckJsonConverter()
        {
            if (JsonConverter == null)
                throw new NullReferenceException("Json converter is null");
        }

        /// <summary>
        /// Allows this object to be used as a response in webhooks
        /// </summary>
        public bool IsWebhookResponse { get; set; }

        /// <summary>
        /// If <see cref="IsWebhookResponse"/> is set to <see langword="true"/> is set to the method
        /// name, otherwise it won't be serialized
        /// </summary>

        internal string WebHookMethodName => IsWebhookResponse ? MethodName : default;
    }
}
