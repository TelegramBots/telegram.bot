using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send text messages
    /// </summary>
    public sealed class SendMessageRequest : RequestBase<Message>,
                                             IChatMessage,
                                             INotifiableMessage,
                                             IReplyMessage,
                                             IFormattableMessage,
                                             IReplyMarkupMessage<IReplyMarkup>
    {
        /// <inheritdoc/>
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// Text of the message to be sent
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public string Text { get; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }

        /// <summary>
        /// Disables link previews for links in this message
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool DisableWebPagePreview { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public bool DisableNotification { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public int ReplyToMessageId { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public IReplyMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="text">Text of the message to be sent</param>
        public SendMessageRequest([DisallowNull] ChatId chatId, [DisallowNull] string text)
            : base("sendMessage")
        {
            ChatId = chatId;
            Text = text;
        }
    }
}
