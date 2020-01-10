// ReSharper disable once CheckNamespace

namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Remove webhook integration if you decide to switch back to getUpdates.
    /// </summary>
    public class DeleteWebhookRequest : ParameterlessRequest<bool>
    {
        /// <summary>
        /// Initializes a new request
        /// </summary>
        public DeleteWebhookRequest()
            : base("deleteWebhook")
        { }
    }
}
