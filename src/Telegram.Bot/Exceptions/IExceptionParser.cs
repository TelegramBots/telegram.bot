using Telegram.Bot.Types;

namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// Parses unsuccessful responses from Telegram Bot API to make specific exceptions
    /// </summary>
    public interface IExceptionParser
    {
        /// <summary>
        /// Parses HTTP response and constructs a specific exception out of it
        /// </summary>
        /// <param name="apiResponse">ApiResponse with an error</param>
        /// <returns></returns>
        ApiRequestException Parse(ApiResponse apiResponse);
    }
}
