using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a sticker set.
    /// <see href="https://core.telegram.org/bots/api#stickerset"/>
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class StickerSet
    {
        /// <summary>
        /// Sticker set name
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Sticker set title
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; } = default!;

        /// <summary>
        /// True, if the sticker set contains animated stickers
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public bool IsAnimated { get; set; }

        /// <summary>
        /// True, if the sticker set contains masks
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public bool ContainsMasks { get; set; }

        /// <summary>
        /// List of all set stickers
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Sticker[] Stickers { get; set; } = default!;

        /// <summary>
        /// Optional. Sticker set thumbnail in the .WEBP or .TGS format
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PhotoSize? Thumb { get; set; }
    }
}
