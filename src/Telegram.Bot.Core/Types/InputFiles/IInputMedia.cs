using ParseMode = Telegram.Bot.Types.Enums.ParseMode;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types
{
    /// <summary>
    /// A marker interface for input media content
    /// </summary>
    public interface IInputMedia
    {
        /// <summary>
        /// Type of the media
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Media to send
        /// </summary>
        InputMedia Media { get; }

        /// <summary>
        /// Optional. Caption of the photo to be sent, 0-1024 characters
        /// </summary>
        string Caption { get; }

        /// <summary>
        /// Change, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in a caption
        /// </summary>
        ParseMode ParseMode { get; }
    }
}
