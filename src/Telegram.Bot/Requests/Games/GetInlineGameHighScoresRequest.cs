using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Get data for high score tables. Will return the score of the specified user and several of
    /// his neighbors in a game. On success, returns an array of <see cref="GameHighScore"/>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class GetInlineGameHighScoresRequest : RequestBase<GameHighScore[]>, IUserTargetable
    {
        /// <summary>
        /// User identifier
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int UserId { get; }

        /// <summary>
        /// Identifier of the inline message
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string InlineMessageId { get; }

        /// <summary>
        /// Initializes a new request with userId and inlineMessageId
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="inlineMessageId">Unique identifier of the inline message</param>
        public GetInlineGameHighScoresRequest(int userId, string inlineMessageId)
            : base("getGameHighScores")
        {
            UserId = userId;
            InlineMessageId = inlineMessageId;
        }
    }
}
