using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to a video file stored on the Telegram servers. By default, this video file will be sent by the user with an optional caption. Alternatively, you can use input_message_content to send a message with the specified content instead of the video.
    /// </summary>
    [DataContract]
    public class InlineQueryResultCachedVideo : InlineQueryResultBase,
        ICaptionInlineQueryResult,
        ITitleInlineQueryResult,
        IInputMessageContentResult
    {
        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// A valid file identifier for the video file
        /// </summary>
        [DataMember(IsRequired = true)]
        public string VideoFileId { get; set; }

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

        private InlineQueryResultCachedVideo()
            : base(InlineQueryResultType.Video)
        { }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="videoFileId">A valid file identifier for the video file</param>
        /// <param name="title">Title of the result</param>
        public InlineQueryResultCachedVideo(string id, string videoFileId, string title)
            : base(InlineQueryResultType.Video, id)
        {
            VideoFileId = videoFileId;
            Title = title;
        }
    }
}
