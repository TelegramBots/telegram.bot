using System.Runtime.Serialization;

namespace Telegram.Bot.Types.Payments
{
    /// <summary>
    /// This object contains basic information about a successful payment.
    /// </summary>
    [DataContract]
    public class SuccessfulPayment
    {
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

        /// <summary>
        /// Telegram payment identifier
        /// </summary>
        [DataMember(IsRequired = true)]
        public string TelegramPaymentChargeId { get; set; }

        /// <summary>
        /// Provider payment identifier
        /// </summary>
        [DataMember(IsRequired = true)]
        public string ProviderPaymentChargeId { get; set; }
    }
}
