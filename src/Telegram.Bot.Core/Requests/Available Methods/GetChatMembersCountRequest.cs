using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Get the number of members in a chat
    /// </summary>
    public class GetChatMembersCountRequest : RequestBase<int>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target supergroup or channel (in the format @channelusername)
        /// </summary>
        public ChatId ChatId { get; }

        /// <summary>
        /// Initializes a new request with chatId
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        public GetChatMembersCountRequest(ChatId chatId)
            : base("getChatMembersCount")
        {
            ChatId = chatId;
        }
    }
}
