// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the chat does not exist
    /// </summary>
    public class ContactRequestException : BadRequestException
    {
        /// <summary>
        /// Initializes a new object of the <see cref="ChatNotFoundException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        public ContactRequestException(string message)
            : base(message)
        {
        }
    }
}