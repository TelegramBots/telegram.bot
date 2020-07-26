using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Get basic info about a file and prepare it for downloading. For the moment, bots can
    /// download files of up to 20MB in size. On success, a <see cref="File"/> object is returned.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class GetFileRequest : RequestBase<File>
    {
        /// <summary>
        /// File identifier to get info about
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string FileId { get; }

        /// <summary>
        /// Initializes a new request with <see cref="FileId"/>
        /// </summary>
        /// <param name="fileId">File identifier to get info about</param>
        public GetFileRequest(string fileId)
            : base("getFile")
        {
            FileId = fileId;
        }
    }
}
