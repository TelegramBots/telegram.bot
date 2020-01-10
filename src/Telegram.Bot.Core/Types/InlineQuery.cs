using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents an incoming inline query. When the user sends an empty query, your bot could return some default or trending results.
    /// </summary>
    [DataContract]
    public class InlineQuery
    {
        /// <summary>
        /// Unique identifier for this query
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Id { get; set; }

        /// <summary>
        /// Sender
        /// </summary>
        [DataMember(IsRequired = true)]
        public User From { get; set; }

        /// <summary>
        /// Text of the query
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Query { get; set; }

        /// <summary>
        /// Optional. Sender location, only for bots that request user location
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Location Location { get; set; }

        /// <summary>
        /// Offset of the results to be returned, can be controlled by the bot
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Offset { get; set; }
    }
}
