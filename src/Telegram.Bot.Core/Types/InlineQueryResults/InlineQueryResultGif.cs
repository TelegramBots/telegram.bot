using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a link to an animated GIF file.
    /// By default, this animated GIF file will be sent by the user with optional caption.
    /// Alternatively, you can provide message_text to send it instead of the animation.
    /// </summary>
    [DataContract]
    public class InlineQueryResultGif : InlineQueryResultBase,
                                        ICaptionInlineQueryResult,
                                        IThumbnailUrlInlineQueryResult,
                                        ITitleInlineQueryResult,
                                        IInputMessageContentResult
    {
        /// <summary>
        /// A valid URL for the GIF file. File size must not exceed 1MB
        /// </summary>
        [DataMember(IsRequired = true)]
        public string GifUrl { get; set; }

        /// <summary>
        /// Optional. Width of the GIF.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int GifWidth { get; set; }

        /// <summary>
        /// Optional. Height of the GIF.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int GifHeight { get; set; }

        /// <summary>
        /// Optional. Duration of the GIF.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int GifDuration { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public string Caption { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }

        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string ThumbUrl { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public string Title { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMessageContentBase InputMessageContent { get; set; }

        private InlineQueryResultGif()
            : base(InlineQueryResultType.Gif)
        { }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="gifUrl">Width of the GIF</param>
        /// <param name="thumbUrl">Url of the thumbnail for the result.</param>
        public InlineQueryResultGif(string id, string gifUrl, string thumbUrl)
            : base(InlineQueryResultType.Gif, id)
        {
            GifUrl = gifUrl;
            ThumbUrl = thumbUrl;
        }
    }
}
