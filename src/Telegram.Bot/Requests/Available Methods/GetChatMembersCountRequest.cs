using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Get the number of members in a chat
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class GetChatMembersCountRequest : RequestBase<int>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target supergroup or channel
        /// (in the format @channelusername)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Initializes a new request with chatId
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        public GetChatMembersCountRequest(ChatId chatId)
            : base("getChatMembersCount")
        {
            ChatId = chatId;
        }
    }
}
