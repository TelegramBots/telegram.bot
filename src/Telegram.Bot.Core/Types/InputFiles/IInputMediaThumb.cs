// ReSharper disable once CheckNamespace

namespace Telegram.Bot.Types
{
    /// <summary>
    /// Indicates that an <see cref="InputMediaBase"/> has a thumbnail.
    /// </summary>
    public interface IInputMediaThumb
    {
        /// <summary>
        /// Thumbnail of the file sent. The thumbnail should be in JPEG format and less than 200 kB in size. A
        /// thumbnail's width and height should not exceed 90. Thumbnails can't be reused and can be only uploaded as
        /// a new file.
        /// </summary>
        InputMedia Thumb { get; }
    }
}
