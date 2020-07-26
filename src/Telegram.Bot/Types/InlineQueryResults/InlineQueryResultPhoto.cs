using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to a photo. By default, this photo will be sent by the user with
    /// optional caption. Alternatively, you can provide <see cref="InputMessageContent"/> to
    /// send it instead of photo.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InlineQueryResultPhoto : InlineQueryResultBase
    {
        /// <summary>
        /// URL of the thumbnail for the photo
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// A valid URL of the photo. Photo size must not exceed 5MB.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Optional. Width of the photo
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? PhotoWidth { get; set; }

        /// <summary>
        /// Optional. Height of the photo
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? PhotoHeight { get; set; }

        /// <summary>
        /// Optional. Short description of the result
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Description { get; set; }

        /// <summary>
        /// Optional. Caption of the result to be sent, 0-1024 characters.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Caption { get; set; }

        /// <summary>
        /// Optional. Send Markdown or HTML, if you want Telegram apps to show bold, italic,
        /// fixed-width text or inline URLs in the media caption.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ParseMode? ParseMode { get; set; }

        /// <summary>
        /// Optional. Title of the result
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Title { get; set; }

        /// <summary>
        /// Optional. Content of the message to be sent instead of the photo
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputMessageContentBase? InputMessageContent { get; set; }

#pragma warning disable 8618
        private InlineQueryResultPhoto()
#pragma warning restore 8618
            : base(InlineQueryResultType.Photo)
        {
        }

        /// <summary>
        /// Initializes a new inline query representing a link to a photo
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="photoUrl">A valid URL of the photo. Photo size must not exceed 5MB.</param>
        /// <param name="thumbUrl">Optional. Url of the thumbnail for the result.</param>
        public InlineQueryResultPhoto(string id, string photoUrl, string thumbUrl)
            : base(InlineQueryResultType.Photo, id)
        {
            PhotoUrl = photoUrl;
            ThumbUrl = thumbUrl;
        }
    }
}
