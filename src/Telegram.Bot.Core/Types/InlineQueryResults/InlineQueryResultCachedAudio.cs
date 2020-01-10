using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to an mp3 audio file stored on the Telegram servers. By default, this audio file will be sent by the user. Alternatively, you can use input_message_content to send a message with the specified content instead of the audio.
    /// </summary>
    [DataContract]
    public class InlineQueryResultCachedAudio : InlineQueryResultBase,
        ICaptionInlineQueryResult,
        IInputMessageContentResult
    {
        /// <summary>
        /// A valid file identifier for the audio file
        /// </summary>
        [DataMember(IsRequired = true)]
        public string AudioFileId { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public string Caption { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMessageContentBase InputMessageContent { get; set; }

        private InlineQueryResultCachedAudio()
            : base(InlineQueryResultType.Audio)
        {
        }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="audioFileId">A valid file identifier for the audio file</param>
        public InlineQueryResultCachedAudio(string id, string audioFileId)
            : base(InlineQueryResultType.Audio, id)
        {
            AudioFileId = audioFileId;
        }
    }
}
