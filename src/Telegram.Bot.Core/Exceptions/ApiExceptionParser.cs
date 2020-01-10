using System;
using System.Linq;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Exceptions
{
    internal static class ApiExceptionParser
    {
        private static readonly IApiExceptionInfo<ApiRequestException>[] ExceptionInfos =
        {
            new BadRequestExceptionInfo<ChatNotFoundException>("chat not found"),
            new BadRequestExceptionInfo<UserNotFoundException>("user not found"),
            new BadRequestExceptionInfo<InvalidUserIdException>("USER_ID_INVALID"),
            new BadRequestExceptionInfo<InvalidQueryIdException>("QUERY_ID_INVALID"),

            #region Stickers

            new BadRequestExceptionInfo<InvalidStickerSetNameException>("sticker set name invalid"),
            new BadRequestExceptionInfo<InvalidStickerEmojisException>("invalid sticker emojis"),
            new BadRequestExceptionInfo<InvalidStickerDimensionsException>("STICKER_PNG_DIMENSIONS"),
            new BadRequestExceptionInfo<StickerSetNameExistsException>("sticker set name is already occupied"),
            new BadRequestExceptionInfo<StickerSetNotModifiedException>("STICKERSET_NOT_MODIFIED"),

            #endregion

            #region Games

            new BadRequestExceptionInfo<InvalidGameShortNameException>("GAME_SHORTNAME_INVALID"),
            new BadRequestExceptionInfo<InvalidGameShortNameException>("game_short_name is empty"),
            new BadRequestExceptionInfo<InvalidGameShortNameException>("wrong game short name specified"),

            #endregion

            new BadRequestExceptionInfo<ContactRequestException>("phone number can be requested in a private chats only"),

            new ForbiddenExceptionInfo<ChatNotInitiatedException>("bot can't initiate conversation with a user"),

            new BadRequestExceptionInfo<InvalidParameterException>($@"\w{{3,}} Request: invalid (?<{InvalidParameterException.ParamGroupName}>[\w|\s]+)$"),
            new BadRequestExceptionInfo<InvalidParameterException>($@"\w{{3,}} Request: (?<{InvalidParameterException.ParamGroupName}>[\w|\s]+) invalid$"),

            new BadRequestExceptionInfo<MessageIsNotModifiedException>("message is not modified"),
        };

        public static ApiRequestException Parse<T>(ApiResponse<T> apiResponse)
        {
            ApiRequestException exception;

            var typeInfo = ExceptionInfos
                .FirstOrDefault(info => Regex.IsMatch(apiResponse.Description, info.ErrorMessageRegex));

            if (typeInfo is null)
            {
                exception = new ApiRequestException(apiResponse.Description, apiResponse.ErrorCode, apiResponse.Parameters);
            }
            else
            {
                string errorMessage;
                bool isBadRequestError = typeInfo.ErrorCode == BadRequestException.BadRequestErrorCode;

                if (isBadRequestError)
                {
                    errorMessage = TruncateBadRequestErrorDescription(apiResponse.Description);

                    if (typeInfo.Type == typeof(InvalidParameterException))
                    {
                        string paramName = Regex.Match(apiResponse.Description, typeInfo.ErrorMessageRegex)
                            .Groups[InvalidParameterException.ParamGroupName]
                            .Value;
                        exception = new InvalidParameterException(paramName, errorMessage);
                    }
                    else
                        exception = Activator.CreateInstance(typeInfo.Type, errorMessage) as ApiRequestException;
                }
                else
                {
                    errorMessage = TruncateForbiddenErrorDescription(apiResponse.Description);
                    exception = Activator.CreateInstance(typeInfo.Type, errorMessage) as ApiRequestException;
                }
            }
            return exception;
        }

        private static string TruncateBadRequestErrorDescription(string message) =>
            TryTruncateErrorDescription(message, BadRequestException.BadRequestErrorDescription);

        private static string TruncateForbiddenErrorDescription(string message) =>
            TryTruncateErrorDescription(message, ForbiddenException.ForbiddenErrorDescription);

        private static string TryTruncateErrorDescription(string message, string description)
        {
            bool hasErrorTypeDescription = message?.IndexOf(description) == 0;
            if (hasErrorTypeDescription)
                message = message.Substring(description.Length);
            return message;
        }
    }
}
