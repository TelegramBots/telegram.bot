using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to a video animation (H.264/MPEG-4 AVC video without sound).
    /// By default, this animated MPEG-4 file will be sent by the user with optional caption.
    /// Alternatively, you can provide message_text to send it instead of the animation.
    /// </summary>
    [DataContract]
    public class InlineQueryResultMpeg4Gif : InlineQueryResultBase,
        ICaptionInlineQueryResult,
        IThumbnailUrlInlineQueryResult,
        ITitleInlineQueryResult,
        IInputMessageContentResult
    {
        /// <summary>
        /// A valid URL for the MP4 file. File size must not exceed 1MB.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Mpeg4Url { get; set; }

        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// Optional. Video width
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Mpeg4Width { get; set; }

        /// <summary>
        /// Optional. Video height
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Mpeg4Height { get; set; }

        /// <summary>
        /// Optional. Duration of the Video
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Mpeg4Duration { get; set; }

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

        private InlineQueryResultMpeg4Gif()
            : base(InlineQueryResultType.Mpeg4Gif)
        {
        }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="mpeg4Url">A valid URL for the MP4 file. File size must not exceed 1MB.</param>
        /// <param name="thumbUrl">Url of the thumbnail for the result.</param>
        public InlineQueryResultMpeg4Gif(string id, string mpeg4Url, string thumbUrl)
            : base(InlineQueryResultType.Mpeg4Gif, id)
        {
            Mpeg4Url = mpeg4Url;
            ThumbUrl = thumbUrl;
        }
    }
}
