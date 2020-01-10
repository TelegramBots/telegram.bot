// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the message is not modified
    /// </summary>
    public class MessageIsNotModifiedException : BadRequestException
    {
        /// <summary>
        /// Initializes a new object of the <see cref="MessageIsNotModifiedException"/> class
        /// </summary>
        /// <param name="message">The error message of this exception.</param>
        public MessageIsNotModifiedException(string message)
            : base(message)
        {
        }
    }
}