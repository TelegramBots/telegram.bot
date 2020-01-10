using System.Runtime.Serialization;

namespace Telegram.Bot.Types.Payments
{
    /// <summary>
    /// This object contains basic information about an invoice.
    /// </summary>
    /// <seealso href="https://core.telegram.org/bots/api#invoice"/>
    [DataContract]
    public class Invoice
    {
        /// <summary>
        /// Product name
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Description { get; set; }

        /// <summary>
        /// Unique bot deep-linking parameter that can be used to generate this invoice
        /// </summary>
        [DataMember(IsRequired = true)]
        public string StartParameter { get; set; }

        /// <summary>
        /// Three-letter ISO 4217 currency code
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Currency { get; set; }

        /// <summary>
        /// Total price in the smallest units of the currency.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int TotalAmount { get; set; }
    }
}
