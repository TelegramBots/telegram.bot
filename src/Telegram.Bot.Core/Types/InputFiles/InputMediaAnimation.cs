

// ReSharper disable once CheckNamespace

using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// Represents an animation file (GIF or H.264/MPEG-4 AVC video without sound) to be sent
    /// </summary>
    [DataContract]
    public class InputMediaAnimation : InputMediaBase, IInputMediaThumb
    {
        /// <summary>
        /// Optional. Animation width
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Width { get; set; }

        /// <summary>
        /// Optional. Animation height
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Height { get; set; }

        /// <summary>
        /// Optional. Animation duration
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Duration { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMedia Thumb { get; set; }

        /// <summary>
        /// Initializes a new animation media to send with an <see cref="InputMedia"/>
        /// </summary>
        public InputMediaAnimation(InputMedia media)
        {
            Type = "animation";
            Media = media;
        }
    }
}
