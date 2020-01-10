using System;
using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// Contains information about the current status of a webhook.
    /// </summary>
    [DataContract]
    public class WebhookInfo
    {
        /// <summary>
        /// Webhook URL, may be empty if webhook is not set up
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Url { get; set; }

        /// <summary>
        /// True, if a custom certificate was provided for webhook certificate checks
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool HasCustomCertificate { get; set; }

        /// <summary>
        /// Number of updates awaiting delivery
        /// </summary>
        [DataMember(IsRequired = true)]
        public int PendingUpdateCount { get; set; }

        /// <summary>
        /// Unix time for the most recent error that happened when trying to deliver an update via webhook
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public DateTime LastErrorDate { get; set; }

        /// <summary>
        /// Error message in human-readable format for the most recent error that happened when trying to deliver an update via webhook
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string LastErrorMessage { get; set; }

        /// <summary>
        /// Maximum allowed number of simultaneous HTTPS connections to the webhook for update delivery
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int MaxConnections { get; set; }

        /// <summary>
        /// A list of update types the bot is subscribed to. Defaults to all update types
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public UpdateType[] AllowedUpdates { get; set; }
    }
}
