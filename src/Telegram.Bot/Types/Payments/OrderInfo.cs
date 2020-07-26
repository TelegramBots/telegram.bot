using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.Payments
{
    /// <summary>
    /// This object represents information about an order.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class OrderInfo
    {
        /// <summary>
        /// Optional. User name
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Name { get; set; }

        /// <summary>
        /// Optional. User's phone number
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Optional. User email
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Email { get; set; }

        /// <summary>
        /// Optional. User shipping address
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ShippingAddress? ShippingAddress { get; set; }
    }
}
