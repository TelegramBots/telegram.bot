using System.Runtime.Serialization;

namespace Telegram.Bot.Types.ReplyMarkups
{
    /// <summary>
    /// Upon receiving a message with this object, Telegram clients will remove the current custom keyboard and display the default letter-keyboard.
    /// By default, custom keyboards are displayed until a new keyboard is sent by a bot.
    /// An exception is made for one-time keyboards that are hidden immediately after the user presses a button
    /// </summary>
    [DataContract]
    public class ReplyKeyboardRemove : ReplyMarkupBase
    {
        /// <summary>
        /// Requests clients to remove the custom keyboard (user will not be able to summon this keyboard; if you want to hide the keyboard from sight but keep it accessible, use one_time_keyboard in ReplyKeyboardMarkup)
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool RemoveKeyboard => true;
    }
}
