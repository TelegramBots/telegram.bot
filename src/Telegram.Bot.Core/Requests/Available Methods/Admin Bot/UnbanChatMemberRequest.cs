using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Unban a previously kicked user in a supergroup or channel
    /// </summary>
    public class UnbanChatMemberRequest : ChatIdRequestBase<bool>
    {
        /// <summary>
        /// Unique identifier of the target user
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Initializes a new request with <see cref="ChatId"/> and <see cref="UserId"/>
        /// </summary>
        /// <param name="chatId">Unique identifier for the target group or username of the target supergroup or channel</param>
        /// <param name="userId">Unique identifier of the target user</param>
        public UnbanChatMemberRequest([NotNull] ChatId chatId, int userId)
            : base("unbanChatMember")
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
