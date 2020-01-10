

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Once the user has confirmed their payment and shipping details, the Bot API sends the final confirmation in the form of an Update with the field pre_checkout_query. Use this method to respond to such pre-checkout queries. On success, True is returned. Note: The Bot API must receive an answer within 10 seconds after the pre-checkout query was sent.
    /// </summary>
    public class AnswerPreCheckoutQueryRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the query to be answered
        /// </summary>
        public string PreCheckoutQueryId { get; }

        /// <summary>
        /// Specify True if everything is alright (goods are available, etc.) and the bot is ready to proceed with the order. Use False if there are any problems.
        /// </summary>
        public bool Ok { get; }

        /// <summary>
        /// Required if ok is False. Error message in human readable form that explains the reason for failure to proceed with the checkout (e.g. "Sorry, somebody just bought the last of our amazing black T-shirts while you were busy filling out your payment details. Please choose a different color or garment!"). Telegram will display this message to the user.
        /// </summary>
        public string ErrorMessage { get; set; }

        private AnswerPreCheckoutQueryRequest()
            : base("answerPreCheckoutQuery")
        { }

        /// <summary>
        /// Initializes a new successful answerPreCheckoutQuery request
        /// </summary>
        /// <param name="preCheckoutQuery">Unique identifier for the query to be answered</param>
        public AnswerPreCheckoutQueryRequest(string preCheckoutQuery)
            : this(preCheckoutQuery, null)
        {
            Ok = true;
        }

        /// <summary>
        /// Initializes a new failing answerPreCheckoutQuery request with error message
        /// </summary>
        /// <param name="preCheckoutQuery">Unique identifier for the query to be answered</param>
        /// <param name="errorMessage">Error message in human readable form</param>
        public AnswerPreCheckoutQueryRequest(string preCheckoutQuery, string errorMessage)
            : this()
        {
            PreCheckoutQueryId = preCheckoutQuery;
            ErrorMessage = errorMessage;
        }
    }
}
