using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents link to a page containing an embedded video player or a video file.
    /// </summary>
    [DataContract]
    public class InlineQueryResultVideo : InlineQueryResultBase,
        ICaptionInlineQueryResult,
        IThumbnailUrlInlineQueryResult,
        ITitleInlineQueryResult,
        IInputMessageContentResult
    {
        /// <summary>
        /// A valid URL for the embedded video player or video file
        /// </summary>
        [DataMember(IsRequired = true)]
        public string VideoUrl { get; set; }

        /// <summary>
        /// Mime type of the content of video url, i.e. "text/html" or "video/mp4"
        /// </summary>
        [DataMember(IsRequired = true)]
        public string MimeType { get; set; }

        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string ThumbUrl { get; set; }

        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Optional. Video width
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int VideoWidth { get; set; }

        /// <summary>
        /// Optional. Video height
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int VideoHeight { get; set; }

        /// <summary>
        /// Optional. Video duration in seconds
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int VideoDuration { get; set; }

        /// <summary>
        /// Optional. Short description of the result
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public string Caption { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMessageContentBase InputMessageContent { get; set; }

        private InlineQueryResultVideo()
            : base(InlineQueryResultType.Video)
        {
        }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="videoUrl">A valid URL for the embedded video player or video file</param>
        /// <param name="mimeType">Mime type of the content of video url, i.e. "text/html" or "video/mp4"</param>
        /// <param name="thumbUrl">Url of the thumbnail for the result</param>
        /// <param name="title">Title of the result</param>
        public InlineQueryResultVideo(
            string id,
            string videoUrl,
            string mimeType,
            string thumbUrl,
            string title
        )
            : base(InlineQueryResultType.Video, id)
        {
            VideoUrl = videoUrl;
            MimeType = mimeType;
            ThumbUrl = thumbUrl;
            Title = title;
        }
    }
}
