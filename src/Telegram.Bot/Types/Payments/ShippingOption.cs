using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.Payments
{
    /// <summary>
    /// This object represents one shipping option.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ShippingOption
    {
        /// <summary>
        /// Shipping option identifier
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; } = default!;

        /// <summary>
        /// Option title
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; } = default!;

        /// <summary>
        /// List of price portions
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public LabeledPrice[] Prices { get; set; } = default!;
    }
}
