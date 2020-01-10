// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the sticker set is invalid
    /// </summary>
    public class InvalidStickerDimensionsException : InvalidParameterException
    {
        /// <summary>
        /// Initializes a new object of the <see cref="InvalidStickerDimensionsException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        public InvalidStickerDimensionsException(string message)
            : base("png_sticker", message)
        {
        }
    }
}