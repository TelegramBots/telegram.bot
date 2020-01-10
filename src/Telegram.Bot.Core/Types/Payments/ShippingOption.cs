using System.Runtime.Serialization;

namespace Telegram.Bot.Types.Payments
{
    /// <summary>
    /// This object represents one shipping option.
    /// </summary>
    [DataContract]
    public class ShippingOption
    {
        /// <summary>
        /// Shipping option identifier
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Id { get; set; }

        /// <summary>
        /// Option title
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// List of price portions
        /// </summary>
        [DataMember(IsRequired = true)]
        public LabeledPrice[] Prices { get; set; }
    }
}
