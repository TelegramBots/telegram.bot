using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a voice note.
    /// </summary>
    [DataContract]
    public class Voice : FileBase
    {
        /// <summary>
        /// Duration of the audio in seconds as defined by sender
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Duration { get; set; }

        /// <summary>
        /// Optional. MIME type of the file as defined by sender
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string MimeType { get; set; }
    }
}
