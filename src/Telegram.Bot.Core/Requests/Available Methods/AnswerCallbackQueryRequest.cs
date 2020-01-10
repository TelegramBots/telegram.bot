

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send answers to callback queries sent from inline keyboards
    /// </summary>
    public class AnswerCallbackQueryRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the query to be answered
        /// </summary>
        public string CallbackQueryId { get; }

        /// <summary>
        /// Text of the notification. If not specified, nothing will be shown to the user, 0-200 characters.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// If true, an alert will be shown by the client instead of a notification at the top of the chat screen. Defaults to false.
        /// </summary>
        public bool ShowAlert { get; set; }

        /// <summary>
        /// URL that will be opened by the user's client
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The maximum amount of time in seconds that the result of the callback query may be cached client-side. Telegram apps will support caching starting in version 3.14. Defaults to 0.
        /// </summary>
        public int CacheTime { get; set; }

        /// <summary>
        /// Initializes a new request with callbackQueryId
        /// </summary>
        /// <param name="callbackQueryId">Unique identifier for the query to be answered</param>
        public AnswerCallbackQueryRequest(string callbackQueryId)
            : base("answerCallbackQuery")
        {
            CallbackQueryId = callbackQueryId;
        }
    }
}
