using System.Runtime.Serialization;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents the content of a contact message to be sent as the result of an <see cref="InlineQuery"/>.
    /// </summary>
    [DataContract]
    public class InputContactMessageContent : InputMessageContentBase
    {
        /// <summary>
        /// Contact's phone number
        /// </summary>
        [DataMember(IsRequired = true)]
        public string PhoneNumber { get; private set; }

        /// <summary>
        /// Contact's first name
        /// </summary>
        [DataMember(IsRequired = true)]
        public string FirstName { get; private set; }

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

        private InputContactMessageContent()
        { }

        /// <summary>
        /// Initializes a new input contact message content
        /// </summary>
        /// <param name="phoneNumber">The phone number of the contact</param>
        /// <param name="firstName">The first name of the contact</param>
        public InputContactMessageContent(string phoneNumber, string firstName)
        {
            PhoneNumber = phoneNumber;
            FirstName = firstName;
        }
    }
}
