using System;
using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types
{
    /// <summary>
    /// Represents a video to be sent
    /// </summary>
    [DataContract]
    public class InputMediaVideo : InputMediaBase, IInputMediaThumb, IAlbumInputMedia
    {
        /// <summary>
        /// Optional. Video width
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Width { get; set; }

        /// <summary>
        /// Optional. Video height
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Height { get; set; }

        /// <summary>
        /// Optional. Video duration
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Duration { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMedia Thumb { get; set; }

        /// <summary>
        /// Optional. Pass True, if the uploaded video is suitable for streaming
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool SupportsStreaming { get; set; }

        /// <summary>
        /// Initializes a new video media to send
        /// </summary>
        [Obsolete("Use the other overload of this constructor with required parameter instead.")]
        public InputMediaVideo()
        {
            Type = "video";
        }

        /// <summary>
        /// Initializes a new video media to send with an <see cref="InputMedia"/>
        /// </summary>
        public InputMediaVideo(InputMedia media)
        {
            Type = "video";
            Media = media;
        }
    }
}
