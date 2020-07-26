using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object contains information about one answer option in a poll.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class PollOption
    {
        /// <summary>
        /// Option text, 1-100 characters
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Text { get; set; } = default!;

        /// <summary>
        /// Number of users that voted for this option
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int VoterCount { get; set; }
    }
}
