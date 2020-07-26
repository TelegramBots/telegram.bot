using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Get a list of administrators in a chat. On success, returns an Array of
    /// <see cref="ChatMember"/> objects that contains information about all chat administrators
    /// except other bots. If the chat is a group or a supergroup and no administrators were
    /// appointed, only the creator will be returned.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class GetChatAdministratorsRequest : RequestBase<ChatMember[]>, IChatTargetable
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
        public GetChatAdministratorsRequest(ChatId chatId)
            : base("getChatAdministrators")
        {
            ChatId = chatId;
        }
    }
}
