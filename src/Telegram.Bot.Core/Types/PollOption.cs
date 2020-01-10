using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object contains information about one answer option in a poll.
    /// </summary>
    [DataContract]
    public class PollOption
    {
        /// <summary>
        /// Option text, 1-100 characters
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Text { get; set; }

        /// <summary>
        /// Number of users that voted for this option
        /// </summary>
        [DataMember(IsRequired = true)]
        public int VoterCount { get; set; }
    }
}
