using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object contains information about a poll.
    /// </summary>
    [DataContract]
    public class Poll
    {
        /// <summary>
        /// Unique poll identifier
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Id { get; set; }

        /// <summary>
        /// Poll question, 1-255 characters
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Question { get; set; }

        /// <summary>
        /// List of poll options
        /// </summary>
        [DataMember(IsRequired = true)]
        public PollOption[] Options { get; set; }

        /// <summary>
        /// True, if the poll is closed
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool IsClosed { get; set; }
    }
}
