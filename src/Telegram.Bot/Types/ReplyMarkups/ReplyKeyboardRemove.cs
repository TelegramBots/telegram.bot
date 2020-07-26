using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.ReplyMarkups
{
    /// <summary>
    /// Upon receiving a message with this object, Telegram clients will remove the current custom
    /// keyboard and display the default letter-keyboard. By default, custom keyboards are
    /// displayed until a new keyboard is sent by a bot. An exception is made for one-time
    /// keyboards that are hidden immediately after the user presses a button
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ReplyKeyboardRemove : ReplyMarkupBase
    {
        /// <summary>
        /// Requests clients to remove the custom keyboard (user will not be able to summon this
        /// keyboard; if you want to hide the keyboard from sight but keep it accessible, use
        /// <see cref="ReplyKeyboardMarkup.OneTimeKeyboard"/> in <see cref="ReplyKeyboardMarkup"/>)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public bool RemoveKeyboard => true;
    }
}
