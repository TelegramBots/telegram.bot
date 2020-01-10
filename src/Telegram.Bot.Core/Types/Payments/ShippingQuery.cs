using System.Runtime.Serialization;

namespace Telegram.Bot.Types.Payments
{
    /// <summary>
    /// This object contains information about an incoming shipping query.
    /// </summary>
    [DataContract]
    public class ShippingQuery
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
        /// Bot specified invoice payload
        /// </summary>
        [DataMember(IsRequired = true)]
        public string InvoicePayload { get; set; }

        /// <summary>
        /// User specified shipping address
        /// </summary>
        [DataMember(IsRequired = true)]
        public ShippingAddress ShippingAddress { get; set; }
    }
}
