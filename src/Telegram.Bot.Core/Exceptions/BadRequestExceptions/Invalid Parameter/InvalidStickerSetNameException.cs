// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the sticker set is invalid
    /// </summary>
    public class InvalidStickerSetNameException : InvalidParameterException
    {
        /// <summary>
        /// Initializes a new object of the <see cref="InvalidStickerSetNameException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        public InvalidStickerSetNameException(string message)
            : base("name", message)
        {
        }
    }
}