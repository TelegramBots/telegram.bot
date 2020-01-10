using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a video message (available in Telegram apps as of v.4.0).
    /// </summary>
    [DataContract]
    public class VideoNote : FileBase
    {
        /// <summary>
        /// Video width and height as defined by sender
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Length { get; set; }

        /// <summary>
        /// Duration of the video in seconds as defined by sender
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Duration { get; set; }

        /// <summary>
        /// Optional. Video thumbnail
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public PhotoSize Thumb { get; set; }
    }
}
