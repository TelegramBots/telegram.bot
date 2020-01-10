using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Helpers;
using Telegram.Bot.Json;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using File = Telegram.Bot.Types.File;

namespace Telegram.Bot
{
    /// <summary>
    /// A client to use the Telegram Bot API
    /// </summary>
    public class TelegramBotClient : ITelegramBotClient
    {
        /// <inheritdoc/>
        public int BotId { get; }

        private readonly string _baseRequestUrl;
        private readonly string _baseFileRequestUrl;

        private readonly HttpClient _httpClient;

        /// <summary>
        /// Create a new <see cref="TelegramBotClient"/> instance.
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="httpClient">Http client that is used to send the request to Telegram Bot API</param>
        /// <param name="jsonConverter">Json converter. Defaults to NewtonsoftTelegramBotJsonConverter</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="token"/> format is invalid</exception>
        public TelegramBotClient(string token, HttpClient httpClient = null,
                                 ITelegramBotJsonConverter jsonConverter = null)
        {
            _ = token ?? throw new ArgumentNullException(nameof(token));

            string[] parts = token.Split(new[] {':'}, 1);
            if (parts.Length > 1 && int.TryParse(parts[0], out int id))
            {
                BotId = id;
            }
            else
            {
                throw new ArgumentException(
                    "Invalid format. A valid token looks like \"1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy\".",
                    nameof(token)
                );
            }

            _httpClient = httpClient ?? new HttpClient();
            _baseRequestUrl = $"{Defaults.BaseUrl}{token}/";
            _baseFileRequestUrl = $"{Defaults.BaseFileUrl}{token}/";

            if (jsonConverter != null)
                JsonConverter = jsonConverter;
        }

        #region Helpers

        /// <inheritdoc />
        public async Task<TResponse> MakeRequestAsync<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            string url = _baseRequestUrl + request.MethodName;

            var httpRequest = new HttpRequestMessage(request.Method, url)
            {
                Content = await request.ToHttpContentAsync(JsonConverter, cancellationToken)
            };

            HttpResponseMessage httpResponse;
            try
            {
                httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException e)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw;

                throw new ApiRequestException("Request timed out", 408, e);
            }

            // required since user might be able to set new status code using following event arg
            var actualResponseStatusCode = httpResponse.StatusCode;
            string responseJson = await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            switch (actualResponseStatusCode)
            {
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest when !string.IsNullOrWhiteSpace(responseJson):
                case HttpStatusCode.Forbidden when !string.IsNullOrWhiteSpace(responseJson):
                case HttpStatusCode.Conflict when !string.IsNullOrWhiteSpace(responseJson):
                    // Do NOT throw here, an ApiRequestException will be thrown next
                    break;
                default:
                    httpResponse.EnsureSuccessStatusCode();
                    break;
            }

            var apiResponse =
                JsonConvert.DeserializeObject<ApiResponse<TResponse>>(responseJson)
                ?? new ApiResponse<TResponse> // ToDo is required? unit test
                {
                    Ok = false,
                    Description = "No response received"
                };

            if (!apiResponse.Ok)
                throw ApiExceptionParser.Parse(apiResponse);

            return apiResponse.Result;
        }

        /// <summary>
        /// Test the API token
        /// </summary>
        /// <returns><c>true</c> if token is valid</returns>
        public async Task<bool> TestApiAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await GetMeAsync(cancellationToken).ConfigureAwait(false);
                return true;
            }
            catch (ApiRequestException e)
                when (e.ErrorCode == 401)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public ITelegramBotJsonConverter JsonConverter { get; } = new NewtonsoftTelegramBotJsonConverter();

        #endregion Helpers

        #region Getting updates

        /// <inheritdoc />
        public Task<Update[]> GetUpdatesAsync(
            int offset = default,
            int limit = default,
            int timeout = default,
            IEnumerable<UpdateType> allowedUpdates = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new GetUpdatesRequest
            {
                Offset = offset,
                Limit = limit,
                Timeout = timeout,
                AllowedUpdates = allowedUpdates
            }, cancellationToken);

        /// <inheritdoc />
        public Task SetWebhookAsync(
            string url,
            InputFileStream certificate = default,
            int maxConnections = default,
            IEnumerable<UpdateType> allowedUpdates = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SetWebhookRequest(url, certificate)
            {
                MaxConnections = maxConnections,
                AllowedUpdates = allowedUpdates
            }, cancellationToken);

        /// <inheritdoc />
        public Task DeleteWebhookAsync(CancellationToken cancellationToken = default)
            => MakeRequestAsync(new DeleteWebhookRequest(), cancellationToken);

        /// <inheritdoc />
        public Task<WebhookInfo> GetWebhookInfoAsync(CancellationToken cancellationToken = default)
            => MakeRequestAsync(new GetWebhookInfoRequest(), cancellationToken);

        #endregion Getting updates

        #region Available methods

        /// <inheritdoc />
        public Task<User> GetMeAsync(CancellationToken cancellationToken = default)
            => MakeRequestAsync(new GetMeRequest(), cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendTextMessageAsync(
            ChatId chatId,
            string text,
            ParseMode parseMode = default,
            bool disableWebPagePreview = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendMessageRequest(chatId, text)
            {
                ParseMode = parseMode,
                DisableWebPagePreview = disableWebPagePreview,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> ForwardMessageAsync(
            ChatId chatId,
            ChatId fromChatId,
            int messageId,
            bool disableNotification = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new ForwardMessageRequest(chatId, fromChatId, messageId)
            {
                DisableNotification = disableNotification
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendPhotoAsync(
            ChatId chatId,
            InputOnlineFile photo,
            string caption = default,
            ParseMode parseMode = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendPhotoRequest(chatId, photo)
            {
                Caption = caption,
                ParseMode = parseMode,
                ReplyToMessageId = replyToMessageId,
                DisableNotification = disableNotification,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendAudioAsync(
            ChatId chatId,
            InputOnlineFile audio,
            string caption = default,
            ParseMode parseMode = default,
            int duration = default,
            string performer = default,
            string title = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default,
            InputMedia thumb = default
        ) =>
            MakeRequestAsync(new SendAudioRequest(chatId, audio)
            {
                Caption = caption,
                ParseMode = parseMode,
                Duration = duration,
                Performer = performer,
                Title = title,
                Thumb = thumb,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendDocumentAsync(
            ChatId chatId,
            InputOnlineFile document,
            string caption = default,
            ParseMode parseMode = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default,
            InputMedia thumb = default
        ) =>
            MakeRequestAsync(new SendDocumentRequest(chatId, document)
            {
                Caption = caption,
                Thumb = thumb,
                ParseMode = parseMode,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendStickerAsync(
            ChatId chatId,
            InputOnlineFile sticker,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendStickerRequest(chatId, sticker)
            {
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendVideoAsync(
            ChatId chatId,
            InputOnlineFile video,
            int duration = default,
            int width = default,
            int height = default,
            string caption = default,
            ParseMode parseMode = default,
            bool supportsStreaming = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default,
            InputMedia thumb = default
        ) =>
            MakeRequestAsync(new SendVideoRequest(chatId, video)
            {
                Duration = duration,
                Width = width,
                Height = height,
                Thumb = thumb,
                Caption = caption,
                ParseMode = parseMode,
                SupportsStreaming = supportsStreaming,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendAnimationAsync(
            ChatId chatId,
            InputOnlineFile animation,
            int duration = default,
            int width = default,
            int height = default,
            InputMedia thumb = default,
            string caption = default,
            ParseMode parseMode = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendAnimationRequest(chatId, animation)
            {
                Duration = duration,
                Width = width,
                Height = height,
                Thumb = thumb,
                Caption = caption,
                ParseMode = parseMode,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup,
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendVoiceAsync(
            ChatId chatId,
            InputOnlineFile voice,
            string caption = default,
            ParseMode parseMode = default,
            int duration = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendVoiceRequest(chatId, voice)
            {
                Caption = caption,
                ParseMode = parseMode,
                Duration = duration,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendVideoNoteAsync(
            ChatId chatId,
            InputTelegramFile videoNote,
            int duration = default,
            int length = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default,
            InputMedia thumb = default
        ) =>
            MakeRequestAsync(new SendVideoNoteRequest(chatId, videoNote)
            {
                Duration = duration,
                Length = length,
                Thumb = thumb,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        [Obsolete("Use the other overload of this method instead. Only photo and video input types are allowed.")]
        public Task<Message[]> SendMediaGroupAsync(
            ChatId chatId,
            IEnumerable<InputMediaBase> media,
            bool disableNotification = default,
            int replyToMessageId = default,
            CancellationToken cancellationToken = default
        )
        {
            var inputMedia = media
                .Select(m => m as IAlbumInputMedia)
                .Where(m => m != null)
                .ToArray();
            return MakeRequestAsync(new SendMediaGroupRequest(chatId, inputMedia)
            {
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
            }, cancellationToken);
        }

        /// <inheritdoc />
        public Task<Message[]> SendMediaGroupAsync(
            IEnumerable<IAlbumInputMedia> inputMedia,
            ChatId chatId,
            bool disableNotification = default,
            int replyToMessageId = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendMediaGroupRequest(chatId, inputMedia)
            {
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendLocationAsync(
            ChatId chatId,
            float latitude,
            float longitude,
            int livePeriod = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendLocationRequest(chatId, latitude, longitude)
            {
                LivePeriod = livePeriod,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendVenueAsync(
            ChatId chatId,
            float latitude,
            float longitude,
            string title,
            string address,
            string foursquareId = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default,
            string foursquareType = default
        ) =>
            MakeRequestAsync(new SendVenueRequest(chatId, latitude, longitude, title, address)
            {
                FoursquareId = foursquareId,
                FoursquareType = foursquareType,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendContactAsync(
            ChatId chatId,
            string phoneNumber,
            string firstName,
            string lastName = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default,
            string vCard = default
        ) =>
            MakeRequestAsync(new SendContactRequest(chatId, phoneNumber, firstName)
            {
                LastName = lastName,
                Vcard = vCard,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendPollAsync(
            ChatId chatId,
            string question,
            IEnumerable<string> options,
            bool disableNotification = default,
            int replyToMessageId = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendPollRequest(chatId, question, options)
            {
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task SendChatActionAsync(
            ChatId chatId,
            ChatAction chatAction,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendChatActionRequest(chatId, chatAction), cancellationToken);

        /// <inheritdoc />
        public Task<UserProfilePhotos> GetUserProfilePhotosAsync(
            int userId,
            int offset = default,
            int limit = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new GetUserProfilePhotosRequest(userId)
            {
                Offset = offset,
                Limit = limit
            }, cancellationToken);

        /// <inheritdoc />
        public Task<File> GetFileAsync(
            string fileId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new GetFileRequest(fileId), cancellationToken);

        /// <inheritdoc />
        [Obsolete("This method will be removed in next major release. Use its overload instead.")]
        public async Task<Stream> DownloadFileAsync(
            string filePath,
            CancellationToken cancellationToken = default
        )
        {
            var stream = new MemoryStream();
            await DownloadFileAsync(filePath, stream, cancellationToken)
                .ConfigureAwait(false);
            return stream;
        }

        /// <inheritdoc />
        public async Task DownloadFileAsync(
            string filePath,
            Stream destination,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrWhiteSpace(filePath) || filePath.Length < 2)
            {
                throw new ArgumentException("Invalid file path", nameof(filePath));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var fileUri = new Uri(_baseFileRequestUrl + filePath);

            var response = await _httpClient
                .GetAsync(fileUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            using (response)
            {
                await response.Content.CopyToAsync(destination)
                    .ConfigureAwait(false);
            }
        }

        /// <inheritdoc />
        public async Task<File> GetInfoAndDownloadFileAsync(
            string fileId,
            Stream destination,
            CancellationToken cancellationToken = default
        )
        {
            var file = await GetFileAsync(fileId, cancellationToken)
                .ConfigureAwait(false);

            await DownloadFileAsync(file.FilePath, destination, cancellationToken)
                .ConfigureAwait(false);

            return file;
        }

        /// <inheritdoc />
        public Task KickChatMemberAsync(
            ChatId chatId,
            int userId,
            DateTime untilDate = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new KickChatMemberRequest(chatId, userId)
            {
                UntilDate = untilDate
            }, cancellationToken);

        /// <inheritdoc />
        public Task LeaveChatAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new LeaveChatRequest(chatId), cancellationToken);

        /// <inheritdoc />
        public Task UnbanChatMemberAsync(
            ChatId chatId,
            int userId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new UnbanChatMemberRequest(chatId, userId), cancellationToken);

        /// <inheritdoc />
        public Task<Chat> GetChatAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new GetChatRequest(chatId), cancellationToken);

        /// <inheritdoc />
        public Task<ChatMember[]> GetChatAdministratorsAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new GetChatAdministratorsRequest(chatId), cancellationToken);

        /// <inheritdoc />
        public Task<int> GetChatMembersCountAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new GetChatMembersCountRequest(chatId), cancellationToken);

        /// <inheritdoc />
        public Task<ChatMember> GetChatMemberAsync(
            ChatId chatId,
            int userId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new GetChatMemberRequest(chatId, userId), cancellationToken);

        /// <inheritdoc />
        public Task AnswerCallbackQueryAsync(
            string callbackQueryId,
            string text = default,
            bool showAlert = default,
            string url = default,
            int cacheTime = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new AnswerCallbackQueryRequest(callbackQueryId)
            {
                Text = text,
                ShowAlert = showAlert,
                Url = url,
                CacheTime = cacheTime
            }, cancellationToken);

        /// <inheritdoc />
        public Task RestrictChatMemberAsync(
            ChatId chatId,
            int userId,
            ChatPermissions permissions,
            DateTime untilDate = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(
                new RestrictChatMemberRequest(chatId, userId, permissions)
                {
                    UntilDate = untilDate
                },
                cancellationToken);

        /// <inheritdoc />
        public Task PromoteChatMemberAsync(
            ChatId chatId,
            int userId,
            bool? canChangeInfo = default,
            bool? canPostMessages = default,
            bool? canEditMessages = default,
            bool? canDeleteMessages = default,
            bool? canInviteUsers = default,
            bool? canRestrictMembers = default,
            bool? canPinMessages = default,
            bool? canPromoteMembers = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new PromoteChatMemberRequest(chatId, userId)
            {
                CanChangeInfo = canChangeInfo,
                CanPostMessages = canPostMessages,
                CanEditMessages = canEditMessages,
                CanDeleteMessages = canDeleteMessages,
                CanInviteUsers = canInviteUsers,
                CanRestrictMembers = canRestrictMembers,
                CanPinMessages = canPinMessages,
                CanPromoteMembers = canPromoteMembers
            }, cancellationToken);

        /// <inheritdoc />
        public Task SetChatAdministratorCustomTitleAsync(
            ChatId chatId,
            int userId,
            string customTitle,
            CancellationToken cancellationToken = default)
            => MakeRequestAsync(
                new SetChatAdministratorCustomTitleRequest(chatId, userId, customTitle),
                cancellationToken);

        /// <inheritdoc />
        public Task SetChatPermissionsAsync(
            ChatId chatId,
            ChatPermissions permissions,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SetChatPermissionsRequest(chatId, permissions), cancellationToken);

        /// <inheritdoc />
        public Task<Message> StopMessageLiveLocationAsync(
            ChatId chatId,
            int messageId,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new StopMessageLiveLocationRequest(chatId, messageId)
            {
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task StopMessageLiveLocationAsync(
            string inlineMessageId,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new StopInlineMessageLiveLocationRequest(inlineMessageId)
            {
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        #endregion Available methods

        #region Updating messages

        /// <inheritdoc />
        public Task<Message> EditMessageTextAsync(
            ChatId chatId,
            int messageId,
            string text,
            ParseMode parseMode = default,
            bool disableWebPagePreview = default,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditMessageTextRequest(chatId, messageId, text)
            {
                ParseMode = parseMode,
                DisableWebPagePreview = disableWebPagePreview,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task EditMessageTextAsync(
            string inlineMessageId,
            string text,
            ParseMode parseMode = default,
            bool disableWebPagePreview = default,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditInlineMessageTextRequest(inlineMessageId, text)
            {
                DisableWebPagePreview = disableWebPagePreview,
                ReplyMarkup = replyMarkup,
                ParseMode = parseMode
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> EditMessageCaptionAsync(
            ChatId chatId,
            int messageId,
            string caption,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default,
            ParseMode parseMode = default
        ) =>
            MakeRequestAsync(new EditMessageCaptionRequest(chatId, messageId, caption)
            {
                ParseMode = parseMode,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task EditMessageCaptionAsync(
            string inlineMessageId,
            string caption,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default,
            ParseMode parseMode = default
        ) =>
            MakeRequestAsync(new EditInlineMessageCaptionRequest(inlineMessageId, caption)
            {
                ParseMode = parseMode,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> EditMessageMediaAsync(
            ChatId chatId,
            int messageId,
            InputMediaBase media,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditMessageMediaRequest(chatId, messageId, media)
            {
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task EditMessageMediaAsync(
            string inlineMessageId,
            InputMediaBase media,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditInlineMessageMediaRequest(inlineMessageId, media)
            {
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> EditMessageReplyMarkupAsync(
            ChatId chatId,
            int messageId,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(
                new EditMessageReplyMarkupRequest(chatId, messageId, replyMarkup),
                cancellationToken);

        /// <inheritdoc />
        public Task EditMessageReplyMarkupAsync(
            string inlineMessageId,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(
                new EditInlineMessageReplyMarkupRequest(inlineMessageId, replyMarkup),
                cancellationToken);

        /// <inheritdoc />
        public Task<Message> EditMessageLiveLocationAsync(
            ChatId chatId,
            int messageId,
            float latitude,
            float longitude,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditMessageLiveLocationRequest(chatId, messageId, latitude, longitude)
            {
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task EditMessageLiveLocationAsync(
            string inlineMessageId,
            float latitude,
            float longitude,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditInlineMessageLiveLocationRequest(inlineMessageId, latitude, longitude)
            {
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Poll> StopPollAsync(
            ChatId chatId,
            int messageId,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new StopPollRequest(chatId, messageId)
            {
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task DeleteMessageAsync(
            ChatId chatId,
            int messageId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new DeleteMessageRequest(chatId, messageId), cancellationToken);

        #endregion Updating messages

        #region Inline mode

        /// <inheritdoc />
        public Task AnswerInlineQueryAsync(
            string inlineQueryId,
            IEnumerable<InlineQueryResultBase> results,
            int? cacheTime = default,
            bool isPersonal = default,
            string nextOffset = default,
            string switchPmText = default,
            string switchPmParameter = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new AnswerInlineQueryRequest(inlineQueryId, results)
            {
                CacheTime = cacheTime,
                IsPersonal = isPersonal,
                NextOffset = nextOffset,
                SwitchPmText = switchPmText,
                SwitchPmParameter = switchPmParameter
            }, cancellationToken);

        # endregion Inline mode

        #region Payments

        /// <inheritdoc />
        public Task<Message> SendInvoiceAsync(
            int chatId,
            string title,
            string description,
            string payload,
            string providerToken,
            string startParameter,
            string currency,
            IEnumerable<LabeledPrice> prices,
            string providerData = default,
            string photoUrl = default,
            int photoSize = default,
            int photoWidth = default,
            int photoHeight = default,
            bool needName = default,
            bool needPhoneNumber = default,
            bool needEmail = default,
            bool needShippingAddress = default,
            bool isFlexible = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendInvoiceRequest(
                chatId,
                title,
                description,
                payload,
                providerToken,
                startParameter,
                currency,
                // ReSharper disable once PossibleMultipleEnumeration
                prices
            )
            {
                ProviderData = providerData,
                PhotoUrl = photoUrl,
                PhotoSize = photoSize,
                PhotoWidth = photoWidth,
                PhotoHeight = photoHeight,
                NeedName = needName,
                NeedPhoneNumber = needPhoneNumber,
                NeedEmail = needEmail,
                NeedShippingAddress = needShippingAddress,
                IsFlexible = isFlexible,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task AnswerShippingQueryAsync(
            string shippingQueryId,
            IEnumerable<ShippingOption> shippingOptions,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new AnswerShippingQueryRequest(shippingQueryId, shippingOptions), cancellationToken);

        /// <inheritdoc />
        public Task AnswerShippingQueryAsync(
            string shippingQueryId,
            string errorMessage,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new AnswerShippingQueryRequest(shippingQueryId, errorMessage), cancellationToken);

        /// <inheritdoc />
        public Task AnswerPreCheckoutQueryAsync(
            string preCheckoutQueryId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new AnswerPreCheckoutQueryRequest(preCheckoutQueryId), cancellationToken);

        /// <inheritdoc />
        public Task AnswerPreCheckoutQueryAsync(
            string preCheckoutQueryId,
            string errorMessage,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new AnswerPreCheckoutQueryRequest(preCheckoutQueryId, errorMessage), cancellationToken);

        #endregion Payments

        #region Games

        /// <inheritdoc />
        public Task<Message> SendGameAsync(
            long chatId,
            string gameShortName,
            bool disableNotification = default,
            int replyToMessageId = default,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendGameRequest(chatId, gameShortName)
            {
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SetGameScoreAsync(
            int userId,
            int score,
            long chatId,
            int messageId,
            bool force = default,
            bool disableEditMessage = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SetGameScoreRequest(userId, score, chatId, messageId)
            {
                Force = force,
                DisableEditMessage = disableEditMessage
            }, cancellationToken);

        /// <inheritdoc />
        public Task SetGameScoreAsync(
            int userId,
            int score,
            string inlineMessageId,
            bool force = default,
            bool disableEditMessage = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SetInlineGameScoreRequest(userId, score, inlineMessageId)
            {
                Force = force,
                DisableEditMessage = disableEditMessage
            }, cancellationToken);

        /// <inheritdoc />
        public Task<GameHighScore[]> GetGameHighScoresAsync(
            int userId,
            long chatId,
            int messageId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(
                new GetGameHighScoresRequest(userId, chatId, messageId),
                cancellationToken);

        /// <inheritdoc />
        public Task<GameHighScore[]> GetGameHighScoresAsync(
            int userId,
            string inlineMessageId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(
                new GetInlineGameHighScoresRequest(userId, inlineMessageId),
                cancellationToken);

        #endregion Games

        #region Group and channel management

        /// <inheritdoc />
        public Task<string> ExportChatInviteLinkAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new ExportChatInviteLinkRequest(chatId), cancellationToken);

        /// <inheritdoc />
        public Task SetChatPhotoAsync(
            ChatId chatId,
            InputFileStream photo,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SetChatPhotoRequest(chatId, photo), cancellationToken);

        /// <inheritdoc />
        public Task DeleteChatPhotoAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new DeleteChatPhotoRequest(chatId), cancellationToken);

        /// <inheritdoc />
        public Task SetChatTitleAsync(
            ChatId chatId,
            string title,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SetChatTitleRequest(chatId, title), cancellationToken);

        /// <inheritdoc />
        public Task SetChatDescriptionAsync(
            ChatId chatId,
            string description = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SetChatDescriptionRequest(chatId, description), cancellationToken);

        /// <inheritdoc />
        public Task PinChatMessageAsync(
            ChatId chatId,
            int messageId,
            bool disableNotification = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new PinChatMessageRequest(chatId, messageId)
            {
                DisableNotification = disableNotification
            }, cancellationToken);

        /// <inheritdoc />
        public Task UnpinChatMessageAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new UnpinChatMessageRequest(chatId), cancellationToken);

        /// <inheritdoc />
        public Task SetChatStickerSetAsync(
            ChatId chatId,
            string stickerSetName,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SetChatStickerSetRequest(chatId, stickerSetName), cancellationToken);

        /// <inheritdoc />
        public Task DeleteChatStickerSetAsync(
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new DeleteChatStickerSetRequest(chatId), cancellationToken);

        #endregion

        #region Stickers

        /// <inheritdoc />
        public Task<StickerSet> GetStickerSetAsync(
            string name,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new GetStickerSetRequest(name), cancellationToken);

        /// <inheritdoc />
        public Task<File> UploadStickerFileAsync(
            int userId,
            InputFileStream pngSticker,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new UploadStickerFileRequest(userId, pngSticker), cancellationToken);

        /// <inheritdoc />
        public Task CreateNewStickerSetAsync(
            int userId,
            string name,
            string title,
            InputOnlineFile pngSticker,
            string emojis,
            bool isMasks = default,
            MaskPosition maskPosition = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new CreateNewStickerSetRequest(userId, name, title, pngSticker, emojis)
            {
                ContainsMasks = isMasks,
                MaskPosition = maskPosition
            }, cancellationToken);

        /// <inheritdoc />
        public Task AddStickerToSetAsync(
            int userId,
            string name,
            InputOnlineFile pngSticker,
            string emojis,
            MaskPosition maskPosition = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new AddStickerToSetRequest(userId, name, pngSticker, emojis)
            {
                MaskPosition = maskPosition
            }, cancellationToken);

        /// <inheritdoc />
        public Task SetStickerPositionInSetAsync(
            string sticker,
            int position,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SetStickerPositionInSetRequest(sticker, position), cancellationToken);

        /// <inheritdoc />
        public Task DeleteStickerFromSetAsync(
            string sticker,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new DeleteStickerFromSetRequest(sticker), cancellationToken);

        #endregion
    }
}
