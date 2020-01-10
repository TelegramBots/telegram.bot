using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Set the score of the specified user in a game. On success returns the edited <see cref="Message"/>. Returns an error, if the new score is not greater than the user's current score in the chat and force is False.
    /// </summary>
    public class SetGameScoreRequest : RequestBase<Message>
    {
        /// <summary>
        /// Unique identifier for the target chat
        /// </summary>
        public long ChatId { get; }

        /// <summary>
        /// User identifier
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Identifier of the sent message
        /// </summary>
        public int MessageId { get; }

        /// <summary>
        /// New score, must be non-negative
        /// </summary>
        public int Score { get; }

        /// <summary>
        /// Pass True, if the high score is allowed to decrease. This can be useful when fixing mistakes or banning cheaters.
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// Pass True, if the game message should not be automatically edited to include the current scoreboard
        /// </summary>
        public bool DisableEditMessage { get; set; }

        /// <summary>
        /// Initializes a new request
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="score">New score, must be non-negative</param>
        /// <param name="chatId">Unique identifier for the target chat</param>
        /// <param name="messageId">Identifier of the sent message</param>
        public SetGameScoreRequest(int userId, int score, long chatId, int messageId)
            : base("setGameScore")
        {
            UserId = userId;
            Score = score;
            ChatId = chatId;
            MessageId = messageId;
        }
    }
}
