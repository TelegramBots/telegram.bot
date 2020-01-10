using System.Runtime.Serialization;

namespace Telegram.Bot.Types.Enums
{
    /// <summary>
    /// Type of action the Bot is performing
    /// </summary>
    public enum ChatAction
    {
        /// <summary>
        /// Typing
        /// </summary>
        Typing,

        /// <summary>
        /// Uploading a <see cref="PhotoSize"/>
        /// </summary>
        [EnumMember(Value = "upload_photo")]
        UploadPhoto,

        /// <summary>
        /// Recording a <see cref="Video"/>
        /// </summary>
        [EnumMember(Value = "record_video")]
        RecordVideo,

        /// <summary>
        /// Uploading a <see cref="Video"/>
        /// </summary>
        [EnumMember(Value = "upload_video")]
        UploadVideo,

        /// <summary>
        /// Recording an <see cref="Audio"/>
        /// </summary>
        [EnumMember(Value = "record_audio")]
        RecordAudio,

        /// <summary>
        /// Uploading an <see cref="Audio"/>
        /// </summary>
        [EnumMember(Value = "upload_audio")]
        UploadAudio,

        /// <summary>
        /// Uploading <see cref="Document"/>
        /// </summary>
        [EnumMember(Value = "upload_document")]
        UploadDocument,

        /// <summary>
        /// Finding a <see cref="Location"/>
        /// </summary>
        [EnumMember(Value = "find_location")]
        FindLocation,

        /// <summary>
        /// Recording a <see cref="VideoNote"/>
        /// </summary>
        [EnumMember(Value = "record_video_note")]
        RecordVideoNote,

        /// <summary>
        /// Uploading a <see cref="VideoNote"/>
        /// </summary>
        [EnumMember(Value = "upload_video_note")]
        UploadVideoNote,
    }
}
