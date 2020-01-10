using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to an animated GIF file stored on the Telegram servers. By default, this animated GIF file will be sent by the user with an optional caption. Alternatively, you can use input_message_content to send a message with specified content instead of the animation.
    /// </summary>
    [DataContract]
    public class InlineQueryResultCachedGif : InlineQueryResultBase,
                                              ICaptionInlineQueryResult,
                                              ITitleInlineQueryResult,
                                              IInputMessageContentResult
    {
        /// <summary>
        /// A valid file identifier for the GIF file
        /// </summary>
        [DataMember(IsRequired = true)]
        public string GifFileId { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public string Caption { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public string Title { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMessageContentBase InputMessageContent { get; set; }

        private InlineQueryResultCachedGif()
            : base(InlineQueryResultType.Gif)
        { }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="gifFileId">A valid file identifier for the GIF file</param>
        public InlineQueryResultCachedGif(string id, string gifFileId)
            : base(InlineQueryResultType.Gif, id)
        {
            GifFileId = gifFileId;
        }
    }
}
