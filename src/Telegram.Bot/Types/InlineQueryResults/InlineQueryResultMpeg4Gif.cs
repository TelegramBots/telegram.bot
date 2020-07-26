using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to a video animation (H.264/MPEG-4 AVC video without sound).
    /// By default, this animated MPEG-4 file will be sent by the user with optional caption.
    /// Alternatively, you can provide message_text to send it instead of the animation.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InlineQueryResultMpeg4Gif : InlineQueryResultBase
    {
        /// <summary>
        /// A valid URL for the MP4 file. File size must not exceed 1MB.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Mpeg4Url { get; set; }

        /// <summary>
        /// URL of the static thumbnail for the result.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string ThumbUrl { get; }

        /// <summary>
        /// Optional. MIME type of the thumbnail, must be one of “image/jpeg”, “image/gif”, or
        /// “video/mp4”. Defaults to “image/jpeg”
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ThumbMimeType { get; set; }

        /// <summary>
        /// Optional. Video width
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Mpeg4Width { get; set; }

        /// <summary>
        /// Optional. Video height
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Mpeg4Height { get; set; }

        /// <summary>
        /// Optional. Duration of the Video
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Mpeg4Duration { get; set; }

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
        private InlineQueryResultMpeg4Gif()
#pragma warning restore 8618
            : base(InlineQueryResultType.Mpeg4Gif)
        {
        }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="mpeg4Url">A valid URL for the MP4 file. File size must not exceed 1MB.</param>
        /// <param name="thumbUrl">Url of the thumbnail for the result.</param>
        public InlineQueryResultMpeg4Gif(string id, string mpeg4Url, string thumbUrl)
            : base(InlineQueryResultType.Mpeg4Gif, id)
        {
            Mpeg4Url = mpeg4Url;
            ThumbUrl = thumbUrl;
        }
    }
}
