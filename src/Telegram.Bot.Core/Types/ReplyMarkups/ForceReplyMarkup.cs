using System.Runtime.Serialization;

namespace Telegram.Bot.Types.ReplyMarkups
{
    /// <summary>
    /// Upon receiving a <see cref="Message"/> with this object, Telegram clients will display a reply interface to the user (act as if the user has selected the bot's message and tapped 'Reply'). This can be extremely useful if you want to create user-friendly step-by-step interfaces without having to sacrifice privacy mode.
    /// </summary>
    [DataContract]
    public class ForceReplyMarkup : ReplyMarkupBase
    {
        /// <summary>
        /// Shows reply interface to the user, as if they manually selected the bot's message and tapped 'Reply'
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool ForceReply => true;
    }
}
