using System;

namespace Telegram.Bot.Exceptions
{
    // ReSharper disable once UnusedTypeParameter
    internal interface IApiExceptionInfo<out T>
        where T : ApiRequestException
    {
        int ErrorCode { get; }

        string ErrorMessageRegex { get; }

        Type Type { get; }
    }

    internal class BadRequestExceptionInfo<T> : IApiExceptionInfo<T>
        where T : ApiRequestException
    {
        public int ErrorCode => BadRequestException.BadRequestErrorCode;

        public string ErrorMessageRegex { get; }

        public Type Type => typeof(T);

        public BadRequestExceptionInfo(string errorMessageRegex)
        {
            ErrorMessageRegex = errorMessageRegex;
        }
    }

    internal class ForbiddenExceptionInfo<T> : IApiExceptionInfo<T>
        where T : ApiRequestException
    {
        public int ErrorCode => ForbiddenException.ForbiddenErrorCode;

        public string ErrorMessageRegex { get; }

        public Type Type => typeof(T);

        public ForbiddenExceptionInfo(string errorMessageRegex)
        {
            ErrorMessageRegex = errorMessageRegex;
        }
    }
}
