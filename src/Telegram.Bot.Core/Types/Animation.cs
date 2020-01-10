using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents an animation file to be displayed in the message containing a <see cref="Game"/>.
    /// </summary>
    [DataContract]
    public class Animation
    {
        /// <summary>
        /// Identifier for this file, which can be used to download or reuse the file
        /// </summary>
        [DataMember(IsRequired = true)]
        public string FileId { get; set; }

        /// <summary>
        /// Unique identifier for this file, which is supposed to be the same over time and for different bots. Can't be used to download or reuse the file.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string FileUniqueId { get; set; }

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
        /// Animation thumbnail as defined by sender.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public PhotoSize Thumb { get; set; }

        /// <summary>
        /// Original animation filename as defined by sender.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FileName { get; set; }

        /// <summary>
        /// MIME type of the file as defined by sender.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string MimeType { get; set; }

        /// <summary>
        /// File size.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int FileSize { get; set; }
    }
}
