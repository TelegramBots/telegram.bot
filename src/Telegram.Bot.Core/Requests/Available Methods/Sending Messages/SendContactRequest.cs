using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send phone contacts
    /// </summary>
    public class SendContactRequest : RequestBase<Message>,
                                      INotifiableMessage,
                                      IReplyMessage,
                                      IReplyMarkupMessage<IReplyMarkup>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// </summary>
        public ChatId ChatId { get; }

        /// <summary>
        /// Contact's phone number
        /// </summary>
        public string PhoneNumber { get; }

        /// <summary>
        /// Contact's first name
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Contact's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Optional. Additional data about the contact in the form of a vCard, 0-2048 bytes
        /// </summary>
        public string Vcard { get; set; }

        /// <inheritdoc />
        public bool DisableNotification { get; set; }

        /// <inheritdoc />
        public int ReplyToMessageId { get; set; }

        /// <inheritdoc />
        public IReplyMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId, contacts's phone number and first name
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="phoneNumber">Contact's phone number</param>
        /// <param name="firstName">Contact's first name</param>
        public SendContactRequest(ChatId chatId, string phoneNumber, string firstName)
            : base("sendContact")
        {
            ChatId = chatId;
            PhoneNumber = phoneNumber;
            FirstName = firstName;
        }
    }
}
