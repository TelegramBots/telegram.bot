using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Delete a chat photo. Photos can't be changed for private chats. The bot must be an
    /// administrator in the chat for this to work and must have the appropriate admin rights.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class DeleteChatPhotoRequest : RequestBase<bool>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// (in the format @channelusername)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Initializes a new request with chat_id and photo
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        public DeleteChatPhotoRequest(ChatId chatId)
            : base("deleteChatPhoto")
        {
            ChatId = chatId;
        }
    }
}
