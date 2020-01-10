using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to a voice message stored on the Telegram servers. By default, this voice message will be sent by the user. Alternatively, you can use input_message_content to send a message with the specified content instead of the voice message.
    /// </summary>
    [DataContract]
    public class InlineQueryResultCachedVoice : InlineQueryResultBase,
        ICaptionInlineQueryResult,
        ITitleInlineQueryResult,
        IInputMessageContentResult
    {
        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// A valid file identifier for the voice message
        /// </summary>
        [DataMember(IsRequired = true)]
        public string VoiceFileId { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public string Caption { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMessageContentBase InputMessageContent { get; set; }

        private InlineQueryResultCachedVoice()
            : base(InlineQueryResultType.Voice)
        { }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="fileId">A valid file identifier for the voice message</param>
        /// <param name="title">Title of the result</param>
        public InlineQueryResultCachedVoice(string id, string fileId, string title)
            : base(InlineQueryResultType.Voice, id)
        {
            VoiceFileId = fileId;
            Title = title;
        }
    }
}
