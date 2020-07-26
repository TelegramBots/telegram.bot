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
    public class GetGameHighScoresRequest : RequestBase<GameHighScore[]>,
                                            IChatTargetable,
                                            IUserTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public long ChatId { get; }

        /// <summary>
        /// User identifier
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int UserId { get; }

        /// <summary>
        /// Identifier of the sent message
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int MessageId { get; }

        [JsonIgnore]
        ChatId IChatTargetable.ChatId => ChatId;

        /// <summary>
        /// Initializes a new request with userId, chatId and messageId
        /// </summary>
        /// <param name="userId">Target user id</param>
        /// <param name="chatId">Unique identifier for the target chat</param>
        /// <param name="messageId">Identifier of the sent message</param>
        public GetGameHighScoresRequest(int userId, long chatId, int messageId)
            : base("getGameHighScores")
        {
            UserId = userId;
            ChatId = chatId;
            MessageId = messageId;
        }
    }
}
