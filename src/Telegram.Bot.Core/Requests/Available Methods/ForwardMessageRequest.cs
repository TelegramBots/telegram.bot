using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Forward messages of any kind
    /// </summary>
    public sealed class ForwardMessageRequest : RequestBase<Message>,
                                                IChatMessage,
                                                INotifiableMessage
    {
        /// <inheritdoc/>
        [DataMember(IsRequired = true)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Unique identifier for the chat where the original message was sent (or channel username in the format @channelusername)
        /// </summary>
        [DataMember(IsRequired = true)]
        public ChatId FromChatId { get; }

        /// <summary>
        /// Message identifier in the chat specified in <see cref="FromChatId"/>
        /// </summary>
        [DataMember(IsRequired = true)]
        public int MessageId { get; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public bool DisableNotification { get; set; }

        /// <summary>
        /// Initializes a new request
        /// </summary>
        public ForwardMessageRequest([DisallowNull] ChatId chatId,
                                     [DisallowNull] ChatId fromChatId,
                                     int messageId)
            : base("forwardMessage")
        {
            ChatId = chatId;
            FromChatId = fromChatId;
            MessageId = messageId;
        }
    }
}
