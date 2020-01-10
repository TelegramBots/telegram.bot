using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Pin a message in a group, a supergroup, or a channel.
    /// The bot must be an administrator in the chat for this to work and must have
    /// the ‘can_pin_messages’ admin right in the supergroup or ‘can_edit_messages’ admin right in the channel.
    /// </summary>
    [DataContract]
    public sealed class PinChatMessageRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel (in the format @channel_username)
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// Identifier of a message to pin
        /// </summary>
        [DataMember(IsRequired = true)]
        public int MessageId { get; }

        /// <summary>
        /// Pass True, if it is not necessary to send a notification to all chat members about the new pinned message.
        /// Notifications are always disabled in channels.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool DisableNotification { get; set; }

        /// <summary>
        /// Initializes a new request of type <see cref="PinChatMessageRequest"/>
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel (in the format @channel_username)</param>
        /// <param name="messageId">Identifier of a message to pin</param>
        public PinChatMessageRequest(
            [DisallowNull] ChatId chatId,
            int messageId)
            : base("pinChatMessage")
        {
            ChatId = chatId;
            MessageId = messageId;
        }
    }
}
