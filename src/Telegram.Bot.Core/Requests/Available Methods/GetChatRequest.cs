using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Get up to date information about the chat (current name of the user for one-on-one conversations, current username of a user, group or channel, etc.)
    /// </summary>
    public class GetChatRequest : RequestBase<Chat>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel (in the format @channelusername)
        /// </summary>
        public ChatId ChatId { get; }

        /// <summary>
        /// Initializes a new request with chatId
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        public GetChatRequest(ChatId chatId)
            : base("getChat")
        {
            ChatId = chatId;
        }
    }
}
