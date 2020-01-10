using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a sticker.
    /// <see href="https://core.telegram.org/bots/api#sticker"/>
    /// </summary>
    [DataContract]
    public class Sticker : FileBase
    {
        /// <summary>
        /// Sticker width
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Width { get; set; }

        /// <summary>
        /// Sticker height
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Height { get; set; }

        /// <summary>
        /// True, if the sticker is animated
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool IsAnimated { get; set; }

        /// <summary>
        /// Sticker thumbnail in .webp or .jpg format
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Emoji associated with the sticker
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Emoji { get; set; }

        /// <summary>
        /// Optional. Name of the sticker set to which the sticker belongs
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string SetName { get; set; }

        /// <summary>
        /// Optional. For mask stickers, the position where the mask should be placed
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MaskPosition MaskPosition { get; set; }
    }
}
