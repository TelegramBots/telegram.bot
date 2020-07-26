using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Set the score of the specified user in a game. On success returns <c>true</c>.
    /// Returns an error, if the new score is not greater than the user's current score in the
    /// chat and force is <c>false</c>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SetInlineGameScoreRequest : RequestBase<bool>, IUserTargetable
    {
        /// <summary>
        /// User identifier
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int UserId { get; }

        /// <summary>
        /// New score, must be non-negative
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Score { get; }

        /// <summary>
        /// Identifier of the inline message
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string InlineMessageId { get; }

        /// <summary>
        /// Pass <c>true</c>, if the high score is allowed to decrease. This can be useful when
        /// fixing mistakes or banning cheaters.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Force { get; set; }

        /// <summary>
        /// Pass <c>true</c>, if the game message should not be automatically edited to include
        /// the current scoreboard
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? DisableEditMessage { get; set; }

        /// <summary>
        /// Initializes a new request with userId, inlineMessageId and new score
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="inlineMessageId">Identifier of the inline message</param>
        /// <param name="score">New score, must be non-negative</param>
        public SetInlineGameScoreRequest(int userId, int score, string inlineMessageId)
            : base("setGameScore")
        {
            UserId = userId;
            Score = score;
            InlineMessageId = inlineMessageId;
        }
    }
}
