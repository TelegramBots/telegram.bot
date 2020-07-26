using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to an article or web page.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InlineQueryResultArticle : InlineQueryResultBase
    {
        /// <summary>
        /// Title of the result
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Content of the message to be sent
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public InputMessageContentBase InputMessageContent { get; set; }

        /// <summary>
        /// Optional. URL of the result.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Url { get; set; }

        /// <summary>
        /// Optional. Pass <c>true</c>, if you don't want the URL to be shown in the message.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? HideUrl { get; set; }

        /// <summary>
        /// Optional. Short description of the result.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Description { get; set; }

        /// <summary>
        /// URL of the static thumbnail for the result.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ThumbUrl { get; set; }

        /// <summary>
        /// Optional. Thumbnail width.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ThumbWidth { get; set; }

        /// <summary>
        /// Optional. Thumbnail height.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ThumbHeight { get; set; }

#pragma warning disable 8618
        private InlineQueryResultArticle()
#pragma warning restore 8618
            : base(InlineQueryResultType.Article)
        {
        }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="title">Title of the result</param>
        /// <param name="inputMessageContent">Content of the message to be sent</param>
        public InlineQueryResultArticle(
            string id,
            string title,
            InputMessageContentBase inputMessageContent)
            : base(InlineQueryResultType.Article, id)
        {
            Title = title;
            InputMessageContent = inputMessageContent;
        }
    }
}
