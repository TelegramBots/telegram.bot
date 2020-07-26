using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types
{
    /// <summary>
    /// Represents an audio file to be treated as music to be sent
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InputMediaAudio : InputMediaBase, IInputMediaThumb
    {
        /// <summary>
        /// Optional. Title of the audio
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Title { get; set; }

        /// <summary>
        /// Optional. Performer of the audio
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Performer { get; set; }

        /// <summary>
        /// Optional. Duration of the audio in seconds
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Duration { get; set; }

        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputMedia? Thumb { get; set; }

        /// <summary>
        /// Initializes a new audio media to send with an <see cref="InputMedia"/>
        /// </summary>
        public InputMediaAudio(InputMedia media)
            : base(media, "audio")
        {
        }
    }
}
