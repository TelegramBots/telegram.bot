using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents one row of the high scores table for a game.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class GameHighScore
    {
        /// <summary>
        /// Position in high score table for the game.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Position { get; set; }

        /// <summary>
        /// User
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public User User { get; set; } = default!;

        /// <summary>
        /// Score
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Score { get; set; } = default!;
    }
}
