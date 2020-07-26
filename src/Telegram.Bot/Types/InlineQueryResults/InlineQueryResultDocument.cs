using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to a file. By default, this file will be sent by the user with an
    /// optional caption. Alternatively, you can use <see cref="InputMessageContent"/> to send
    /// a message with the specified content instead of the file. Currently, only .PDF and .ZIP
    /// files can be sent using this method.
    /// </summary>
    /// <remarks>
    /// This will only work in Telegram versions released after 9 April, 2016. Older clients will
    /// ignore them.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InlineQueryResultDocument : InlineQueryResultBase
    {
        /// <summary>
        /// A valid URL for the file
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string DocumentUrl { get; set; }

        /// <summary>
        /// Title of the result
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Mime type of the content of the file, either “application/pdf” or “application/zip”
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string MimeType { get; set; }

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
        /// Optional. Short description of the result
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Description { get; set; }

        /// <summary>
        /// URL of the static thumbnail for the result.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ThumbUrl { get; set; }

        /// <summary>
        /// Thumbnail width.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ThumbWidth { get; set; }

        /// <summary>
        /// Thumbnail height.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ThumbHeight { get; set; }

        /// <summary>
        /// Content of the message to be sent
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputMessageContentBase? InputMessageContent { get; set; }

#pragma warning disable 8618
        private InlineQueryResultDocument()
#pragma warning restore 8618
            : base(InlineQueryResultType.Document)
        {
        }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="documentUrl">A valid URL for the file</param>
        /// <param name="title">Title of the result</param>
        /// <param name="mimeType">
        /// Mime type of the content of the file, either “application/pdf” or “application/zip”
        /// </param>
        public InlineQueryResultDocument(string id, string documentUrl, string title, string mimeType)
            : base(InlineQueryResultType.Document, id)
        {
            DocumentUrl = documentUrl;
            Title = title;
            MimeType = mimeType;
        }
    }
}
