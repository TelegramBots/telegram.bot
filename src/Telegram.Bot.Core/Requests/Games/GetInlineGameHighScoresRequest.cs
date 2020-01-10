using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Get data for high score tables. Will return the score of the specified user and several of his neighbors in a game. On success, returns an array of <see cref="GameHighScore"/>.
    /// </summary>
    public class GetInlineGameHighScoresRequest : RequestBase<GameHighScore[]>,
                                                  IInlineMessage
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public int UserId { get; }

        /// <inheritdoc />
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
