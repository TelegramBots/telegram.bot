using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.ReplyMarkups
{
    /// <summary>
    /// Defines how clients display a reply interface to the user
    /// </summary>
    /// <seealso cref="Telegram.Bot.Types.ReplyMarkups.IReplyMarkup" />
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract class ReplyMarkupBase : IReplyMarkup
    {
        /// <summary>
        /// Optional. Use this parameter if you want to show the keyboard to specific users only.
        /// Targets:
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// users that are @mentioned in the text of the Description object;
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// if the bot's message is a reply (has <see cref="Message.ReplyToMessage"/>), sender of
        /// the original message.
        /// </description>
        /// </item>
        /// </list>
        /// Example: A user requests to change the bot's language, bot replies to the request
        /// with a keyboard to select the new language. Other users in the group don't see the
        /// keyboard.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Selective { get; set; }
    }
}
