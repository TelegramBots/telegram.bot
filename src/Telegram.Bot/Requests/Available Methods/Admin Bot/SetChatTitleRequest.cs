using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Change the title of a chat. Titles can't be changed for private chats. The bot must be an
    /// administrator in the chat for this to work and must have the appropriate admin rights.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SetChatTitleRequest : RequestBase<bool>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// (in the format @channelusername)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// New chat title, 1-255 characters
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Title { get; }

        /// <summary>
        /// Initializes a new request with chatId and new title
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="title">New chat title, 1-255 characters</param>
        public SetChatTitleRequest(ChatId chatId, string title)
            : base("setChatTitle")
        {
            ChatId = chatId;
            Title = title;
        }
    }
}
