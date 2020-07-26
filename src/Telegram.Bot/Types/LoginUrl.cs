using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a parameter of the inline keyboard button used to automatically
    /// authorize a user
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class LoginUrl
    {
        /// <summary>
        /// An HTTP URL to be opened with user authorization data added to the query string when the
        /// button is pressed
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Url { get; set; } = default!;

        /// <summary>
        /// Optional. New text of the button in forwarded messages
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ForwardText { get; set; }

        /// <summary>
        /// Optional. Username of a bot, which will be used for user authorization
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? BotUsername { get; set; }

        /// <summary>
        /// Optional. Pass True to request the permission for your bot to send messages to the user
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? RequestWriteAccess { get; set; }
    }
}
