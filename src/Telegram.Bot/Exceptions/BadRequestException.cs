using System;
using System.Net;
using Telegram.Bot.Types;

namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// Represents an error from Bot API with 400 Bad Request HTTP status
    /// </summary>
    public class BadRequestException : ApiRequestException
    {
        /// <inheritdoc />
        public override int ErrorCode => BadRequestErrorCode;

        /// <summary>
        /// Represent error code number
        /// </summary>
        public const int BadRequestErrorCode = (int)HttpStatusCode.BadRequest;

        /// <summary>
        /// Represent error description
        /// </summary>
        public const string BadRequestErrorDescription = "Bad Request: ";

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        public BadRequestException(string message)
            : base(message, BadRequestErrorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="innerException">The inner exception</param>
        public BadRequestException(string message, Exception innerException)
            : base(message, BadRequestErrorCode, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="parameters">Response parameters</param>
        public BadRequestException(string message, ResponseParameters parameters)
            : base(message, BadRequestErrorCode, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadRequestException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="parameters">Response parameters</param>
        /// <param name="innerException">The inner exception</param>
        public BadRequestException(string message, ResponseParameters parameters, Exception innerException)
            : base(message, BadRequestErrorCode, parameters, innerException)
        {
        }
    }
}
