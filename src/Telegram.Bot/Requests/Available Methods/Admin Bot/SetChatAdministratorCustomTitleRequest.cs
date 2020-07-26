using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Use this method to set a custom title for an administrator in a supergroup promoted by the
    /// bot. Returns <c>true</c> on success.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SetChatAdministratorCustomTitleRequest : RequestBase<bool>,
                                                          IChatTargetable,
                                                          IUserTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// (in the format @channelusername)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Unique identifier of the target user
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int UserId { get; set; }

        /// <summary>
        /// New custom title for the administrator; 0-16 characters, emoji are not allowed
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string CustomTitle { get; }

        /// <summary>
        /// Initializes a new request with chatId and new title
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="userId">Unique identifier of the target user</param>
        /// <param name="customTitle">
        /// New custom title for the administrator; 0-16 characters, emoji are not allowed
        /// </param>
        public SetChatAdministratorCustomTitleRequest(
            ChatId chatId,
            int userId,
            string customTitle)
            : base("setChatAdministratorCustomTitle")
        {
            ChatId = chatId;
            UserId = userId;
            CustomTitle = customTitle;
        }
    }
}
