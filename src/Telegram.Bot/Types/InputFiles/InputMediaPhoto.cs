using System;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types
{
    /// <summary>
    /// Represents a photo to be sent
    /// </summary>
    public class InputMediaPhoto : InputMediaBase, IAlbumInputMedia
    {
        /// <summary>
        /// Initializes a new photo media to send with an <see cref="InputMedia"/>
        /// </summary>
        public InputMediaPhoto(InputMedia media)
            : base(media, "photo")
        {
        }
    }
}
