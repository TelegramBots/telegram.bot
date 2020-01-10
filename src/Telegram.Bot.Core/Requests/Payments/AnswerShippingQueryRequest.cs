using System.Collections.Generic;
using Telegram.Bot.Types.Payments;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// If you sent an invoice requesting a shipping address and the parameter is_flexible was specified, the Bot API will send an Update with a shipping_query field to the bot. Use this method to reply to shipping queries. On success, True is returned.
    /// </summary>
    public class AnswerShippingQueryRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the query to be answered
        /// </summary>
        public string ShippingQueryId { get; }

        /// <summary>
        /// Specify True if delivery to the specified address is possible and False if there are any problems (for example, if delivery to the specified address is not possible)
        /// </summary>
        public bool Ok { get; }

        /// <summary>
        /// Required if ok is True. A JSON-serialized array of available shipping options.
        /// </summary>
        public IEnumerable<ShippingOption> ShippingOptions { get; set; }

        /// <summary>
        /// Required if ok is False. Error message in human readable form that explains why it is impossible to complete the order (e.g. "Sorry, delivery to your desired address is unavailable'). Telegram will display this message to the user.
        /// </summary>
        public string ErrorMessage { get; set; }

        private AnswerShippingQueryRequest()
            : base("answerShippingQuery")
        { }

        /// <summary>
        /// Initializes a new failing answerShippingQuery request with error message
        /// </summary>
        /// <param name="shippingQueryId">Unique identifier for the query to be answered</param>
        /// <param name="errorMessage">Error message in human readable form</param>
        public AnswerShippingQueryRequest(string shippingQueryId, string errorMessage)
            : this()
        {
            ShippingQueryId = shippingQueryId;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Initializes a new successful answerShippingQuery request with shipping options
        /// </summary>
        /// <param name="shippingQueryId">Unique identifier for the query to be answered</param>
        /// <param name="shippingOptions">A JSON-serialized array of available shipping options</param>
        public AnswerShippingQueryRequest(string shippingQueryId, IEnumerable<ShippingOption> shippingOptions)
            : this(shippingQueryId, null as string)
        {
            Ok = true;
            ShippingOptions = shippingOptions;
        }
    }
}
