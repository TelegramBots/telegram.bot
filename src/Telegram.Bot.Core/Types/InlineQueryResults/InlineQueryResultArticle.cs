using System.Runtime.Serialization;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to an article or web page.
    /// </summary>
    [DataContract]
    public class InlineQueryResultArticle : InlineQueryResultBase,
        IThumbnailInlineQueryResult,
        ITitleInlineQueryResult,
        IInputMessageContentResult
    {
        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public InputMessageContentBase InputMessageContent { get; set; }

        /// <summary>
        /// Optional. URL of the result.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Url { get; set; }

        /// <summary>
        /// Optional. Pass <c>true</c>, if you don't want the URL to be shown in the message.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool HideUrl { get; set; }

        /// <summary>
        /// Optional. Short description of the result.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public string ThumbUrl { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public int ThumbWidth { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public int ThumbHeight { get; set; }

        private InlineQueryResultArticle()
            : base(InlineQueryResultType.Article)
        {
        }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="title">Title of the result</param>
        /// <param name="inputMessageContent">Content of the message to be sent</param>
        public InlineQueryResultArticle(string id, string title, InputMessageContentBase inputMessageContent)
            : base(InlineQueryResultType.Article, id)
        {
            Title = title;
            InputMessageContent = inputMessageContent;
        }
    }
}
