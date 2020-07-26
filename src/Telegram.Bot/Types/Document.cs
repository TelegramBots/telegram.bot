using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a general file (as opposed to <see cref="PhotoSize"/> and
    /// <see cref="Audio"/> files).
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Document : FileBase
    {
        /// <summary>
        /// Document thumbnail as defined by sender
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PhotoSize? Thumb { get; set; }

        /// <summary>
        /// Optional. Original filename as defined by sender
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? FileName { get; set; }

        /// <summary>
        /// Optional. MIME type of the file as defined by sender
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? MimeType { get; set; }
    }
}
