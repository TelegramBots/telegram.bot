using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Kick a user from a group, a supergroup or a channel
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class KickChatMemberRequest : RequestBase<bool>, IChatTargetable, IUserTargetable
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
        /// Date when the user will be unbanned, unix time. If user is banned for more than
        /// 366 days or less than 30 seconds from the current time they are considered to be
        /// banned forever.
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? UntilDate { get; set; }

        /// <summary>
        /// Initializes a new request with chatId and userId
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target group or username of the target supergroup or channel
        /// </param>
        /// <param name="userId">Unique identifier of the target user</param>
        public KickChatMemberRequest(ChatId chatId, int userId)
            : base("kickChatMember")
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
