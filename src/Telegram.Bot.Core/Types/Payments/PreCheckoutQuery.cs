using System.Runtime.Serialization;

namespace Telegram.Bot.Types.Payments
{
    /// <summary>
    /// This object contains information about an incoming pre-checkout query
    /// </summary>
    [DataContract]
    public class PreCheckoutQuery
    {
        /// <summary>
        /// Unique query identifier
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Id { get; set; }

        /// <summary>
        /// User who sent the query
        /// </summary>
        [DataMember(IsRequired = true)]
        public User From { get; set; }

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

        /// <summary>
        /// Bot specified invoice payload
        /// </summary>
        [DataMember(IsRequired = true)]
        public string InvoicePayload { get; set; }

        /// <summary>
        /// Optional. Identifier of the shipping option chosen by the user
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string ShippingOptionId { get; set; }

        /// <summary>
        /// Optional. Order info provided by the user
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public OrderInfo OrderInfo { get; set; }
    }
}
