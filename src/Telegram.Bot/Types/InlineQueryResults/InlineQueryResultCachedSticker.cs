using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to a sticker stored on the Telegram servers. By default, this sticker
    /// will be sent by the user. Alternatively, you can use <see cref="InputMessageContent"/>
    /// to send a message with the specified content instead of the sticker.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InlineQueryResultCachedSticker : InlineQueryResultBase
    {
        /// <summary>
        /// A valid file identifier of the sticker
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string StickerFileId { get; set; }

        /// <summary>
        /// Content of the message to be sent
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputMessageContentBase? InputMessageContent { get; set; }

#pragma warning disable 8618
        private InlineQueryResultCachedSticker()
#pragma warning restore 8618
            : base(InlineQueryResultType.Sticker)
        { }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="stickerFileId">A valid file identifier of the sticker</param>
        public InlineQueryResultCachedSticker(string id, string stickerFileId)
            : base(InlineQueryResultType.Sticker, id)
        {
            StickerFileId = stickerFileId;
        }
    }
}
