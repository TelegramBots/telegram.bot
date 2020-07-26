using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a file ready to be downloaded. The file can be downloaded via
    /// <see cref="GetFileRequest"/>. It is guaranteed that the link will be valid for at least 1
    /// hour. When the link expires, a new one can be requested by calling
    /// <see cref="GetFileRequest"/>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract class FileBase
    {
        /// <summary>
        /// Identifier for this file, which can be used to download or reuse the file
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string FileId { get; set; } = default!;

        /// <summary>
        /// Unique identifier for this file, which is supposed to be the same over time and for
        /// different bots. Can't be used to download or reuse the file.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string FileUniqueId { get; set; } = default!;

        /// <summary>
        /// Optional. File size, if known
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? FileSize { get; set; }
    }
}
