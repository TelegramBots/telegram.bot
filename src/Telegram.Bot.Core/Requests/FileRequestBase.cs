using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Helpers;
using Telegram.Bot.Types.InputFiles;

namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Represents an API request with a file
    /// </summary>
    /// <typeparam name="TResponse">Type of result expected in result</typeparam>
    public abstract class FileRequestBase<TResponse> : RequestBase<TResponse>
    {
        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">Bot API method</param>
        protected FileRequestBase(string methodName)
            : base(methodName)
        { }

        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">Bot API method</param>
        /// <param name="method">HTTP method to use</param>
        protected FileRequestBase(string methodName, HttpMethod method)
            : base(methodName, method)
        { }

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="jsonConverter">JSON converter that is used for (de)serialization of the JSON</param>
        /// <param name="fileParameterName"></param>
        /// <param name="inputFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async ValueTask<MultipartFormDataContent> ToMultipartFormDataContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            [DisallowNull] string fileParameterName,
            [DisallowNull] InputFileStream inputFile,
            CancellationToken cancellationToken)
        {
            var multipartContent =
                await GenerateMultipartFormDataContent(jsonConverter, cancellationToken, fileParameterName);

            multipartContent.AddStreamContent(inputFile.Content, fileParameterName, inputFile.FileName);

            return multipartContent;
        }

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="jsonConverter">JSON converter that is used for (de)serialization of the JSON</param>
        /// <param name="cancellationToken">Cancellation token used for cancellation of the multipart form-data generation.</param>
        /// <param name="exceptPropertyNames">Property names to remove from multipart form-data content.</param>
        /// <returns></returns>
        protected async ValueTask<MultipartFormDataContent> GenerateMultipartFormDataContent(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken,
            [DisallowNull] params string[] exceptPropertyNames)
        {
            var multipartContent = new MultipartFormDataContent(Guid.NewGuid().ToString() + DateTime.UtcNow.Ticks);

            var nodes = await jsonConverter.ToNodesAsync(this, GetType(), exceptPropertyNames, cancellationToken);

            foreach (var (name, content) in nodes)
                multipartContent.Add(content, name);

            return multipartContent;
        }
    }
}
