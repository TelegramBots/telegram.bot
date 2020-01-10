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
        /// Initializes a new photo media to send
        /// </summary>
        [Obsolete("Use the other overload of this constructor with required parameter instead.")]
        public InputMediaPhoto()
        {
            Type = "photo";
        }

        /// <summary>
        /// Initializes a new photo media to send with an <see cref="InputMedia"/>
        /// </summary>
        public InputMediaPhoto(InputMedia media)
        {
            Type = "photo";
            Media = media;
        }
    }
}
