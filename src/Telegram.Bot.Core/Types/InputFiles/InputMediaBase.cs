using System.Runtime.Serialization;
using ParseMode = Telegram.Bot.Types.Enums.ParseMode;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents the content of a media message to be sent
    /// </summary>
    [DataContract]
    public abstract class InputMediaBase : IInputMedia
    {
        /// <summary>
        /// Type of the media
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Type { get; protected set; }

        /// <summary>
        /// Media to send
        /// </summary>
        [DataMember(IsRequired = true)]
        public InputMedia Media { get; set; } // ToDo Should be get-only. Media is set in ctors

        /// <summary>
        /// Optional. Caption of the photo to be sent, 0-1024 characters
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Caption { get; set; }

        /// <summary>
        /// Change, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in a caption
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public ParseMode ParseMode { get; set; }
    }
}
