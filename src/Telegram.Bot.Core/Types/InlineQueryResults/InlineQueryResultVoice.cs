using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to a voice recording in an .ogg container encoded with OPUS. By default, this voice recording will be sent by the user. Alternatively, you can use <see cref="InputMessageContent"/> to send a message with the specified content instead of the voice message.
    /// </summary>
    /// <remarks>
    /// This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.
    /// </remarks>
    [DataContract]
    public class InlineQueryResultVoice : InlineQueryResultBase,
        ICaptionInlineQueryResult,
        ITitleInlineQueryResult,
        IInputMessageContentResult
    {
        /// <summary>
        /// A valid URL for the voice recording
        /// </summary>
        [DataMember(IsRequired = true)]
        public string VoiceUrl { get; set; }

        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Optional. Recording duration in seconds
        /// </summary>
        [DataMember(IsRequired = true)]
        public int VoiceDuration { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public string Caption { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMessageContentBase InputMessageContent { get; set; }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        public InlineQueryResultVoice()
            : base(InlineQueryResultType.Voice)
        { }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="voiceUrl">A valid URL for the voice recording</param>
        /// <param name="title">Title of the result</param>
        public InlineQueryResultVoice(string id, string voiceUrl, string title)
            : base(InlineQueryResultType.Voice, id)
        {
            VoiceUrl = voiceUrl;
            Title = title;
        }
    }
}
