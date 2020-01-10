using System.Runtime.Serialization;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a contact with a phone number. By default, this contact will be sent by the user. Alternatively, you can use input_message_content to send a message with the specified content instead of the contact.
    /// </summary>
    /// <remarks>
    /// This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.
    /// </remarks>
    [DataContract]
    public class InlineQueryResultContact : InlineQueryResultBase,
        IThumbnailInlineQueryResult,
        IInputMessageContentResult
    {
        /// <summary>
        /// Contact's phone number
        /// </summary>
        [DataMember(IsRequired = true)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Contact's first name
        /// </summary>
        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }

        /// <summary>
        /// Optional. Contact's last name
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string LastName { get; set; }

        /// <summary>
        /// Optional. Additional data about the contact in the form of a vCard, 0-2048 bytes
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Vcard { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public string ThumbUrl { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public int ThumbWidth { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public int ThumbHeight { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMessageContentBase InputMessageContent { get; set; }

        private InlineQueryResultContact()
            : base(InlineQueryResultType.Contact)
        { }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="phoneNumber">Contact's phone number</param>
        /// <param name="firstName">Contact's first name</param>
        public InlineQueryResultContact(string id, string phoneNumber, string firstName)
            : base(InlineQueryResultType.Contact, id)
        {
            PhoneNumber = phoneNumber;
            FirstName = firstName;
        }
    }
}
