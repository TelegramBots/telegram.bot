using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to an animated GIF file stored on the Telegram servers. By default, this animated GIF file will be sent by the user with an optional caption. Alternatively, you can use <see cref="InlineQueryResultCachedGif.InputMessageContent"/> to send a message with specified content instead of the animation.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InlineQueryResultCachedGif : InlineQueryResult,
                                              ICaptionInlineQueryResult
    {
        /// <summary>
        /// Type of the result, must be gif
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public override InlineQueryResultType Type => InlineQueryResultType.Gif;

        /// <summary>
        /// A valid file identifier for the GIF file
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string GifFileId { get; }

        /// <summary>
        /// Optional. Title for the result
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Title { get; set; }

        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Caption { get; set; }

        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ParseMode? ParseMode { get; set; }

        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MessageEntity[]? CaptionEntities { get; set; }

        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputMessageContent? InputMessageContent { get; set; }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="gifFileId">A valid file identifier for the GIF file</param>
        public InlineQueryResultCachedGif(string id, string gifFileId)
            : base(id)
        {
            GifFileId = gifFileId;
        }
    }
}