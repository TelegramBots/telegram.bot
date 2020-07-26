using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents the content of a media message to be sent
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract class InputMediaBase : IInputMedia
    {
        /// <summary>
        /// Type of the media
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Type { get; }

        /// <summary>
        /// Media to send
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public InputMedia Media { get; }

        /// <summary>
        /// Optional. Caption of the photo to be sent, 0-1024 characters
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Caption { get; set; }

        /// <summary>
        /// Change, if you want Telegram apps to show bold, italic, fixed-width text or inline
        /// URLs in a caption
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ParseMode? ParseMode { get; set; }

        /// <summary>
        /// Constructs an <see cref="InputMediaBase"/> from a <see cref="InputMedia"/> and a
        /// type of the media
        /// </summary>
        /// <param name="media"><see cref="InputMedia"/> to be sent</param>
        /// <param name="type">Type of of the media to be sent</param>
        /// <remarks>
        /// See <see href="https://core.telegram.org/bots/api#inputmedia"/> for valid types of media
        /// </remarks>
        protected InputMediaBase(InputMedia media, string type)
        {
            Type = type;
            Media = media;
        }
    }
}
