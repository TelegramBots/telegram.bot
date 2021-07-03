using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
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
    public class TelegramBotClient : ITelegramBotClient, IDisposable
    {
        private const string BaseTelegramUrl = "https://api.telegram.org";
        private static readonly Update[] EmptyUpdates = { };

        // Max long value that fits 52 bits is 9007199254740991 and is 16 chars long
        private static readonly Regex TokenFormat = new Regex("^(?<token>[-]?[0-9]{1,16}):.*$", RegexOptions.Compiled);

        private readonly string _baseFileUrl;
        private readonly string _baseRequestUrl;
        private readonly bool _localBotServer;
        private readonly HttpClient _httpClient;
        private CancellationTokenSource _receivingCancellationTokenSource;

        #region Config Properties

        /// <inheritdoc/>
        public long? BotId { get; }

        /// <summary>
        /// Timeout for requests
        /// </summary>
        public TimeSpan Timeout
        {
            get => _httpClient.Timeout;
            set => _httpClient.Timeout = value;
        }

        /// <summary>
        /// Indicates if receiving updates
        /// </summary>
        [Obsolete("This property will be removed in the next major version. " +
            "Please consider using Telegram.Bot.Extensions.Polling instead.")]
        public bool IsReceiving { get; set; }

        /// <summary>
        /// The current message offset
        /// </summary>
        [Obsolete("This property will be removed in the next major version. " +
            "Please consider using Telegram.Bot.Extensions.Polling instead.")]
        public int MessageOffset { get; set; }

        #endregion Config Properties

        #region Events

        /// <summary>
        /// Occurs before sending a request to API
        /// </summary>
        public event AsyncEventHandler<ApiRequestEventArgs> OnMakingApiRequest;

        /// <summary>
        /// Occurs after receiving the response to an API request
        /// </summary>
        public event AsyncEventHandler<ApiResponseEventArgs> OnApiResponseReceived;

        /// <summary>
        /// Raises the <see cref="OnUpdate" />, <see cref="OnMessage"/>,
        /// <see cref="OnInlineQuery"/>, <see cref="OnInlineResultChosen"/> and
        /// <see cref="OnCallbackQuery"/> events.
        /// </summary>
        /// <param name="e">
        /// The <see cref="UpdateEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnUpdateReceived(UpdateEventArgs e)
        {
            OnUpdate?.Invoke(this, e);

            switch (e.Update.Type)
            {
                case UpdateType.Message:
                    OnMessage?.Invoke(this, e);
                    break;

                case UpdateType.InlineQuery:
                    OnInlineQuery?.Invoke(this, e);
                    break;

                case UpdateType.ChosenInlineResult:
                    OnInlineResultChosen?.Invoke(this, e);
                    break;

                case UpdateType.CallbackQuery:
                    OnCallbackQuery?.Invoke(this, e);
                    break;

                case UpdateType.EditedMessage:
                    OnMessageEdited?.Invoke(this, e);
                    break;
            }
        }

        /// <summary>
        /// Occurs when an <see cref="Update"/> is received.
        /// </summary>
        [Obsolete("This event will be removed in the next major version. " +
            "Please consider using Telegram.Bot.Extensions.Polling instead.")]
        public event EventHandler<UpdateEventArgs> OnUpdate;

        /// <summary>
        /// Occurs when a <see cref="Message"/> is received.
        /// </summary>
        [Obsolete("This event will be removed in the next major version. " +
            "Please consider using Telegram.Bot.Extensions.Polling instead.")]
        public event EventHandler<MessageEventArgs> OnMessage;

        /// <summary>
        /// Occurs when <see cref="Message"/> was edited.
        /// </summary>
        [Obsolete("This event will be removed in the next major version. " +
            "Please consider using Telegram.Bot.Extensions.Polling instead.")]
        public event EventHandler<MessageEventArgs> OnMessageEdited;

        /// <summary>
        /// Occurs when an <see cref="InlineQuery"/> is received.
        /// </summary>
        [Obsolete("This event will be removed in the next major version. " +
            "Please consider using Telegram.Bot.Extensions.Polling instead.")]
        public event EventHandler<InlineQueryEventArgs> OnInlineQuery;

        /// <summary>
        /// Occurs when a <see cref="ChosenInlineResult"/> is received.
        /// </summary>
        [Obsolete("This event will be removed in the next major version. " +
            "Please consider using Telegram.Bot.Extensions.Polling instead.")]
        public event EventHandler<ChosenInlineResultEventArgs> OnInlineResultChosen;

        /// <summary>
        /// Occurs when an <see cref="CallbackQuery"/> is received
        /// </summary>
        [Obsolete("This event will be removed in the next major version. " +
            "Please consider using Telegram.Bot.Extensions.Polling instead.")]
        public event EventHandler<CallbackQueryEventArgs> OnCallbackQuery;

        /// <summary>
        /// Occurs when an error occurs during the background update pooling.
        /// </summary>
        [Obsolete("This event will be removed in the next major version. " +
            "Please consider using Telegram.Bot.Extensions.Polling instead.")]
        public event EventHandler<ReceiveErrorEventArgs> OnReceiveError;

        /// <summary>
        /// Occurs when an error occurs during the background update pooling.
        /// </summary>
        [Obsolete("This event will be removed in the next major version. " +
            "Please consider using Telegram.Bot.Extensions.Polling instead.")]
        public event EventHandler<ReceiveGeneralErrorEventArgs> OnReceiveGeneralError;

        #endregion

        #region Construction and disposal

        /// <summary>
        /// Create a new <see cref="TelegramBotClient"/> instance.
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="httpClient">A custom <see cref="HttpClient"/></param>
        /// <param name="baseUrl">
        /// Used to change base url to your private bot api server URL. It looks like
        /// http://localhost:8081. Path, query and fragment will be omitted if present.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="token"/> format is invalid
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="baseUrl"/> format is invalid
        /// </exception>
        public TelegramBotClient(
            string token,
            HttpClient httpClient = null,
            string baseUrl = default)
        {
            if (token is null) throw new ArgumentNullException(nameof(token));

            var match = TokenFormat.Match(token);
            if (match.Success)
            {
                var botIdString = match.Groups["token"].Value;
                if (long.TryParse(botIdString, out var botId))
                {
                    BotId = botId;
                }
            }

            _localBotServer = TrySetBaseUrl(
                baseUrl ?? BaseTelegramUrl,
                out var effectiveBaseUrl
            );

            _baseRequestUrl = $"{effectiveBaseUrl}/bot{token}";
            _baseFileUrl = $"{effectiveBaseUrl}/file/bot{token}";
            _httpClient = httpClient ?? new HttpClient();
            _isCustomHttpClientUsed = httpClient != null;
        }

        /// <summary>
        /// Create a new <see cref="TelegramBotClient"/> instance behind a proxy.
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="webProxy">Use this <see cref="IWebProxy"/> to connect to the API</param>
        /// <param name="baseUrl">
        /// Used to change base url to your private bot api server URL. It looks like
        /// http://localhost:8081. Path, query and fragment will be omitted if present.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="token"/> format is invalid
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="baseUrl"/> format is invalid
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="webProxy"/> is null
        /// </exception>
        [Obsolete("Provide httpClient with configured proxy instead.")]
        public TelegramBotClient(string token, IWebProxy webProxy, string baseUrl = default)
            : this(token, CreateHttpClient(webProxy), baseUrl)
        { }

        private bool _isDisposed;
        private readonly bool _isCustomHttpClientUsed;

        /// <summary>
        /// Allows a <see cref="TelegramBotClient"/> to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~TelegramBotClient() => dispose();

        /// <summary>
        /// Disposes this <see cref="TelegramBotClient"/>
        /// </summary>
        public void Dispose()
        {
            dispose();
            GC.SuppressFinalize(this);
        }

        private void dispose()
        {
            if (_isDisposed)
                return;

            if (!_isCustomHttpClientUsed)
                _httpClient.Dispose();

            _isDisposed = true;
        }

        #endregion

        #region Helpers

        /// <inheritdoc />
        public async Task<TResponse> MakeRequestAsync<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            var url = $"{_baseRequestUrl}/{request.MethodName}";

            var httpRequest = new HttpRequestMessage(request.Method, url)
            {
                Content = request.ToHttpContent()
            };

            var reqDataArgs = new ApiRequestEventArgs
            {
                MethodName = request.MethodName,
                HttpContent = httpRequest.Content,
            };

            if (OnMakingApiRequest != null)
            {
                await OnMakingApiRequest.Invoke(this, reqDataArgs, cancellationToken)
                    .ConfigureAwait(false);
            }

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

            if (OnApiResponseReceived != null)
            {
                await OnApiResponseReceived.Invoke(
                    this,
                    new ApiResponseEventArgs
                    {
                        ResponseMessage = httpResponse,
                        ApiRequestEventArgs = reqDataArgs
                    },
                    cancellationToken
                ).ConfigureAwait(false);
            }

            switch (actualResponseStatusCode)
            {
                case HttpStatusCode.OK:
                    break;
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.BadRequest when !string.IsNullOrWhiteSpace(responseJson):
                case HttpStatusCode.Forbidden when !string.IsNullOrWhiteSpace(responseJson):
                case HttpStatusCode.Conflict when !string.IsNullOrWhiteSpace(responseJson):
                case (HttpStatusCode)429 when !string.IsNullOrWhiteSpace(responseJson):
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

        /// <summary>
        /// Start update receiving
        /// </summary>
        /// <param name="allowedUpdates">List the types of updates you want your bot to receive.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ApiRequestException"> Thrown if token is invalid</exception>
        [Obsolete("This method will be removed in the next major version. " +
            "Please consider using Telegram.Bot.Extensions.Polling instead.")]
        public void StartReceiving(UpdateType[] allowedUpdates = null,
                                   CancellationToken cancellationToken = default)
        {
            _receivingCancellationTokenSource = new CancellationTokenSource();

            cancellationToken.Register(() => _receivingCancellationTokenSource.Cancel());

            ReceiveAsync(allowedUpdates, _receivingCancellationTokenSource.Token);
        }

#pragma warning disable AsyncFixer03 // Avoid fire & forget async void methods
        private async void ReceiveAsync(
            UpdateType[] allowedUpdates,
            CancellationToken cancellationToken = default)
        {
            IsReceiving = true;
            while (!cancellationToken.IsCancellationRequested)
            {
                var timeout = Convert.ToInt32(Timeout.TotalSeconds);
                var updates = EmptyUpdates;

                try
                {
                    updates = await GetUpdatesAsync(
                        MessageOffset,
                        timeout: timeout,
                        allowedUpdates: allowedUpdates,
                        cancellationToken: cancellationToken
                    ).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                }
                catch (ApiRequestException apiException)
                {
                    OnReceiveError?.Invoke(this, apiException);
                }
                catch (Exception generalException)
                {
                    OnReceiveGeneralError?.Invoke(this, generalException);
                }

                try
                {
                    foreach (var update in updates)
                    {
                        OnUpdateReceived(new UpdateEventArgs(update));
                        MessageOffset = update.Id + 1;
                    }
                }
                catch
                {
                    IsReceiving = false;
                    throw;
                }
            }

            IsReceiving = false;
        }
#pragma warning restore AsyncFixer03 // Avoid fire & forget async void methods

        /// <summary>
        /// Stop update receiving
        /// </summary>
        [Obsolete("This method will be removed in the next major version. " +
            "Please consider using Telegram.Bot.Extensions.Polling instead.")]
        public void StopReceiving()
        {
            try
            {
                _receivingCancellationTokenSource.Cancel();
            }
            catch (WebException)
            {
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                _receivingCancellationTokenSource.Dispose();
            }
        }

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
            string ipAddress = default,
            int maxConnections = default,
            IEnumerable<UpdateType> allowedUpdates = default,
            bool dropPendingUpdates = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SetWebhookRequest(url, certificate)
            {
                MaxConnections = maxConnections,
                AllowedUpdates = allowedUpdates,
                IpAddress = ipAddress,
                DropPendingUpdates = dropPendingUpdates
            }, cancellationToken);

        /// <inheritdoc />
        public Task DeleteWebhookAsync(
            bool dropPendingUpdates = default,
            CancellationToken cancellationToken = default
        )
            => MakeRequestAsync(new DeleteWebhookRequest() { DropPendingUpdates = dropPendingUpdates }, cancellationToken);

        /// <inheritdoc />
        public Task<WebhookInfo> GetWebhookInfoAsync(CancellationToken cancellationToken = default)
            => MakeRequestAsync(new GetWebhookInfoRequest(), cancellationToken);

        #endregion Getting updates

        #region Available methods

        /// <inheritdoc />
        public Task<User> GetMeAsync(CancellationToken cancellationToken = default)
            => MakeRequestAsync(new GetMeRequest(), cancellationToken);

        /// <inheritdoc />
        public Task LogOutAsync(CancellationToken cancellationToken = default)
            => MakeRequestAsync(new LogOutRequest(), cancellationToken);

        /// <inheritdoc />
        public Task CloseAsync(CancellationToken cancellationToken = default)
            => MakeRequestAsync(new CloseRequest(), cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendTextMessageAsync(
            ChatId chatId,
            string text,
            ParseMode parseMode = default,
            IEnumerable<MessageEntity> entities = default,
            bool disableWebPagePreview = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendMessageRequest(chatId, text)
            {
                ParseMode = parseMode,
                Entities = entities,
                DisableWebPagePreview = disableWebPagePreview,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
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
        public Task<MessageId> CopyMessageAsync(ChatId chatId, ChatId fromChatId, int messageId,
                                                 string caption = default,
                                                 ParseMode parseMode = default,
                                                 IEnumerable<MessageEntity> captionEntities = default,
                                                 int replyToMessageId = default,
                                                 bool disableNotification = default,
                                                 IReplyMarkup replyMarkup = default,
                                                 CancellationToken cancellationToken = default)
            => MakeRequestAsync(new CopyMessageRequest(chatId, fromChatId, messageId)
            {
                Caption = caption,
                ParseMode = parseMode,
                CaptionEntities = captionEntities,
                ReplyToMessageId = replyToMessageId,
                DisableNotification = disableNotification,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendPhotoAsync(
            ChatId chatId,
            InputOnlineFile photo,
            string caption = default,
            ParseMode parseMode = default,
            IEnumerable<MessageEntity> captionEntities = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendPhotoRequest(chatId, photo)
            {
                Caption = caption,
                ParseMode = parseMode,
                CaptionEntities = captionEntities,
                ReplyToMessageId = replyToMessageId,
                DisableNotification = disableNotification,
                AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendAudioAsync(
            ChatId chatId,
            InputOnlineFile audio,
            string caption = default,
            ParseMode parseMode = default,
            IEnumerable<MessageEntity> captionEntities = default,
            int duration = default,
            string performer = default,
            string title = default,
            InputMedia thumb = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendAudioRequest(chatId, audio)
            {
                Caption = caption,
                ParseMode = parseMode,
                CaptionEntities = captionEntities,
                Duration = duration,
                Performer = performer,
                Title = title,
                Thumb = thumb,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendDocumentAsync(
            ChatId chatId,
            InputOnlineFile document,
            InputMedia thumb = default,
            string caption = default,
            ParseMode parseMode = default,
            IEnumerable<MessageEntity> captionEntities = default,
            bool disableContentTypeDetection = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendDocumentRequest(chatId, document)
            {
                Caption = caption,
                Thumb = thumb,
                ParseMode = parseMode,
                CaptionEntities = captionEntities,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = replyMarkup,
                DisableContentTypeDetection = disableNotification
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendStickerAsync(
            ChatId chatId,
            InputOnlineFile sticker,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendStickerRequest(chatId, sticker)
            {
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendVideoAsync(
            ChatId chatId,
            InputOnlineFile video,
            int duration = default,
            int width = default,
            int height = default,
            InputMedia thumb = default,
            string caption = default,
            ParseMode parseMode = default,
            IEnumerable<MessageEntity> captionEntities = default,
            bool supportsStreaming = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendVideoRequest(chatId, video)
            {
                Duration = duration,
                Width = width,
                Height = height,
                Thumb = thumb,
                Caption = caption,
                ParseMode = parseMode,
                CaptionEntities = captionEntities,
                SupportsStreaming = supportsStreaming,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
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
            IEnumerable<MessageEntity> captionEntities = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
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
                CaptionEntities = captionEntities,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = replyMarkup,
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendVoiceAsync(
            ChatId chatId,
            InputOnlineFile voice,
            string caption = default,
            ParseMode parseMode = default,
            IEnumerable<MessageEntity> captionEntities = default,
            int duration = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendVoiceRequest(chatId, voice)
            {
                Caption = caption,
                ParseMode = parseMode,
                CaptionEntities = captionEntities,
                Duration = duration,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendVideoNoteAsync(
            ChatId chatId,
            InputTelegramFile videoNote,
            int duration = default,
            int length = default,
            InputMedia thumb = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendVideoNoteRequest(chatId, videoNote)
            {
                Duration = duration,
                Length = length,
                Thumb = thumb,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message[]> SendMediaGroupAsync(
            ChatId chatId,
            IEnumerable<IAlbumInputMedia> media,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendMediaGroupRequest(chatId, media)
            {
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendLocationAsync(
            ChatId chatId,
            float latitude,
            float longitude,
            int livePeriod = default,
            int heading = default,
            int proximityAlertRadius = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendLocationRequest(chatId, latitude, longitude)
            {
                LivePeriod = livePeriod,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = replyMarkup,
                Heading = heading,
                ProximityAlertRadius = proximityAlertRadius
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendVenueAsync(
            ChatId chatId,
            float latitude,
            float longitude,
            string title,
            string address,
            string foursquareId = default,
            string foursquareType = default,
            string googlePlaceId = default,
            string googlePlaceType = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendVenueRequest(chatId, latitude, longitude, title, address)
            {
                FoursquareId = foursquareId,
                FoursquareType = foursquareType,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = replyMarkup,
                GooglePlaceId = googlePlaceId,
                GooglePlaceType = googlePlaceType
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendContactAsync(
            ChatId chatId,
            string phoneNumber,
            string firstName,
            string lastName = default,
            string vCard = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendContactRequest(chatId, phoneNumber, firstName)
            {
                LastName = lastName,
                Vcard = vCard,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendPollAsync(
            ChatId chatId,
            string question,
            IEnumerable<string> options,
            bool? isAnonymous = default,
            PollType? type = default,
            bool? allowsMultipleAnswers = default,
            int? correctOptionId = default,
            string explanation = default,
            ParseMode explanationParseMode = default,
            IEnumerable<MessageEntity> explanationEntities = default,
            int? openPeriod = default,
            DateTime? closeDate = default,
            bool? isClosed = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendPollRequest(chatId, question, options)
            {
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = replyMarkup,
                IsAnonymous = isAnonymous,
                Type = type,
                AllowsMultipleAnswers = allowsMultipleAnswers,
                CorrectOptionId = correctOptionId,
                IsClosed = isClosed,
                OpenPeriod = openPeriod,
                CloseDate = closeDate,
                Explanation = explanation,
                ExplanationParseMode = explanationParseMode,
                ExplanationEntities = explanationEntities,
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SendDiceAsync(
            ChatId chatId,
            Emoji? emoji = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            IReplyMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(
                new SendDiceRequest(chatId)
                {
                    DisableNotification = disableNotification,
                    ReplyToMessageId = replyToMessageId,
                    AllowSendingWithoutReply = allowSendingWithoutReply,
                    ReplyMarkup = replyMarkup,
                    Emoji = emoji
                },
                cancellationToken
            );

        /// <inheritdoc />
        public Task SendChatActionAsync(
            ChatId chatId,
            ChatAction chatAction,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendChatActionRequest(chatId, chatAction), cancellationToken);

        /// <inheritdoc />
        public Task<UserProfilePhotos> GetUserProfilePhotosAsync(
            long userId,
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

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            //case file is local
            if (_localBotServer && System.IO.File.Exists(filePath))
            {
                using (var fileStream = System.IO.File.OpenRead(filePath))
                {
                    await fileStream.CopyToAsync(destination).ConfigureAwait(false);
                }

                return;
            }
            var fileUri = new Uri($"{_baseFileUrl}/{filePath}");

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
            long userId,
            DateTime untilDate = default,
            bool? revokeMessages = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new KickChatMemberRequest(chatId, userId)
            {
                UntilDate = untilDate,
                RevokeMessages = revokeMessages
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
            long userId,
            bool onlyIfBanned = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new UnbanChatMemberRequest(chatId, userId) { OnlyIfBanned = onlyIfBanned }, cancellationToken);

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
            long userId,
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
            long userId,
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
            long userId,
            bool? isAnonymous = default,
            bool? canManageChat = default,
            bool? canChangeInfo = default,
            bool? canPostMessages = default,
            bool? canEditMessages = default,
            bool? canDeleteMessages = default,
            bool? canManageVoiceChats = default,
            bool? canInviteUsers = default,
            bool? canRestrictMembers = default,
            bool? canPinMessages = default,
            bool? canPromoteMembers = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new PromoteChatMemberRequest(chatId, userId)
            {
                IsAnonymous = isAnonymous,
                CanChangeInfo = canChangeInfo,
                CanPostMessages = canPostMessages,
                CanEditMessages = canEditMessages,
                CanDeleteMessages = canDeleteMessages,
                CanInviteUsers = canInviteUsers,
                CanRestrictMembers = canRestrictMembers,
                CanPinMessages = canPinMessages,
                CanPromoteMembers = canPromoteMembers,
                CanManageChat = canManageChat,
                CanManageVoiceChat = canManageVoiceChats
            }, cancellationToken);

        /// <inheritdoc />
        public Task SetChatAdministratorCustomTitleAsync(
            ChatId chatId,
            long userId,
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
        public Task<BotCommand[]> GetMyCommandsAsync(CancellationToken cancellationToken = default) =>
            MakeRequestAsync(new GetMyCommandsRequest(), cancellationToken);

        /// <inheritdoc />
        public Task SetMyCommandsAsync(
            IEnumerable<BotCommand> commands,
            CancellationToken cancellationToken = default) =>
            MakeRequestAsync(new SetMyCommandsRequest(commands), cancellationToken);

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
            IEnumerable<MessageEntity> entities = default,
            bool disableWebPagePreview = default,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditMessageTextRequest(chatId, messageId, text)
            {
                ParseMode = parseMode,
                Entities = entities,
                DisableWebPagePreview = disableWebPagePreview,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task EditMessageTextAsync(
            string inlineMessageId,
            string text,
            ParseMode parseMode = default,
            IEnumerable<MessageEntity> entities = default,
            bool disableWebPagePreview = default,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditInlineMessageTextRequest(inlineMessageId, text)
            {
                DisableWebPagePreview = disableWebPagePreview,
                ReplyMarkup = replyMarkup,
                ParseMode = parseMode,
                Entities = entities
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> EditMessageCaptionAsync(
            ChatId chatId,
            int messageId,
            string caption,
            ParseMode parseMode = default,
            IEnumerable<MessageEntity> captionEntities = default,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditMessageCaptionRequest(chatId, messageId, caption)
            {
                ParseMode = parseMode,
                CaptionEntities = captionEntities,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task EditMessageCaptionAsync(
            string inlineMessageId,
            string caption,
            ParseMode parseMode = default,
            IEnumerable<MessageEntity> captionEntities = default,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditInlineMessageCaptionRequest(inlineMessageId, caption)
            {
                ParseMode = parseMode,
                CaptionEntities = captionEntities,
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
            float horizontalAccuracy = default,
            int heading = default,
            int proximityAlertRadius = default,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditMessageLiveLocationRequest(chatId, messageId, latitude, longitude)
            {
                ReplyMarkup = replyMarkup,
                HorizontalAccuracy = horizontalAccuracy,
                Heading = heading,
                ProximityAlertRadius = proximityAlertRadius
            }, cancellationToken);

        /// <inheritdoc />
        public Task EditMessageLiveLocationAsync(
            string inlineMessageId,
            float latitude,
            float longitude,
            float horizontalAccuracy = default,
            int heading = default,
            int proximityAlertRadius = default,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditInlineMessageLiveLocationRequest(inlineMessageId, latitude, longitude)
            {
                ReplyMarkup = replyMarkup,
                HorizontalAccuracy = horizontalAccuracy,
                Heading = heading,
                ProximityAlertRadius = proximityAlertRadius
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
            long chatId,
            string title,
            string description,
            string payload,
            string providerToken,
            string currency,
            IEnumerable<LabeledPrice> prices,
            int maxTipAmount = default,
            int[] suggestedTipAmounts = default,
            string startParameter = default,
            string providerData = default,
            string photoUrl = default,
            int photoSize = default,
            int photoWidth = default,
            int photoHeight = default,
            bool needName = default,
            bool needPhoneNumber = default,
            bool needEmail = default,
            bool needShippingAddress = default,
            bool sendPhoneNumberToProvider = default,
            bool sendEmailToProvider = default,
            bool isFlexible = default,
            bool disableNotification = default,
            int replyToMessageId = default,
            bool allowSendingWithoutReply = default,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendInvoiceRequest(
                chatId,
                title,
                description,
                payload,
                providerToken,
                currency,
                // ReSharper disable once PossibleMultipleEnumeration
                prices
            )
            {
                MaxTipAmount = maxTipAmount,
                SuggestedTipAmounts = suggestedTipAmounts,
                StartParameter = startParameter,
                ProviderData = providerData,
                PhotoUrl = photoUrl,
                PhotoSize = photoSize,
                PhotoWidth = photoWidth,
                PhotoHeight = photoHeight,
                NeedName = needName,
                NeedPhoneNumber = needPhoneNumber,
                NeedEmail = needEmail,
                NeedShippingAddress = needShippingAddress,
                SendPhoneNumberToProvider = sendPhoneNumberToProvider,
                SendEmailToProvider = sendEmailToProvider,
                IsFlexible = isFlexible,
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
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
            bool allowSendingWithoutReply = default,
            InlineKeyboardMarkup replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new SendGameRequest(chatId, gameShortName)
            {
                DisableNotification = disableNotification,
                ReplyToMessageId = replyToMessageId,
                AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = replyMarkup
            }, cancellationToken);

        /// <inheritdoc />
        public Task<Message> SetGameScoreAsync(
            long userId,
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
            long userId,
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
            long userId,
            long chatId,
            int messageId,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(
                new GetGameHighScoresRequest(userId, chatId, messageId),
                cancellationToken);

        /// <inheritdoc />
        public Task<GameHighScore[]> GetGameHighScoresAsync(
            long userId,
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
            int messageId = default,
            CancellationToken cancellationToken = default) =>
            MakeRequestAsync(new UnpinChatMessageRequest(chatId) { MessageId = messageId }, cancellationToken);

        /// <inheritdoc />
        public Task UnpinAllChatMessages(ChatId chatId, CancellationToken cancellationToken = default)
            => MakeRequestAsync(new UnpinAllChatMessagesRequest(chatId), cancellationToken);

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
        /// <inheritdoc />
        public Task<ChatInviteLink> CreateChatInviteLinkAsync(
            ChatId chatId,
            DateTime? expireDate = default,
            int? memberLimit = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new CreateChatInviteLinkRequest(chatId)
            {
                ExpireDate = expireDate,
                MemberLimit = memberLimit
            }, cancellationToken);

        /// <inheritdoc />
        public Task<ChatInviteLink> EditChatInviteLinkAsync(
            ChatId chatId,
            string inviteLink,
            DateTime? expireDate = default,
            int? memberLimit = default,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new EditChatInviteLinkRequest(chatId, inviteLink)
            {
                ExpireDate = expireDate,
                MemberLimit = memberLimit
            }, cancellationToken);

        /// <inheritdoc />
        public Task<ChatInviteLink> RevokeChatInviteLinkAsync(
            ChatId chatId,
            string inviteLink,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new RevokeChatInviteLinkRequest(chatId, inviteLink), cancellationToken);

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
            long userId,
            InputFileStream pngSticker,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new UploadStickerFileRequest(userId, pngSticker), cancellationToken);

        /// <inheritdoc />
        public Task CreateNewStickerSetAsync(
            long userId,
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
            long userId,
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
        public Task CreateNewAnimatedStickerSetAsync(
            long userId,
            string name,
            string title,
            InputFileStream tgsSticker,
            string emojis,
            bool isMasks = default,
            MaskPosition maskPosition = default,
            CancellationToken cancellationToken = default) =>
            MakeRequestAsync(
                new CreateNewAnimatedStickerSetRequest(userId, name, title, tgsSticker, emojis)
                {
                    ContainsMasks = isMasks,
                    MaskPosition = maskPosition
                },
                cancellationToken
            );

        /// <inheritdoc />
        public Task AddAnimatedStickerToSetAsync(
            long userId,
            string name,
            InputFileStream tgsSticker,
            string emojis,
            MaskPosition maskPosition = default,
            CancellationToken cancellationToken = default) =>
            MakeRequestAsync(
                new AddAnimatedStickerToSetRequest(userId, name, tgsSticker, emojis)
                {
                    MaskPosition = maskPosition
                },
                cancellationToken
            );

        /// <inheritdoc />
        public Task SetStickerPositionInSetAsync(
            string sticker,
            int position,
            CancellationToken cancellationToken = default) =>
            MakeRequestAsync(
                new SetStickerPositionInSetRequest(sticker, position),
                cancellationToken
            );

        /// <inheritdoc />
        public Task DeleteStickerFromSetAsync(
            string sticker,
            CancellationToken cancellationToken = default
        ) =>
            MakeRequestAsync(new DeleteStickerFromSetRequest(sticker), cancellationToken);

        /// <inheritdoc />
        public Task SetStickerSetThumbAsync(
            string name,
            long userId,
            InputOnlineFile thumb = default,
            CancellationToken cancellationToken = default) =>
            MakeRequestAsync(
                new SetStickerSetThumbRequest(name, userId, thumb),
                cancellationToken
            );

        #endregion

        private static HttpClient CreateHttpClient(IWebProxy webProxy)
        {
            if (webProxy is null) throw new ArgumentNullException(nameof(webProxy));
            return new HttpClient(new HttpClientHandler { Proxy = webProxy, UseProxy = true });
        }

        private static bool TrySetBaseUrl(
            string baseUrl,
            out string target)
        {
            if (baseUrl is null) throw new ArgumentNullException(nameof(baseUrl));

            if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var baseUri) ||
                string.IsNullOrEmpty(baseUri.Scheme) ||
                string.IsNullOrEmpty(baseUri.Authority))
            {
                throw new ArgumentException(
                    "Invalid format. A valid base url looks \"http://localhost:8081\" ",
                    nameof(baseUrl)
                );
            }

            if (!baseUri.Host.Equals("api.telegram.org", StringComparison.Ordinal))
            {
                target = $"{baseUri.Scheme}://{baseUri.Authority}";
                return true;
            }

            target = baseUrl;
            return false;
        }

        #region For testing purposes

        internal string BaseRequestUrl => _baseRequestUrl;
        internal string BaseFileUrl => _baseFileUrl;
        internal bool LocalBotServer => _localBotServer;

        #endregion
    }
}
