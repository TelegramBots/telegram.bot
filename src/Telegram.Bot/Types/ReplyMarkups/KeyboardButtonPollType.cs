using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.ReplyMarkups
{
    /// <summary>
    /// This object represents type of a poll, which is allowed to be created and sent when the
    /// corresponding button is pressed.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class KeyboardButtonPollType
    {
        /// <summary>
        /// Optional. If quiz is passed, the user will be allowed to create only polls in the quiz
        /// mode. If regular is passed, only regular polls will be allowed. Otherwise, the user
        /// will be allowed to create a poll of any type
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Type { get; set; }
    }
}
