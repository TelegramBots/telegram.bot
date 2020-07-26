using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// If you sent an invoice requesting a shipping address and the parameter
    /// <see cref="SendInvoiceRequest.IsFlexible"/> was specified, the Bot API will send an
    /// <see cref="Update"/> with a <see cref="Update.ShippingQuery"/> field to the bot.
    /// Use this method to reply to shipping queries. On success, <c>true</c> is returned.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class AnswerShippingQueryRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the query to be answered
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string ShippingQueryId { get; }

        /// <summary>
        /// Specify <c>true</c> if delivery to the specified address is possible and <c>false</c>
        /// if there are any problems (for example, if delivery to the specified address is
        /// not possible)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public bool Ok { get; }

        /// <summary>
        /// Required if ok is <c>true</c>. A JSON-serialized array of available shipping options.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<ShippingOption>? ShippingOptions { get; set; }

        /// <summary>
        /// Required if ok is <c>false</c>. Error message in human readable form that explains why
        /// it is impossible to complete the order (e.g. "Sorry, delivery to your desired address
        /// is unavailable'). Telegram will display this message to the user.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new failing answerShippingQuery request with error message
        /// </summary>
        /// <param name="shippingQueryId">Unique identifier for the query to be answered</param>
        /// <param name="errorMessage">Error message in human readable form</param>
        public AnswerShippingQueryRequest(string shippingQueryId, string errorMessage)
            : base("answerShippingQuery")
        {
            ShippingQueryId = shippingQueryId;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Initializes a new successful answerShippingQuery request with shipping options
        /// </summary>
        /// <param name="shippingQueryId">Unique identifier for the query to be answered</param>
        /// <param name="shippingOptions">
        /// A JSON-serialized array of available shipping options
        /// </param>
        public AnswerShippingQueryRequest(
            string shippingQueryId,
            IEnumerable<ShippingOption> shippingOptions)
            : base("answerShippingQuery")
        {
            Ok = true;
            ShippingQueryId = shippingQueryId;
            ShippingOptions = shippingOptions;
        }
    }
}
