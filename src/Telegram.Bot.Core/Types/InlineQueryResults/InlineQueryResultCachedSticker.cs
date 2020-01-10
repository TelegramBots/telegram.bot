using System.Runtime.Serialization;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to a sticker stored on the Telegram servers. By default, this sticker will be sent by the user. Alternatively, you can use input_message_content to send a message with the specified content instead of the sticker.
    /// </summary>
    [DataContract]
    public class InlineQueryResultCachedSticker : InlineQueryResultBase,
                                                  IInputMessageContentResult
    {
        /// <summary>
        /// A valid file identifier of the sticker
        /// </summary>
        [DataMember(IsRequired = true)]
        public string StickerFileId { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMessageContentBase InputMessageContent { get; set; }

        private InlineQueryResultCachedSticker()
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
