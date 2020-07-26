using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to an animated GIF file.
    /// By default, this animated GIF file will be sent by the user with optional caption.
    /// Alternatively, you can provide message_text to send it instead of the animation.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InlineQueryResultGif : InlineQueryResultBase
    {
        /// <summary>
        /// A valid URL for the GIF file. File size must not exceed 1MB
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string GifUrl { get; set; }

        /// <summary>
        /// Optional. Width of the GIF.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? GifWidth { get; set; }

        /// <summary>
        /// Optional. Height of the GIF.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? GifHeight { get; set; }

        /// <summary>
        /// Optional. Duration of the GIF.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? GifDuration { get; set; }

        /// <summary>
        /// Caption of the result to be sent, 0-1024 characters.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Caption { get; set; }

        /// <summary>
        /// Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width
        /// text or inline URLs in the media caption.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ParseMode? ParseMode { get; set; }

        /// <summary>
        /// URL of the static thumbnail for the result.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string? ThumbUrl { get; set; }

        /// <summary>
        /// Optional. MIME type of the thumbnail, must be one of “image/jpeg”, “image/gif”, or
        /// “video/mp4”. Defaults to “image/jpeg”
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ThumbMimeType { get; set; }

        /// <summary>
        /// Title of the result
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Title { get; set; }

        /// <summary>
        /// Content of the message to be sent
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputMessageContentBase? InputMessageContent { get; set; }

#pragma warning disable 8618
        private InlineQueryResultGif()
#pragma warning restore 8618
            : base(InlineQueryResultType.Gif)
        { }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="gifUrl">Width of the GIF</param>
        /// <param name="thumbUrl">Url of the thumbnail for the result.</param>
        public InlineQueryResultGif(string id, string gifUrl, string thumbUrl)
            : base(InlineQueryResultType.Gif, id)
        {
            GifUrl = gifUrl;
            ThumbUrl = thumbUrl;
        }
    }
}
