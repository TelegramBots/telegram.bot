namespace Telegram.Bot.Exceptions
{
    internal class ExceptionParser : IExceptionParser
    {
        public ApiRequestException Parse(ApiResponse apiResponse) =>
            new ApiRequestException(apiResponse.Description, apiResponse.ErrorCode, apiResponse.Parameters);
    }
}
