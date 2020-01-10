using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents one special entity in a text message. For example, hashtags, usernames, URLs, etc.
    /// </summary>
    [DataContract]
    public class MessageEntity
    {
        /// <summary>
        /// Type of the entity
        /// </summary>
        [DataMember(IsRequired = true)]
        public MessageEntityType Type { get; set; }

        /// <summary>
        /// Offset in UTF-16 code units to the start of the entity
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Offset { get; set; }

        /// <summary>
        /// Length of the entity in UTF-16 code units
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Length { get; set; }

        /// <summary>
        /// Optional. For "text_link" only, url that will be opened after user taps on the text
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Url { get; set; }

        /// <summary>
        /// Optional. For "text_mention" only, the mentioned user (for users without usernames)
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public User User { get; set; }
    }
}
