using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents one size of a photo or a file / sticker thumbnail.
    /// </summary>
    /// <remarks>A missing thumbnail for a file (or sticker) is presented as an empty object.</remarks>
    [DataContract]
    public class PhotoSize : FileBase
    {
        /// <summary>
        /// Photo width
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Width { get; set; }

        /// <summary>
        /// Photo height
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Height { get; set; }
    }
}
