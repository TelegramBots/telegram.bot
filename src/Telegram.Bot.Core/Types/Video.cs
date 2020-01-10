using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a video file.
    /// </summary>
    [DataContract]
    public class Video : FileBase
    {
        /// <summary>
        /// Video width as defined by sender
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Width { get; set; }

        /// <summary>
        /// Video height as defined by sender
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Height { get; set; }

        /// <summary>
        /// Duration of the video in seconds as defined by sender
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Duration { get; set; }

        /// <summary>
        /// Video thumbnail
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Optional. Mime type of a file as defined by sender
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string MimeType { get; set; }
    }
}
