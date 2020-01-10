using System;
using Telegram.Bot.Types;

namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// Represents an API error
    /// </summary>
    public class ApiRequestException : Exception
    {
        /// <summary>
        /// Gets the error code.
        /// </summary>
        public virtual int ErrorCode { get; }

        /// <summary>
        /// Contains information about why a request was unsuccessful.
        /// </summary>
        public ResponseParameters Parameters { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ApiRequestException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorCode">The error code.</param>
        public ApiRequestException(string message, int errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ApiRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="innerException">The inner exception.</param>
        public ApiRequestException(string message, int errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="errorCode">The error code</param>
        /// <param name="parameters">Response parameters</param>
        public ApiRequestException(string message, int errorCode, ResponseParameters parameters)
            : base(message)
        {
            ErrorCode = errorCode;
            Parameters = parameters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestException"/> class
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="errorCode">The error code</param>
        /// <param name="parameters">Response parameters</param>
        /// <param name="innerException">The inner exception</param>
        public ApiRequestException(string message, int errorCode, ResponseParameters parameters,
                                   Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            Parameters = parameters;
        }

        /// <summary>
        /// Returns a new instance of the <see cref="ApiRequestException"/> class.
        /// </summary>
        /// <typeparam name="T">Expected type of operation result</typeparam>
        /// <param name="apiResponse">The API response.</param>
        /// <returns><see cref="ApiRequestException"/></returns>
        public static ApiRequestException FromApiResponse<T>(ApiResponse<T> apiResponse)
        {
            return ApiExceptionParser.Parse(apiResponse);
            /*
            string message;
            switch (apiResponse.ErrorCode)
            {
                //TODO: Add test cases for all those
                case 400:
                    message = apiResponse.Description.Remove(0, "Bad Request: ".Length);
                    switch (message.Trim())
                    {
                        case "have no rights to send a message":
                            return new BotRestrictedException(apiResponse.Description)
                            {
                                Parameters = apiResponse.Parameters
                            };
                        case "not enough rights to restrict/unrestrict chat member":
                            return new NotEnoughRightsException(apiResponse.Description)
                            {
                                Parameters = apiResponse.Parameters
                            };
                        case "method is available for supergroup and channel chats only":
                            return new WrongChatTypeException(apiResponse.Description)
                            {
                                Parameters = apiResponse.Parameters
                            };
                        default:
                            if (message.EndsWith(" is empty"))
                            {
                                return new MissingParameterException(apiResponse.Description, message.Remove(message.IndexOf(" is empty")))
                                {
                                    Parameters = apiResponse.Parameters
                                };
                            }
                            LogMissingError(apiResponse);
                            return new ApiRequestException(apiResponse.Description, 400)
                            {
                                Parameters = apiResponse.Parameters
                            };
                    }
                case 403:
                    message = apiResponse.Description.Remove(0, "Forbidden: ".Length);
                    switch (message.Trim())
                    {
                        case "bot was blocked by the user":
                            return new BotBlockedException(apiResponse.Description)
                            {
                                Parameters = apiResponse.Parameters
                            };
                        default:
                            LogMissingError(apiResponse);
                            return new ApiRequestException(apiResponse.Description, 403)
                            {
                                Parameters = apiResponse.Parameters
                            };
                    }
                default:
                    LogMissingError(apiResponse);
                    return new ApiRequestException(apiResponse.Description, apiResponse.ErrorCode)
                    {
                        Parameters = apiResponse.Parameters
                    };
            }
            */
        }
    }
}
