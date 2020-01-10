using System;
using Telegram.Bot.Types;

namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// Represents an error from Bot API with 403 Forbidden HTTP status
    /// </summary>
    public abstract class ForbiddenException : ApiRequestException
    {
        /// <inheritdoc />
        public override int ErrorCode => ForbiddenErrorCode;

        /// <summary>
        /// Represent error code number
        /// </summary>
        public const int ForbiddenErrorCode = 403;

        /// <summary>
        /// Represent error description
        /// </summary>
        public const string ForbiddenErrorDescription = "Forbidden: ";

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        protected ForbiddenException(string message)
            : base(message, ForbiddenErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="innerException">The inner exception</param>
        protected ForbiddenException(string message, Exception innerException)
            : base(message, ForbiddenErrorCode, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="parameters">Response parameters</param>
        protected ForbiddenException(string message, ResponseParameters parameters)
            : base(message, ForbiddenErrorCode, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForbiddenException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="parameters">Response parameters</param>
        /// <param name="innerException">The inner exception</param>
        protected ForbiddenException(string message, ResponseParameters parameters, Exception innerException)
            : base(message, ForbiddenErrorCode, parameters, innerException)
        {
        }
    }
}
