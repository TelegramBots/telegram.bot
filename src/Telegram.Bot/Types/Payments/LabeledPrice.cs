using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.Payments
{
    /// <summary>
    /// This object represents a portion of the price for goods or services.
    /// </summary>
    /// <see href="https://core.telegram.org/bots/api#labeledprice"/>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class LabeledPrice
    {
        /// <summary>
        /// Portion label
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Label { get; set; }

        /// <summary>
        /// Price of the product in the smallest units of the
        /// <see href="https://core.telegram.org/bots/payments#supported-currencies">currency</see>.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Amount { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="LabeledPrice"/>
        /// </summary>
#pragma warning disable 8618
        private LabeledPrice()
#pragma warning restore 8618
        { }

        /// <summary>
        /// Initializes an instance of <see cref="LabeledPrice"/>
        /// </summary>
        /// <param name="label">Portion label</param>
        /// <param name="amount">Price of the product</param>
        public LabeledPrice(string label, int amount)
        {
            Label = label;
            Amount = amount;
        }
    }
}
