using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a sticker set.
    /// <see href="https://core.telegram.org/bots/api#stickerset"/>
    /// </summary>
    [DataContract]
    public class StickerSet
    {
        /// <summary>
        /// Sticker set name
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Sticker set title
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// True, if the sticker set contains animated stickers
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool IsAnimated { get; set; }

        /// <summary>
        /// True, if the sticker set contains masks
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool ContainsMasks { get; set; }

        /// <summary>
        /// List of all set stickers
        /// </summary>
        [DataMember(IsRequired = true)]
        public Sticker[] Stickers { get; set; }
    }
}
