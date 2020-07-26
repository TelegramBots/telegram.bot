using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents one special entity in a text message. For example, hashtags,
    /// usernames, URLs, etc.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class MessageEntity
    {
        /// <summary>
        /// Type of the entity
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public MessageEntityType Type { get; set; }

        /// <summary>
        /// Offset in UTF-16 code units to the start of the entity
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Offset { get; set; }

        /// <summary>
        /// Length of the entity in UTF-16 code units
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Length { get; set; }

        /// <summary>
        /// Optional. For <see cref="MessageEntityType.TextLink"/> only, url that will be opened
        /// after user taps on the text
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Url { get; set; }

        /// <summary>
        /// Optional. For <see cref="MessageEntityType.TextMention"/> only, the mentioned user
        /// (for users without usernames)
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public User? User { get; set; }

        /// <summary>
        /// Optional. For <see cref="MessageEntityType.Pre"/> only, the programming language of the
        /// entity text
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Language { get; set; }
    }
}
