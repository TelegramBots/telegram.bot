using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.Payments
{
    /// <summary>
    /// This object represents a shipping address.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ShippingAddress
    {
        /// <summary>
        /// ISO 3166-1 alpha-2 country code
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string CountryCode { get; set; } = default!;

        /// <summary>
        /// State, if applicable
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? State { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string City { get; set; } = default!;

        /// <summary>
        /// First line for the address
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string StreetLine1 { get; set; } = default!;

        /// <summary>
        /// Second line for the address
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string StreetLine2 { get; set; } = default!;

        /// <summary>
        /// Address post code
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string PostCode { get; set; } = default!;
    }
}
