using System.Runtime.Serialization;

namespace Telegram.Bot.Types.Payments
{
    /// <summary>
    /// This object represents a shipping address.
    /// </summary>
    [DataContract]
    public class ShippingAddress
    {
        /// <summary>
        /// ISO 3166-1 alpha-2 country code
        /// </summary>
        [DataMember(IsRequired = true)]
        public string CountryCode { get; set; }

        /// <summary>
        /// State, if applicable
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string State { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [DataMember(IsRequired = true)]
        public string City { get; set; }

        /// <summary>
        /// First line for the address
        /// </summary>
        [DataMember(IsRequired = true)]
        public string StreetLine1 { get; set; }

        /// <summary>
        /// Second line for the address
        /// </summary>
        [DataMember(IsRequired = true)]
        public string StreetLine2 { get; set; }

        /// <summary>
        /// Address post code
        /// </summary>
        [DataMember(IsRequired = true)]
        public string PostCode { get; set; }
    }
}
