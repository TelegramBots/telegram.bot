using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Delete a message, including service messages, with the following limitations:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// A message can only be deleted if it was sent less than 48 hours ago.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Bots can delete outgoing messages in groups and supergroups.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Bots granted can_post_messages permissions can delete outgoing messages in channels.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// If the bot is an administrator of a group, it can delete any message there.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// If the bot has can_delete_messages permission in a supergroup or a channel, it can delete
    /// any message there.
    /// </description>
    /// </item>
    /// </list>
    /// <para/>
    /// Returns <c>true</c> on success.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class DeleteMessageRequest : RequestBase<bool>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// (in the format @channelusername)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Identifier of the sent message
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int MessageId { get; }

        /// <summary>
        /// Initializes a new request with chatId and messageId
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="messageId">Identifier of the sent message</param>
        public DeleteMessageRequest(ChatId chatId, int messageId)
            : base("deleteMessage")
        {
            ChatId = chatId;
            MessageId = messageId;
        }
    }
}
