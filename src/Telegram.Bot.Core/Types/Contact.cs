using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a phone contact.
    /// </summary>
    [DataContract]
    public class Contact
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
        /// Optional. Contact's user identifier in Telegram
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int UserId { get; set; }

        /// <summary>
        /// Optional. Additional data about the contact in the form of a vCard
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Vcard { get; set; }
    }
}
