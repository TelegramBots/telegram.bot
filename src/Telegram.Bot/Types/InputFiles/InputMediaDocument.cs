using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types
{
    /// <summary>
    /// Represents a general file to be sent
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InputMediaDocument : InputMediaBase, IInputMediaThumb
    {
        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputMedia? Thumb { get; set; }

        /// <summary>
        /// Initializes a new document media to send with an <see cref="InputMedia"/>
        /// </summary>
        public InputMediaDocument(InputMedia media)
            : base(media, "document")
        {
        }
    }
}
