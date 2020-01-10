

// ReSharper disable once CheckNamespace

using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// Represents a general file to be sent
    /// </summary>
    [DataContract]
    public class InputMediaDocument : InputMediaBase, IInputMediaThumb
    {
        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMedia Thumb { get; set; }

        /// <summary>
        /// Initializes a new document media to send with an <see cref="InputMedia"/>
        /// </summary>
        public InputMediaDocument(InputMedia media)
        {
            Type = "document";
            Media = media;
        }
    }
}
