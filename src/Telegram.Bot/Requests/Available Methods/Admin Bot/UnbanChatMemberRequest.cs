using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Unban a previously kicked user in a supergroup or channel
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class UnbanChatMemberRequest : RequestBase<bool>, IChatTargetable, IUserTargetable
    {
        /// <summary>
        /// Unique identifier for the target group or username of the target supergroup or channel
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Unique identifier of the target user
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int UserId { get; }

        /// <summary>
        /// Initializes a new request with chatId and userId
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target group or username of the target supergroup or channel
        /// </param>
        /// <param name="userId">Unique identifier of the target user</param>
        public UnbanChatMemberRequest(ChatId chatId, int userId)
            : base("unbanChatMember")
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
