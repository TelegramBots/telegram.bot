using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Tell the user that something is happening on the bot's side
    /// </summary>
    public class SendChatActionRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// </summary>
        public ChatId ChatId { get; }

        /// <summary>
        /// Type of action to broadcast
        /// </summary>
        public ChatAction Action { get; }

        /// <summary>
        /// Initializes a new request chatId and action
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="action">Type of action to broadcast</param>
        public SendChatActionRequest(ChatId chatId, ChatAction action)
            : base("sendChatAction")
        {
            ChatId = chatId;
            Action = action;
        }
    }
}
