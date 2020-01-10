using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to a photo.
    /// By default, this photo will be sent by the user with optional caption.
    /// Alternatively, you can provide message_text to send it instead of photo.
    /// </summary>
    [DataContract]
    public class InlineQueryResultPhoto : InlineQueryResultBase,
        ICaptionInlineQueryResult,
        IThumbnailUrlInlineQueryResult,
        ITitleInlineQueryResult,
        IInputMessageContentResult
    {
        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// A valid URL of the photo. Photo size must not exceed 5MB.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Optional. Width of the photo
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int PhotoWidth { get; set; }

        /// <summary>
        /// Optional. Height of the photo
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int PhotoHeight { get; set; }

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
        public string Title { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMessageContentBase InputMessageContent { get; set; }

        private InlineQueryResultPhoto()
            : base(InlineQueryResultType.Photo)
        {
        }

        /// <summary>
        /// Initializes a new inline query representing a link to a photo
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="photoUrl">A valid URL of the photo. Photo size must not exceed 5MB.</param>
        /// <param name="thumbUrl">Optional. Url of the thumbnail for the result.</param>
        public InlineQueryResultPhoto(string id, string photoUrl, string thumbUrl)
            : base(InlineQueryResultType.Photo, id)
        {
            PhotoUrl = photoUrl;
            ThumbUrl = thumbUrl;
        }
    }
}
