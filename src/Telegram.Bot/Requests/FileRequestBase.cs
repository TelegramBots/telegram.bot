using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Helpers;
using Telegram.Bot.Types.InputFiles;

namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Represents an API request with a file
    /// </summary>
    /// <typeparam name="TResponse">Type of result expected in result</typeparam>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
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
        /// Automatically generates <see cref="MultipartFormDataContent"/> instance and adds
        /// property passed in <paramref name="fileParameterName"/> parameter as a stream.
        /// </summary>
        /// <param name="fileParameterName">
        /// Name of the property that should be added as a stream
        /// </param>
        /// <param name="inputFile"></param>
        /// <returns>Generated instance of <see cref="MultipartFormDataContent"/></returns>
        protected MultipartFormDataContent ToMultipartFormDataContent(
            string fileParameterName,
            InputFileStream inputFile)
        {
            var multipartContent = GenerateMultipartFormDataContent(fileParameterName);

            multipartContent.AddStreamContent(
                inputFile.Content!,
                fileParameterName,
                inputFile.FileName!
            );

            return multipartContent;
        }

        /// <summary>
        /// Automatically generates <see cref="MultipartFormDataContent"/> instance with string
        /// data skipping properties passed in <paramref name="exceptPropertyNames"/> parameter.
        /// All names of media properties should be passed in
        /// <paramref name="exceptPropertyNames"/>.
        /// </summary>
        /// <param name="exceptPropertyNames">Properties to skip</param>
        /// <returns>
        /// Generated instance of <see cref="MultipartFormDataContent"/> without properties passed
        /// in <paramref name="exceptPropertyNames"/>
        /// </returns>
        protected MultipartFormDataContent GenerateMultipartFormDataContent(
            params string[] exceptPropertyNames)
        {
            var multipartContent = new MultipartFormDataContent(
                Guid.NewGuid().ToString() + DateTime.UtcNow.Ticks
            );

            var stringContents = JObject.FromObject(this)
                .Properties()
                .Where(prop => exceptPropertyNames?.Contains(prop.Name) == false)
                .Select(prop => new
                {
                    prop.Name,
                    Content = new StringContent(prop.Value.ToString())
                });

            foreach (var strContent in stringContents)
            {
                multipartContent.Add(strContent.Content, strContent.Name);
            }

            return multipartContent;
        }
    }
}
