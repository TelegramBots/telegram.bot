

// ReSharper disable once CheckNamespace

using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// Represents an audio file to be treated as music to be sent
    /// </summary>
    [DataContract]
    public class InputMediaAudio : InputMediaBase, IInputMediaThumb
    {
        /// <summary>
        /// Optional. Title of the audio
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Title { get; set; }

        /// <summary>
        /// Optional. Performer of the audio
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Performer { get; set; }

        /// <summary>
        /// Optional. Duration of the audio in seconds
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Duration { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMedia Thumb { get; set; }

        /// <summary>
        /// Initializes a new audio media to send with an <see cref="InputMedia"/>
        /// </summary>
        public InputMediaAudio(InputMedia media)
        {
            Type = "audio";
            Media = media;
        }
    }
}
