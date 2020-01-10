using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to a video animation (H.264/MPEG-4 AVC video without sound) stored on the Telegram servers. By default, this animated MPEG-4 file will be sent by the user with an optional caption. Alternatively, you can use input_message_content to send a message with the specified content instead of the animation.
    /// </summary>
    [DataContract]
    public class InlineQueryResultCachedMpeg4Gif : InlineQueryResultBase,
                                                   ICaptionInlineQueryResult,
                                                   ITitleInlineQueryResult,
                                                   IInputMessageContentResult
    {
        /// <summary>
        /// A valid file identifier for the MP4 file
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Mpeg4FileId { get; set; }

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

        private InlineQueryResultCachedMpeg4Gif()
            : base(InlineQueryResultType.Mpeg4Gif)
        { }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="mpeg4FileId">A valid file identifier for the MP4 file</param>
        public InlineQueryResultCachedMpeg4Gif(string id, string mpeg4FileId)
            : base(InlineQueryResultType.Mpeg4Gif, id)
        {
            Mpeg4FileId = mpeg4FileId;
        }
    }
}
