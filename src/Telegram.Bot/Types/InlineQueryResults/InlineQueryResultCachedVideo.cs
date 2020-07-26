using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to a video file stored on the Telegram servers. By default, this video
    /// file will be sent by the user with an optional caption. Alternatively, you can use
    /// <see cref="InputMessageContent"/> to send a message with the specified content instead
    /// of the video.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InlineQueryResultCachedVideo : InlineQueryResultBase
    {
        /// <summary>
        /// Title of the result
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// A valid file identifier for the video file
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string VideoFileId { get; set; }

        /// <summary>
        /// Optional. Short description of the result
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Description { get; set; }

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
        /// Content of the message to be sent
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputMessageContentBase? InputMessageContent { get; set; }

#pragma warning disable 8618
        private InlineQueryResultCachedVideo()
#pragma warning restore 8618
            : base(InlineQueryResultType.Video)
        { }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="videoFileId">A valid file identifier for the video file</param>
        /// <param name="title">Title of the result</param>
        public InlineQueryResultCachedVideo(string id, string videoFileId, string title)
            : base(InlineQueryResultType.Video, id)
        {
            VideoFileId = videoFileId;
            Title = title;
        }
    }
}
