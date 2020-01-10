using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents one row of the high scores table for a game.
    /// </summary>
    [DataContract]
    public class GameHighScore
    {
        /// <summary>
        /// Position in high score table for the game.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Position { get; set; }

        /// <summary>
        /// User
        /// </summary>
        [DataMember(IsRequired = true)]
        public User User { get; set; }

        /// <summary>
        /// Score
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Score { get; set; }
    }
}
