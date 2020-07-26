using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// Contains information about why a request was unsuccessful.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ResponseParameters
    {
        /// <summary>
        /// The group has been migrated to a supergroup with the specified identifier.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? MigrateToChatId { get; set; }

        /// <summary>
        /// In case of exceeding flood control, the number of seconds left to wait before the
        /// request can be repeated.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? RetryAfter { get; set; }
    }
}
