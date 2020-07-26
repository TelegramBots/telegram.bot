using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot
{
    /// <summary>
    /// Extension methods that map to requests from Bot API documentation
    /// </summary>
    public static class TelegramBotClientExtensions
    {
        #region Getting updates

        /// <summary>
        /// Use this method to receive incoming updates using long polling (wiki).
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="offset">
        /// Identifier of the first <see cref="Update"/> to be returned.
        /// Must be greater by one than the highest among the identifiers of previously received
        /// updates. By default, updates starting with the earliest unconfirmed update are
        /// returned. An update is considered confirmed as soon as <see cref="GetUpdatesAsync"/>
        /// is called with an offset higher than its <see cref="Update.Id"/>. The negative offset
        /// can be specified to retrieve updates starting from -offset update from the end of
        /// the updates queue. All previous updates will forgotten.
        /// </param>
        /// <param name="limit">
        /// Limits the number of updates to be retrieved. Values between 1-100 are accepted.
        /// </param>
        /// <param name="timeout">
        /// Timeout in seconds for long polling. Defaults to 0, i.e. usual short polling. Should
        /// be positive, short polling should be used for testing purposes only.
        /// </param>
        /// <param name="allowedUpdates">
        /// List the <see cref="UpdateType"/> of updates you want your bot to receive.
        /// See <see cref="UpdateType"/> for a complete list of available update types. Specify an
        /// empty list to receive all updates regardless of type (default). If not specified,
        /// the previous setting will be used.
        /// <para />
        /// Please note that this parameter doesn't affect updates created before the call to the
        /// <see cref="GetUpdatesAsync"/>, so unwanted updates may be received for a short period
        /// of time.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// This method will not work if an outgoing webhook is set up.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// In order to avoid getting duplicate updates, recalculate offset after each server
        /// response.
        /// </description>
        /// </item>
        /// </list>
        /// Telegram Docs <see href="https://core.telegram.org/bots/api#getupdates"/>
        /// </remarks>
        /// <returns>An Array of <see cref="Update"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#getupdates"/>
        public static async Task<Update[]> GetUpdatesAsync(
            this ITelegramBotClient botClient,
            int? offset = default,
            int? limit = default,
            int? timeout = default,
            IEnumerable<UpdateType>? allowedUpdates = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new GetUpdatesRequest
                    {
                        Offset = offset,
                        Limit = limit,
                        Timeout = timeout,
                        AllowedUpdates = allowedUpdates
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to specify a url and receive incoming updates via an outgoing webhook.
        /// Whenever there is an <see cref="Update"/> for the bot, we will send an HTTPS POST
        /// request to the specified url, containing a JSON-serialized Update. In case of an
        /// unsuccessful request, we will give up after a reasonable amount of attempts.
        /// <para />
        /// If you'd like to make sure that the Webhook request comes from Telegram, we recommend
        /// using a secret path in the URL, e.g. https://www.example.com/&lt;token&gt;. Since
        /// nobody else knows your bot's token, you can be pretty sure it's us.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="url">
        /// HTTPS url to send updates to. Use an empty string to remove webhook integration
        /// </param>
        /// <param name="certificate">
        /// Upload your public key certificate so that the root certificate in use can be checked.
        /// See the <see href="https://core.telegram.org/bots/self-signed">self-signed guide</see>
        /// for details.
        /// </param>
        /// <param name="maxConnections">
        /// Maximum allowed number of simultaneous HTTPS connections to the webhook for update
        /// delivery, 1-100. Defaults to 40. Use lower values to limit the load on your bot's
        /// server, and higher values to increase your bot's throughput.</param>
        /// <param name="allowedUpdates">
        /// List the <see cref="UpdateType"/> of updates you want your bot to receive. See
        /// <see cref="UpdateType"/> for a complete list of available update types. Specify an
        /// empty list to receive all updates regardless of type (default). If not specified, the
        /// previous setting will be used.
        /// <para />
        /// Please note that this parameter doesn't affect updates created before the call
        /// to the <see cref="GetUpdatesAsync"/>, so unwanted updates may be received for a short
        /// period of time.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns><c>true</c></returns>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// You will not be able to receive updates using getUpdates for as long as an outgoing
        /// webhook is set up.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// We currently do not support self-signed certificates.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// For the moment, the only supported port for Webhooks is 443. We may support additional
        /// ports later.
        /// </description>
        /// </item>
        /// </list>
        /// If you're having any trouble setting up webhooks, please check out this
        /// <see href="https://core.telegram.org/bots/webhooks">amazing guide to Webhooks</see>.
        /// </remarks>
        /// <see href="https://core.telegram.org/bots/api#setwebhook"/>
        public static async Task SetWebhookAsync(
            this ITelegramBotClient botClient,
            string url,
            InputFileStream? certificate = default,
            int? maxConnections = default,
            IEnumerable<UpdateType>? allowedUpdates = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SetWebhookRequest(url, certificate)
                    {
                        MaxConnections = maxConnections,
                        AllowedUpdates = allowedUpdates
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to remove webhook integration if you decide to switch back to
        /// getUpdates.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>Returns <c>true</c> on success</returns>
        /// <see href="https://core.telegram.org/bots/api#deletewebhook"/>
        public static async Task DeleteWebhookAsync(
            this ITelegramBotClient botClient,
            CancellationToken cancellationToken = default)
            => await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(new DeleteWebhookRequest(), cancellationToken);

        /// <summary>
        /// Use this method to get current webhook status.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>On success, returns <see cref="WebhookInfo"/>.</returns>
        /// <see href="https://core.telegram.org/bots/api#getwebhookinfo"/>
        public static async Task<WebhookInfo> GetWebhookInfoAsync(
            this ITelegramBotClient botClient,
            CancellationToken cancellationToken = default)
            => await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(new GetWebhookInfoRequest(), cancellationToken);

        #endregion Getting updates

        #region Available methods

        /// <summary>
        /// A simple method for testing your bot's auth token.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>
        /// Returns basic information about the bot in form of <see cref="User"/> object
        /// </returns>
        /// <see href="https://core.telegram.org/bots/api#getme"/>
        public static async Task<User> GetMeAsync(
            this ITelegramBotClient botClient,
            CancellationToken cancellationToken = default)
            => await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(new GetMeRequest(), cancellationToken);

        /// <summary>
        /// Use this method to send text messages. On success, the sent
        /// <see cref="Message"/> is returned.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="text">Text of the message to be sent</param>
        /// <param name="parseMode">
        /// Change, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs
        /// in your bot's message.
        /// </param>
        /// <param name="disableWebPagePreview">
        /// Disables link previews for links in this message
        /// </param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendmessage"/>
        public static async Task<Message> SendTextMessageAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            string text,
            ParseMode? parseMode = default,
            bool? disableWebPagePreview = default,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendMessageRequest(chatId, text)
                    {
                        ParseMode = parseMode,
                        DisableWebPagePreview = disableWebPagePreview,
                        DisableNotification = disableNotification,
                        ReplyToMessageId = replyToMessageId,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to forward messages of any kind. On success, the sent
        /// <see cref="Message"/> is returned.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="fromChatId">
        /// <see cref="ChatId"/> for the chat where the original message was sent
        /// </param>
        /// <param name="messageId">Unique message identifier</param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#forwardmessage"/>
        public static async Task<Message> ForwardMessageAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            ChatId fromChatId,
            int messageId,
            bool disableNotification = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new ForwardMessageRequest(chatId, fromChatId, messageId)
                    {
                        DisableNotification = disableNotification
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to send photos. On success, the sent <see cref="Message"/> is returned.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="photo">Photo to send.</param>
        /// <param name="caption">
        /// Photo caption (may also be used when resending photos by file_id).
        /// </param>
        /// <param name="parseMode">
        /// Change, if you want Telegram apps to show bold, italic, fixed-width text or inline
        /// URLs in your bot's message.
        /// </param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendphoto"/>
        public static async Task<Message> SendPhotoAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            InputOnlineFile photo,
            string? caption = default,
            ParseMode? parseMode = default,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendPhotoRequest(chatId, photo)
                    {
                        Caption = caption,
                        ParseMode = parseMode,
                        ReplyToMessageId = replyToMessageId,
                        DisableNotification = disableNotification,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to send audio files, if you want Telegram clients to display them in
        /// the music player. Your audio must be in the .mp3 format. On success, the sent
        /// <see cref="Message"/> is returned. Bots can currently send audio files of up to 50MB
        /// in size, this limit may be changed in the future.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="audio">Audio file to send.</param>
        /// <param name="caption">Audio caption, 0-1024 characters</param>
        /// <param name="parseMode">
        /// Change, if you want Telegram apps to show bold, italic, fixed-width text or inline
        /// URLs in your bot's message.
        /// </param>
        /// <param name="duration">Duration of the audio in seconds</param>
        /// <param name="performer">Performer</param>
        /// <param name="title">Track name</param>
        /// <param name="thumb">
        /// Thumbnail of the file sent. The thumbnail should be in JPEG format and less than 200KB
        /// in size. A thumbnail's width and height should not exceed 90. Thumbnails can't be
        /// reused and can be only uploaded as a new file.
        /// </param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification,
        /// Android users will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendaudio"/>
        public static async Task<Message> SendAudioAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            InputOnlineFile audio,
            string? caption = default,
            ParseMode? parseMode = default,
            int? duration = default,
            string? performer = default,
            string? title = default,
            InputMedia? thumb = default,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendAudioRequest(chatId, audio)
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
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to send general files. On success, the sent <see cref="Message"/> is
        /// returned. Bots can send files of any type of up to 50MB in size.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="document">File to send.</param>
        /// <param name="thumb">
        /// Thumbnail of the file sent. The thumbnail should be in JPEG format and less than 200KB
        /// in size. A thumbnail's width and height should not exceed 90. Thumbnails can't be
        /// reused and can be only uploaded as a new file.
        /// </param>
        /// <param name="caption">Document caption</param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <param name="parseMode">
        /// Change, if you want Telegram apps to show bold, italic, fixed-width text or inline
        /// URLs in your bot's message.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#senddocument"/>
        public static async Task<Message> SendDocumentAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            InputOnlineFile document,
            InputMedia? thumb = default,
            string? caption = default,
            ParseMode? parseMode = default,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendDocumentRequest(chatId, document)
                    {
                        Caption = caption,
                        Thumb = thumb,
                        ParseMode = parseMode,
                        DisableNotification = disableNotification,
                        ReplyToMessageId = replyToMessageId,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to send .webp stickers. On success, the sent <see cref="Message"/>
        /// is returned.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="sticker">Sticker to send.</param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a custom reply keyboard, instructions to hide keyboard or
        /// to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendsticker"/>
        public static async Task<Message> SendStickerAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            InputOnlineFile sticker,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendStickerRequest(chatId, sticker)
                    {
                        DisableNotification = disableNotification,
                        ReplyToMessageId = replyToMessageId,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to send video files, Telegram clients support mp4 videos (other
        /// formats may be sent as <see cref="Document"/>). On success, the sent
        /// <see cref="Message"/> is returned. Bots can send video files of up to 50MB in size.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="video">Video to send.</param>
        /// <param name="duration">Duration of sent video in seconds</param>
        /// <param name="width">Video width</param>
        /// <param name="height">Video height</param>
        /// <param name="thumb">
        /// Thumbnail of the file sent. The thumbnail should be in JPEG format and less than 200KB
        /// in size. A thumbnail's width and height should not exceed 90. Thumbnails can't be
        /// reused and can be only uploaded as a new file.
        /// </param>
        /// <param name="caption">Video caption</param>
        /// <param name="parseMode">
        /// Change, if you want Telegram apps to show bold, italic, fixed-width text or inline
        /// URLs in your bot's message.
        /// </param>
        /// <param name="supportsStreaming">
        /// Pass <c>true</c>, if the uploaded video is suitable for streaming
        /// </param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendvideo"/>
        public static async Task<Message> SendVideoAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            InputOnlineFile video,
            int? duration = default,
            int? width = default,
            int? height = default,
            InputMedia? thumb = default,
            string? caption = default,
            ParseMode? parseMode = default,
            bool? supportsStreaming = default,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendVideoRequest(chatId, video)
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
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to send animation files (GIF or H.264/MPEG-4 AVC video without sound).
        /// On success, the sent <see cref="Message"/> is returned. Bots can currently send
        /// animation files of up to 50MB in size, this limit may be changed in the future.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="animation">Animation to send</param>
        /// <param name="duration">Duration of sent animation in seconds</param>
        /// <param name="width">Animation width</param>
        /// <param name="height">Animation height</param>
        /// <param name="thumb">
        /// Thumbnail of the file sent. The thumbnail should be in JPEG format and less than 200KB
        /// in size. A thumbnail's width and height should not exceed 90. Thumbnails can't be
        /// reused and can be only uploaded as a new file.
        /// </param>
        /// <param name="caption">Animation caption</param>
        /// <param name="parseMode">
        /// Change, if you want Telegram apps to show bold, italic, fixed-width text or inline
        /// URLs in your bot's message.
        /// </param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive  notice of
        /// cancellation.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendanimation"/>
        public static async Task<Message> SendAnimationAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            InputOnlineFile animation,
            int? duration = default,
            int? width = default,
            int? height = default,
            InputMedia? thumb = default,
            string? caption = default,
            ParseMode? parseMode = default,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendAnimationRequest(chatId, animation)
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
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to send audio files, if you want Telegram clients to display the file
        /// as a playable voice message. For this to work, your audio must be in an .ogg file
        /// encoded with OPUS (other formats may be sent as <see cref="Audio"/> or
        /// <see cref="Document"/>). On success, the sent <see cref="Message"/> is returned.
        /// Bots can currently send voice messages of up to 50MB in size, this limit may be
        /// changed in the future.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="voice">Audio file to send.</param>
        /// <param name="caption">Voice message caption, 0-1024 characters</param>
        /// <param name="parseMode">
        /// Change, if you want Telegram apps to show bold, italic, fixed-width text or inline
        /// URLs in your bot's message.
        /// </param>
        /// <param name="duration">Duration of sent audio in seconds</param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendvoice"/>
        public static async Task<Message> SendVoiceAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            InputOnlineFile voice,
            string? caption = default,
            ParseMode? parseMode = default,
            int? duration = default,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendVoiceRequest(chatId, voice)
                    {
                        Caption = caption,
                        ParseMode = parseMode,
                        Duration = duration,
                        DisableNotification = disableNotification,
                        ReplyToMessageId = replyToMessageId,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// As of v.4.0, Telegram clients support rounded square mp4 videos of up to 1 minute
        /// long. Use this method to send video messages.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="videoNote">Video note to send.</param>
        /// <param name="thumb">
        /// Thumbnail of the file sent. The thumbnail should be in JPEG format and less than 200KB
        /// in size. A thumbnail's width and height should not exceed 90. Thumbnails can't be
        /// reused and can be only uploaded as a new file.
        /// </param>
        /// <param name="duration">Duration of sent video in seconds</param>
        /// <param name="length">Video width and height</param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendvideonote"/>
        public static async Task<Message> SendVideoNoteAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            InputTelegramFile videoNote,
            int? duration = default,
            int? length = default,
            InputMedia? thumb = default,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendVideoNoteRequest(chatId, videoNote)
                    {
                        Duration = duration,
                        Length = length,
                        Thumb = thumb,
                        DisableNotification = disableNotification,
                        ReplyToMessageId = replyToMessageId,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

            /// <summary>
            /// Use this method to send a group of photos or videos as an album. On success,
            /// an array of the sent <see cref="Message"/>s is returned.
            /// </summary>
            /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
            /// <param name="chatId">
            /// Unique identifier for the target chat or username of the target channel
            /// (in the format @channelusername)
            /// </param>
            /// <param name="media">
            /// A JSON-serialized array describing photos and videos to be sent, must include
            /// 2–10 items
            /// </param>
            /// <param name="disableNotification">
            /// Sends the messages silently. Users will receive a notification with no sound.
            /// </param>
            /// <param name="replyToMessageId">
            /// If the message is a reply, ID of the original message
            /// </param>
            /// <param name="cancellationToken">
            /// A cancellation token that can be used by other objects or threads to receive
            /// notice of cancellation.
            /// </param>
            /// <returns>
            /// On success, an array of the sent <see cref="Message"/>s is returned.
            /// </returns>
            /// <see href="https://core.telegram.org/bots/api#sendmediagroup"/>
            public static async Task<Message[]> SendMediaGroupAsync(
                this ITelegramBotClient botClient,
                ChatId chatId,
                IEnumerable<IAlbumInputMedia> media,
                bool? disableNotification = default,
                int? replyToMessageId = default,
                CancellationToken cancellationToken = default
            ) =>
                await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                    .MakeRequestAsync(
                        new SendMediaGroupRequest(chatId, media)
                         {
                            DisableNotification = disableNotification,
                            ReplyToMessageId = replyToMessageId,
                        },
                        cancellationToken
                    );

            /// <summary>
            /// Use this method to send point on the map. On success, the sent
            /// <see cref="Message"/> is returned.
            /// </summary>
            /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
            /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
            /// <param name="latitude">Latitude of location</param>
            /// <param name="longitude">Longitude of location</param>
            /// <param name="livePeriod">
            /// Period in seconds for which the location will be updated. Should be between
            /// 60 and 86400.
            /// </param>
            /// <param name="disableNotification">
            /// Sends the message silently. iOS users will not receive a notification, Android
            /// users will receive a notification with no sound.
            /// </param>
            /// <param name="replyToMessageId">
            /// If the message is a reply, ID of the original message
            /// </param>
            /// <param name="replyMarkup">
            /// A JSON-serialized object for a custom reply keyboard,
            /// instructions to hide keyboard or to force a reply from the user.
            /// </param>
            /// <param name="cancellationToken">
            /// A cancellation token that can be used by other objects or threads to receive
            /// notice of cancellation.
            /// </param>
            /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
            /// <see href="https://core.telegram.org/bots/api#sendlocation"/>
            public static async Task<Message> SendLocationAsync(
                this ITelegramBotClient botClient,
                ChatId chatId,
                double latitude,
                double longitude,
                int? livePeriod = default,
                bool? disableNotification = default,
                int? replyToMessageId = default,
                IReplyMarkup? replyMarkup = default,
                CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendLocationRequest(chatId, latitude, longitude)
                    {
                        LivePeriod = livePeriod,
                        DisableNotification = disableNotification,
                        ReplyToMessageId = replyToMessageId,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to send information about a venue.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="latitude">Latitude of the venue</param>
        /// <param name="longitude">Longitude of the venue</param>
        /// <param name="title">Name of the venue</param>
        /// <param name="address">Address of the venue</param>
        /// <param name="foursquareId">Foursquare identifier of the venue</param>
        /// <param name="foursquareType">
        /// Foursquare type of the venue, if known. (For example, "arts_entertainment/default",
        /// "arts_entertainment/aquarium" or "food/icecream".)
        /// </param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of
        /// cancellation.</param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendvenue"/>
        public static async Task<Message> SendVenueAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            double latitude,
            double longitude,
            string title,
            string address,
            string? foursquareId = default,
            string? foursquareType = default,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendVenueRequest(chatId, latitude, longitude, title, address)
                    {
                        FoursquareId = foursquareId,
                        FoursquareType = foursquareType,
                        DisableNotification = disableNotification,
                        ReplyToMessageId = replyToMessageId,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to send phone contacts.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="phoneNumber">Contact's phone number</param>
        /// <param name="firstName">Contact's first name</param>
        /// <param name="lastName">Contact's last name</param>
        /// <param name="vCard">
        /// Additional data about the contact in the form of a vCard, 0-2048 bytes
        /// </param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for an inline keyboard, custom reply keyboard, instructions
        /// to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendcontact"/>
        public static async Task<Message> SendContactAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            string phoneNumber,
            string firstName,
            string? lastName = default,
            string? vCard = default,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendContactRequest(chatId, phoneNumber, firstName)
                    {
                        LastName = lastName,
                        Vcard = vCard,
                        DisableNotification = disableNotification,
                        ReplyToMessageId = replyToMessageId,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to send a native poll. A native poll can't be sent to a private chat.
        /// On success, the sent <see cref="Message"/> is returned.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="question">Poll question, 1-255 characters</param>
        /// <param name="options">
        /// List of answer options, 2-10 strings 1-100 characters each
        /// </param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for an inline keyboard, custom reply keyboard, instructions
        /// to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <param name="isAnonymous">
        /// <c>true</c>, if the poll needs to be anonymous, defaults to <c>true</c>
        /// </param>
        /// <param name="type">
        /// Poll type, <see cref="PollType.Quiz"/> or <see cref="PollType.Regular"/>, defaults
        /// to <see cref="PollType.Regular"/>
        /// </param>
        /// <param name="allowsMultipleAnswers">
        /// <c>true</c>, if the poll allows multiple answers, ignored for polls in quiz mode,
        /// defaults to <c>false</c>
        /// </param>
        /// <param name="correctOptionId">
        /// 0-based identifier of the correct answer option, required for polls in quiz mode
        /// </param>
        /// <param name="isClosed">
        /// Pass <c>true</c>, if the poll needs to be immediately closed
        /// </param>
        /// <param name="explanation">
        /// Text that is shown when a user chooses an incorrect answer or taps on the lamp icon
        /// in a quiz-style poll
        /// </param>
        /// <param name="explanationParseMode">
        /// Mode for parsing entities in the explanation
        /// </param>
        /// <param name="openPeriod">
        /// Amount of time in seconds the poll will be active after creation
        /// </param>
        /// <param name="closeDate">
        /// Point in time when the poll will be automatically closed
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendpoll"/>
        public static async Task<Message> SendPollAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            string question,
            IEnumerable<string> options,
            bool? isAnonymous = default,
            PollType? type = default,
            bool? allowsMultipleAnswers = default,
            int? correctOptionId = default,
            bool? isClosed = default,
            string? explanation = default,
            ParseMode? explanationParseMode = default,
            int? openPeriod = default,
            DateTime? closeDate = default,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendPollRequest(chatId, question, options)
                    {
                        DisableNotification = disableNotification,
                        ReplyToMessageId = replyToMessageId,
                        ReplyMarkup = replyMarkup,
                        IsAnonymous = isAnonymous,
                        Type = type,
                        AllowsMultipleAnswers = allowsMultipleAnswers,
                        CorrectOptionId = correctOptionId,
                        IsClosed = isClosed,
                        OpenPeriod = openPeriod,
                        CloseDate = closeDate,
                        Explanation = explanation,
                        ExplanationParseMode = explanationParseMode
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this request to send a dice, which will have a random value from 1 to 6. On
        /// success, the sent <see cref="Message"/> is returned
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for an inline keyboard, custom reply keyboard, instructions
        /// to hide keyboard or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <param name="emoji">Emoji on which the dice throw animation is based</param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#senddice"/>
        public static async Task<Message> SendDiceAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            IReplyMarkup? replyMarkup = default,
            Emoji? emoji = default,
            CancellationToken cancellationToken = default) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendDiceRequest(chatId)
                    {
                        DisableNotification = disableNotification,
                        ReplyToMessageId = replyToMessageId,
                        ReplyMarkup = replyMarkup,
                        Emoji = emoji
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method when you need to tell the user that something is happening on the
        /// bot's side. The status is set for 5 seconds or less (when a message arrives from your
        /// bot, Telegram clients clear its typing status).
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="chatAction">
        /// Type of action to broadcast. Choose one, depending on what the user is about to
        /// receive.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <remarks>
        /// We only recommend using this method when a response from the bot will take a
        /// noticeable amount of time to arrive.
        /// </remarks>
        /// <see href="https://core.telegram.org/bots/api#sendchataction"/>
        public static async Task SendChatActionAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            ChatAction chatAction,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendChatActionRequest(chatId, chatAction),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to get a list of profile pictures for a user.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="userId">Unique identifier of the target user</param>
        /// <param name="offset">
        /// Sequential number of the first photo to be returned. By default, all photos are
        /// returned.
        /// </param>
        /// <param name="limit">
        /// Limits the number of photos to be retrieved. Values between 1-100 are accepted.
        /// Defaults to 100.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns a <see cref="UserProfilePhotos"/> object</returns>
        /// <see href="https://core.telegram.org/bots/api#getuserprofilephotos"/>
        public static async Task<UserProfilePhotos> GetUserProfilePhotosAsync(
            this ITelegramBotClient botClient,
            int userId,
            int? offset = default,
            int? limit = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new GetUserProfilePhotosRequest(userId)
                    {
                        Offset = offset,
                        Limit = limit
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to get information about a file. For the moment, bots can download
        /// files of up to 20MB in size.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="fileId">File identifier</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>The <see cref="File"/> object</returns>
        /// <see href="https://core.telegram.org/bots/api#getfile"/>
        public static async Task<File> GetFileAsync(
            this ITelegramBotClient botClient,
            string fileId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(new GetFileRequest(fileId), cancellationToken);

        /// <summary>
        /// Use this method to kick a user from a group or a supergroup. In the case of
        /// supergroups, the user will not be able to return to the group on their own using
        /// invite links, etc., unless unbanned first. The bot must be an administrator in the
        /// group for this to work.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target group</param>
        /// <param name="userId">Unique identifier of the target user</param>
        /// <param name="untilDate">
        /// <see cref="DateTime"/> when the user will be unbanned. If user is banned for more
        /// than 366 days or less than 30 seconds from the current time they are considered to
        /// be banned forever
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#kickchatmember"/>
        public static async Task KickChatMemberAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int userId,
            DateTime? untilDate = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new KickChatMemberRequest(chatId, userId)
                    {
                        UntilDate = untilDate
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method for your bot to leave a group, supergroup or channel.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns a Chat object on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#leavechat"/>
        public static async Task LeaveChatAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(new LeaveChatRequest(chatId), cancellationToken);

        /// <summary>
        /// Use this method to unban a previously kicked user in a supergroup. The user will not
        /// return to the group automatically, but will be able to join via link, etc. The bot
        /// must be an administrator in the group for this to work.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target group</param>
        /// <param name="userId">Unique identifier of the target user</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#unbanchatmember"/>
        public static async Task UnbanChatMemberAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int userId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new UnbanChatMemberRequest(chatId, userId),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to get up to date information about the chat (current name of the user
        /// for one-on-one conversations, current username of a user, group or channel, etc.).
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns a Chat object on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#getchat"/>
        public static async Task<Chat> GetChatAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(new GetChatRequest(chatId), cancellationToken);

        /// <summary>
        /// Use this method to get a list of administrators in a chat.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, returns an Array of <see cref="ChatMember"/> objects that
        /// contains information about all chat administrators except other bots. If the chat is
        /// a group or a supergroup and no administrators were appointed, only the creator
        /// will be returned.
        /// </returns>
        /// <see href="https://core.telegram.org/bots/api#getchatadministrators"/>
        public static async Task<ChatMember[]> GetChatAdministratorsAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new GetChatAdministratorsRequest(chatId),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to get the number of members in a chat.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>The number of members in a chat.</returns>
        /// <see href="https://core.telegram.org/bots/api#getchatmemberscount"/>
        public static async Task<int> GetChatMembersCountAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient))).MakeRequestAsync(
                new GetChatMembersCountRequest(chatId),
                cancellationToken
            );

        /// <summary>
        /// Use this method to get information about a member of a chat.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="userId">Unique identifier of the target user</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>A <see cref="ChatMember"/> object on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#getchatmember"/>
        public static async Task<ChatMember> GetChatMemberAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int userId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new GetChatMemberRequest(chatId, userId),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to send answers to callback queries sent from inline keyboards. The
        /// answer will be displayed to the user as a notification at the top of the chat screen
        /// or as an alert.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="callbackQueryId">Unique identifier for the query to be answered</param>
        /// <param name="text">
        /// Text of the notification. If not specified, nothing will be shown to the user
        /// </param>
        /// <param name="showAlert">
        /// If <c>true</c>, an alert will be shown by the client instead of a notification at the
        /// top of the chat screen. Defaults to <c>true</c>.
        /// </param>
        /// <param name="url">
        /// URL that will be opened by the user's client. If you have created a Game and accepted
        /// the conditions via @Botfather, specify the URL that opens your game — note that this
        /// will only work if the query comes from a callback_game button.
        /// <para />
        /// Otherwise, you may use links like telegram.me/your_bot? start = XXXX that open your
        /// bot with a parameter.
        /// </param>
        /// <param name="cacheTime">
        /// The maximum amount of time in seconds that the result of the callback query may be
        /// cached client-side. Telegram apps will support caching starting in version 3.14.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, <c>true</c> is returned.</returns>
        /// <remarks>
        /// Alternatively, the user can be redirected to the specified Game URL. For this option
        /// to work, you must first create a game for your bot via BotFather and accept the terms.
        /// <para />
        /// Otherwise, you may use links like telegram.me/your_bot?start=XXXX that open your bot
        /// with a parameter.
        /// </remarks>
        /// <see href="https://core.telegram.org/bots/api#answercallbackquery"/>
        public static async Task AnswerCallbackQueryAsync(
            this ITelegramBotClient botClient,
            string callbackQueryId,
            string? text = default,
            bool? showAlert = default,
            string? url = default,
            int? cacheTime = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new AnswerCallbackQueryRequest(callbackQueryId)
                    {
                        Text = text,
                        ShowAlert = showAlert,
                        Url = url,
                        CacheTime = cacheTime
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to restrict a user in a supergroup. The bot must be an administrator
        /// in the supergroup for this to work and must have the appropriate admin rights.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target supergroup
        /// </param>
        /// <param name="userId">Unique identifier of the target user</param>
        /// <param name="permissions">New user permissions</param>
        /// <param name="untilDate">
        /// <see cref="DateTime"/> when restrictions will be lifted for the user. If user is
        /// restricted for more than 366 days or less than 30 seconds from the current time,
        /// they are considered to be restricted forever
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, <c>true</c> is returned</returns>
        /// <remarks>
        /// Pass <c>true</c> for all boolean parameters to lift restrictions from a user.
        /// </remarks>
        /// <see href="https://core.telegram.org/bots/api#restrictchatmember"/>
        public static async Task RestrictChatMemberAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int userId,
            ChatPermissions permissions,
            DateTime? untilDate = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new RestrictChatMemberRequest(chatId, userId, permissions)
                    {
                        UntilDate = untilDate
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to promote or demote a user in a supergroup or a channel. The bot
        /// must be an administrator in the chat for this to work and must have the appropriate
        /// admin rights.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="userId">Unique identifier of the target user</param>
        /// <param name="canChangeInfo">
        /// Pass <c>true</c>, if the administrator can change chat title, photo and other settings
        /// </param>
        /// <param name="canPostMessages">
        /// Pass <c>true</c>, if the administrator can create channel posts, channels only
        /// </param>
        /// <param name="canEditMessages">
        /// Pass <c>true</c>, if the administrator can edit messages of other users, channels only
        /// </param>
        /// <param name="canDeleteMessages">
        /// Pass <c>true</c>, if the administrator can delete messages of other users
        /// </param>
        /// <param name="canInviteUsers">
        /// Pass <c>true</c>, if the administrator can invite new users to the chat
        /// </param>
        /// <param name="canRestrictMembers">
        /// Pass <c>true</c>, if the administrator can restrict, ban or unban chat members
        /// </param>
        /// <param name="canPinMessages">
        /// Pass <c>true</c>, if the administrator can pin messages, supergroups only
        /// </param>
        /// <param name="canPromoteMembers">
        /// Pass <c>true</c>, if the administrator can add new administrators with a subset of
        /// his own privileges or demote administrators that he has promoted, directly or
        /// indirectly (promoted by administrators that were appointed by him)
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success.</returns>
        /// <remarks>Pass <c>false</c> for all boolean parameters to demote a user.</remarks>
        /// <see href="https://core.telegram.org/bots/api#promotechatmember"/>
        public static async Task PromoteChatMemberAsync(
            this ITelegramBotClient botClient,
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
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new PromoteChatMemberRequest(chatId, userId)
                    {
                        CanChangeInfo = canChangeInfo,
                        CanPostMessages = canPostMessages,
                        CanEditMessages = canEditMessages,
                        CanDeleteMessages = canDeleteMessages,
                        CanInviteUsers = canInviteUsers,
                        CanRestrictMembers = canRestrictMembers,
                        CanPinMessages = canPinMessages,
                        CanPromoteMembers = canPromoteMembers
                    },
                    cancellationToken
                );

        /// <summary>
        /// <inheritdoc cref="Telegram.Bot.Requests.SetChatAdministratorCustomTitleRequest"/>
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="userId">Unique identifier of the target user</param>
        /// <param name="customTitle">
        /// New custom title for the administrator; 0-16 characters, emoji are not allowed
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#setchatadministratorcustomtitle"/>
        public static async Task SetChatAdministratorCustomTitleAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int userId,
            string customTitle,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SetChatAdministratorCustomTitleRequest(chatId, userId, customTitle),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to set default chat permissions for all members. The bot must be an
        /// administrator in the group or a supergroup for this to work and must have the
        /// <see cref="ChatMember.CanRestrictMembers"/> admin rights. Returns <c>true</c>
        /// on success.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="permissions">New default permissions</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#setchatpermissions"/>
        public static async Task SetChatPermissionsAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            ChatPermissions permissions,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SetChatPermissionsRequest(chatId, permissions),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to get the current list of the bot's commands
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Array of <see cref="BotCommand"/> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#getmycommands"/>
        public static async Task<BotCommand[]> GetMyCommandsAsync(
            this ITelegramBotClient botClient,
            CancellationToken cancellationToken = default) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(new GetMyCommandsRequest(), cancellationToken);

        /// <summary>
        /// Use this method to change the list of the bot's commands. Returns True on success.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="commands">A list of bot commands to be set</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#setmycommands"/>
        public static async Task SetMyCommandsAsync(
            this ITelegramBotClient botClient,
            IEnumerable<BotCommand> commands,
            CancellationToken cancellationToken = default) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SetMyCommandsRequest(commands),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to stop updating a live location message sent by the bot before
        /// <see cref="SendLocationRequest.LivePeriod"/> expires.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="messageId">Unique identifier of the sent message</param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#stopmessagelivelocation"/>
        public static async Task<Message> StopMessageLiveLocationAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int messageId,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new StopMessageLiveLocationRequest(chatId, messageId)
                    {
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to stop updating a live location message sent via the bot
        /// (for inline bots) before live_period expires.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="inlineMessageId">Identifier of the inline message</param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#stopmessagelivelocation"/>
        public static async Task StopMessageLiveLocationAsync(
            this ITelegramBotClient botClient,
            string inlineMessageId,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new StopInlineMessageLiveLocationRequest(inlineMessageId)
                    {
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        #endregion Available methods

        #region Updating messages

        /// <summary>
        /// Use this method to edit text messages sent by the bot or via the bot (for inline bots).
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="messageId">Unique identifier of the sent message</param>
        /// <param name="text">New text of the message</param>
        /// <param name="parseMode">
        /// Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width
        /// text or inline URLs in your bot's message.
        /// </param>
        /// <param name="disableWebPagePreview">
        /// Disables link previews for links in this message
        /// </param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, the edited <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#editmessagetext"/>
        public static async Task<Message> EditMessageTextAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int messageId,
            string text,
            ParseMode? parseMode = default,
            bool? disableWebPagePreview = default,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new EditMessageTextRequest(chatId, messageId, text)
                    {
                        ParseMode = parseMode,
                        DisableWebPagePreview = disableWebPagePreview,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to edit text messages sent by the bot or via the bot (for inline bots).
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="inlineMessageId">Identifier of the inline message</param>
        /// <param name="text">New text of the message</param>
        /// <param name="parseMode">
        /// Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width
        /// text or inline URLs in your bot's message.
        /// </param>
        /// <param name="disableWebPagePreview">
        /// Disables link previews for links in this message
        /// </param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#editmessagetext"/>
        public static async Task EditMessageTextAsync(
            this ITelegramBotClient botClient,
            string inlineMessageId,
            string text,
            ParseMode? parseMode = default,
            bool? disableWebPagePreview = default,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new EditInlineMessageTextRequest(inlineMessageId, text)
                    {
                        DisableWebPagePreview = disableWebPagePreview,
                        ReplyMarkup = replyMarkup,
                        ParseMode = parseMode
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to edit captions of messages sent by the bot or via the bot
        /// (for inline bots).
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="messageId">Unique identifier of the sent message</param>
        /// <param name="caption">New caption of the message</param>
        /// <param name="parseMode">
        /// Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width
        /// text or inline URLs in the media caption.
        /// </param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, the edited <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#editmessagecaption"/>
        public static async Task<Message> EditMessageCaptionAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int messageId,
            string caption,
            ParseMode? parseMode = default,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new EditMessageCaptionRequest(chatId, messageId, caption)
                    {
                        ParseMode = parseMode,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to edit captions of messages sent by the bot or via the bot
        /// (for inline bots).
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="inlineMessageId">Unique identifier of the sent message</param>
        /// <param name="caption">New caption of the message</param>
        /// <param name="parseMode">
        /// Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width
        /// text or inline URLs in the media caption.
        /// </param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, the edited <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#editmessagecaption"/>
        public static async Task EditMessageCaptionAsync(
            this ITelegramBotClient botClient,
            string inlineMessageId,
            string caption,
            ParseMode? parseMode = default,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new EditInlineMessageCaptionRequest(inlineMessageId, caption)
                    {
                        ParseMode = parseMode,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to edit audio, document, photo, or video messages.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="messageId">Unique identifier of the sent message</param>
        /// <param name="media">A new media content of the message</param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success the edited <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#editmessagemedia"/>
        public static async Task<Message> EditMessageMediaAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int messageId,
            InputMediaBase media,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new EditMessageMediaRequest(chatId, messageId, media)
                    {
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to edit audio, document, photo, or video inline messages.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="inlineMessageId">Unique identifier of the sent message</param>
        /// <param name="media">A new media content of the message</param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#editmessagemedia"/>
        public static async Task EditMessageMediaAsync(
            this ITelegramBotClient botClient,
            string inlineMessageId,
            InputMediaBase media,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new EditInlineMessageMediaRequest(inlineMessageId, media)
                    {
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to edit only the reply markup of messages sent by the bot or via the
        /// bot (for inline bots).
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="messageId">Unique identifier of the sent message</param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, the edited <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#editmessagereplymarkup"/>
        public static async Task<Message> EditMessageReplyMarkupAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int messageId,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new EditMessageReplyMarkupRequest(chatId, messageId, replyMarkup),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to edit only the reply markup of messages sent by the bot or via
        /// the bot (for inline bots).
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="inlineMessageId">Unique identifier of the sent message</param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#editmessagereplymarkup"/>
        public static async Task EditMessageReplyMarkupAsync(
            this ITelegramBotClient botClient,
            string inlineMessageId,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new EditInlineMessageReplyMarkupRequest(inlineMessageId, replyMarkup),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to edit live location messages sent by the bot.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="messageId">Unique identifier of the sent message</param>
        /// <param name="latitude">Latitude of location</param>
        /// <param name="longitude">Longitude of location</param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success the edited <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#editmessagelivelocation"/>
        public static async Task<Message> EditMessageLiveLocationAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int messageId,
            double latitude,
            double longitude,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new EditMessageLiveLocationRequest(chatId, messageId, latitude, longitude)
                    {
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to edit live location messages sent via the bot (for inline bots).
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="inlineMessageId">Unique identifier of the sent message</param>
        /// <param name="latitude">Latitude of location</param>
        /// <param name="longitude">Longitude of location</param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#editmessagelivelocation"/>
        public static async Task EditMessageLiveLocationAsync(
            this ITelegramBotClient botClient,
            string inlineMessageId,
            double latitude,
            double longitude,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new EditInlineMessageLiveLocationRequest(inlineMessageId, latitude, longitude)
                    {
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to send a native poll. A native poll can't be sent to a private chat.
        /// On success, the sent <see cref="Message"/> is returned.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId"><see cref="ChatId"/> for the target chat</param>
        /// <param name="messageId">Identifier of the original message with the poll</param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a new message inline keyboard.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>
        /// On success, the stopped <see cref="Poll"/> with the final results is returned.
        /// </returns>
        /// <see href="https://core.telegram.org/bots/api#stoppoll"/>
        public static async Task<Poll> StopPollAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int messageId,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new StopPollRequest(chatId, messageId)
                    {
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to delete a message. A message can only be deleted if it was sent
        /// less than 48 hours ago. Any such recently sent outgoing message may be deleted.
        /// Additionally, if the bot is an administrator in a group chat, it can delete any
        /// message. If the bot is an administrator in a supergroup, it can delete messages from
        /// any other user and service messages about people joining or leaving the group (other
        /// types of service messages may only be removed by the group creator). In channels, bots
        /// can only remove their own messages.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// <see cref="ChatId"/> Unique identifier for the target chat or username of the target
        /// channel (in the format @channelusername)
        /// </param>
        /// <param name="messageId">Unique identifier of the message to delete</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#deletemessage"/>
        public static async Task DeleteMessageAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int messageId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new DeleteMessageRequest(chatId, messageId),
                    cancellationToken
                );

        #endregion Updating messages

        #region Inline mode

        /// <summary>
        /// Use this method to send answers to an inline query.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="inlineQueryId">Unique identifier for answered query</param>
        /// <param name="results">A array of results for the inline query</param>
        /// <param name="cacheTime">
        /// The maximum amount of time in seconds the result of the inline query may be cached
        /// on the server
        /// </param>
        /// <param name="isPersonal">
        /// Pass <c>true</c>, if results may be cached on the server side only for the user that
        /// sent the query. By default, results may be returned to any user who sends the
        /// same query
        /// </param>
        /// <param name="nextOffset">
        /// Pass the offset that a client should send in the next query with the same text to
        /// receive more results. Pass an empty string if there are no more results or if you
        /// don't support pagination. Offset length can't exceed 64 bytes.
        /// </param>
        /// <param name="switchPmText">
        /// If passed, clients will display a button with specified text that switches the user
        /// to a private chat with the bot and sends the bot a start message with the parameter
        /// switch_pm_parameter
        /// </param>
        /// <param name="switchPmParameter">
        /// Parameter for the start message sent to the bot when user presses the switch button
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, <c>true</c> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#answerinlinequery"/>
        public static async Task AnswerInlineQueryAsync(
            this ITelegramBotClient botClient,
            string inlineQueryId,
            IEnumerable<InlineQueryResultBase> results,
            int? cacheTime = default,
            bool? isPersonal = default,
            string? nextOffset = default,
            string? switchPmText = default,
            string? switchPmParameter = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new AnswerInlineQueryRequest(inlineQueryId, results)
                    {
                        CacheTime = cacheTime,
                        IsPersonal = isPersonal,
                        NextOffset = nextOffset,
                        SwitchPmText = switchPmText,
                        SwitchPmParameter = switchPmParameter
                    },
                    cancellationToken
                );

        # endregion Inline mode

        #region Payments

        /// <summary>
        /// Use this method to send invoices.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">Unique identifier for the target private chat</param>
        /// <param name="title">Product name</param>
        /// <param name="description">Product description</param>
        /// <param name="payload">
        /// Bot-defined invoice payload, 1-128 bytes. This will not be displayed to the user, use
        /// for your internal processes.
        /// </param>
        /// <param name="providerToken">Payments provider token, obtained via Botfather</param>
        /// <param name="startParameter">
        /// Unique deep-linking parameter that can be used to generate this invoice when used as
        /// a start parameter
        /// </param>
        /// <param name="currency">
        /// Three-letter ISO 4217 currency code, see more on currencies
        /// </param>
        /// <param name="providerData">
        /// JSON-encoded data about the invoice, which will be shared with the payment provider.
        /// A detailed description of required fields should be provided by the payment provider.
        /// </param>
        /// <param name="prices">
        /// Price breakdown, a list of components (e.g. product price, tax, discount, delivery
        /// cost, delivery tax, bonus, etc.)
        /// </param>
        /// <param name="photoUrl">
        /// URL of the product photo for the invoice. Can be a photo of the goods or a marketing
        /// image for a service.
        /// </param>
        /// <param name="photoSize">Photo size</param>
        /// <param name="photoWidth">Photo width</param>
        /// <param name="photoHeight">Photo height</param>
        /// <param name="needName">
        /// Pass <c>true</c>, if you require the user's full name to complete the order
        /// </param>
        /// <param name="needPhoneNumber">
        /// Pass <c>true</c>, if you require the user's phone number to complete the order
        /// </param>
        /// <param name="needEmail">
        /// Pass <c>true</c>, if you require the user's email to complete the order
        /// </param>
        /// <param name="needShippingAddress">
        /// Pass <c>true</c>, if you require the user's shipping address to complete the order
        /// </param>
        /// <param name="sendPhoneNumberToProvider">
        /// Pass <c>true</c>, if user's phone number should be sent to provider
        /// </param>
        /// <param name="sendEmailToProvider">
        /// Pass <c>true</c>, if user's email address should be sent to provider
        /// </param>
        /// <param name="isFlexible">
        /// Pass <c>true</c>, if the final price depends on the shipping method
        /// </param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a custom reply keyboard, instructions to hide keyboard
        /// or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendinvoice"/>
        public static async Task<Message> SendInvoiceAsync(
            this ITelegramBotClient botClient,
            int chatId,
            string title,
            string description,
            string payload,
            string providerToken,
            string startParameter,
            string currency,
            IEnumerable<LabeledPrice> prices,
            string? providerData = default,
            string? photoUrl = default,
            int? photoSize = default,
            int? photoWidth = default,
            int? photoHeight = default,
            bool? needName = default,
            bool? needPhoneNumber = default,
            bool? needEmail = default,
            bool? needShippingAddress = default,
            bool? sendPhoneNumberToProvider = default,
            bool? sendEmailToProvider = default,
            bool? isFlexible = default,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendInvoiceRequest(
                        chatId,
                        title,
                        description,
                        payload,
                        providerToken,
                        startParameter,
                        currency,
                        prices)
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
                        SendPhoneNumberToProvider = sendPhoneNumberToProvider,
                        SendEmailToProvider = sendEmailToProvider,
                        IsFlexible = isFlexible,
                        DisableNotification = disableNotification,
                        ReplyToMessageId = replyToMessageId,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to reply to shipping queries with success and shipping options. If
        /// you sent an invoice requesting a shipping address and the parameter
        /// <see cref="SendInvoiceRequest.IsFlexible"/> was  specified, the Bot API will send an
        /// <see cref="Update"/> with a <see cref="Update.ShippingQuery"/> field to the bot.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="shippingQueryId">Unique identifier for the query to be answered</param>
        /// <param name="shippingOptions">Required if OK is <c>true</c>.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, <c>true</c> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#answershippingquery"/>
        public static async Task AnswerShippingQueryAsync(
            this ITelegramBotClient botClient,
            string shippingQueryId,
            IEnumerable<ShippingOption> shippingOptions,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new AnswerShippingQueryRequest(shippingQueryId, shippingOptions),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to reply to shipping queries with failure and error message. If you
        /// sent an invoice requesting a shipping address and the parameter
        /// <see cref="SendInvoiceRequest.IsFlexible"/> was specified, the Bot API will send an
        /// <see cref="Update"/> with a <see cref="Update.ShippingQuery"/> field to the bot.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="shippingQueryId">Unique identifier for the query to be answered</param>
        /// <param name="errorMessage">
        /// Required if OK is <c>false</c>. Error message in human readable form that explains
        /// why it is impossible to complete the order
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, <c>true</c> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#answershippingquery"/>
        public static async Task AnswerShippingQueryAsync(
            this ITelegramBotClient botClient,
            string shippingQueryId,
            string errorMessage,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new AnswerShippingQueryRequest(shippingQueryId, errorMessage),
                    cancellationToken
                );

        /// <summary>
        /// Respond to a pre-checkout query with success
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="preCheckoutQueryId">Unique identifier for the query to be answered</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, <c>true</c> is returned.</returns>
        /// <remarks>
        /// Note: The Bot API must receive an answer within 10 seconds after the pre-checkout
        /// query was sent.
        /// </remarks>
        /// <see href="https://core.telegram.org/bots/api#answerprecheckoutquery"/>
        public static async Task AnswerPreCheckoutQueryAsync(
            this ITelegramBotClient botClient,
            string preCheckoutQueryId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new AnswerPreCheckoutQueryRequest(preCheckoutQueryId),
                    cancellationToken
                );

        /// <summary>
        /// Respond to a pre-checkout query with failure and error message
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="preCheckoutQueryId">Unique identifier for the query to be answered</param>
        /// <param name="errorMessage">
        /// Required if OK is <c>false</c>. Error message in human readable form that explains
        /// the reason for failure to proceed with the checkout
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, <c>true</c> is returned.</returns>
        /// <remarks>
        /// Note: The Bot API must receive an answer within 10 seconds after the pre-checkout
        /// query was sent.
        /// </remarks>
        public static async Task AnswerPreCheckoutQueryAsync(
            this ITelegramBotClient botClient,
            string preCheckoutQueryId,
            string errorMessage,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new AnswerPreCheckoutQueryRequest(preCheckoutQueryId, errorMessage),
                    cancellationToken
                );

        #endregion Payments

        #region Games

        /// <summary>
        /// Use this method to send a game.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">Unique identifier of the target chat</param>
        /// <param name="gameShortName">
        /// Short name of the game, serves as the unique identifier for the game.
        /// </param>
        /// <param name="disableNotification">
        /// Sends the message silently. iOS users will not receive a notification, Android users
        /// will receive a notification with no sound.
        /// </param>
        /// <param name="replyToMessageId">
        /// If the message is a reply, ID of the original message
        /// </param>
        /// <param name="replyMarkup">
        /// A JSON-serialized object for a custom reply keyboard, instructions to hide keyboard
        /// or to force a reply from the user.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, the sent <see cref="Message"/> is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#sendgame"/>
        public static async Task<Message> SendGameAsync(
            this ITelegramBotClient botClient,
            long chatId,
            string gameShortName,
            bool? disableNotification = default,
            int? replyToMessageId = default,
            InlineKeyboardMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SendGameRequest(chatId, gameShortName)
                    {
                        DisableNotification = disableNotification,
                        ReplyToMessageId = replyToMessageId,
                        ReplyMarkup = replyMarkup
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to set the score of the specified user in a game.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="userId">Unique identifier of the target user.</param>
        /// <param name="score">The score.</param>
        /// <param name="chatId">Unique identifier of the target chat.</param>
        /// <param name="messageId">Unique identifier of the sent message.</param>
        /// <param name="force">
        /// Pass <c>true</c>, if the high score is allowed to decrease. This can be useful when
        /// fixing mistakes or banning cheaters
        /// </param>
        /// <param name="disableEditMessage">
        /// Pass <c>true</c>, if the game message should not be automatically edited to include
        /// the current scoreboard
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, returns the edited <see cref="Message"/></returns>
        /// <see href="https://core.telegram.org/bots/api#setgamescore"/>
        public static async Task<Message> SetGameScoreAsync(
            this ITelegramBotClient botClient,
            int userId,
            int score,
            long chatId,
            int messageId,
            bool? force = default,
            bool? disableEditMessage = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SetGameScoreRequest(userId, score, chatId, messageId)
                    {
                        Force = force,
                        DisableEditMessage = disableEditMessage
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to set the score of the specified user in a game.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="userId">Unique identifier of the target user.</param>
        /// <param name="score">The score.</param>
        /// <param name="inlineMessageId">Identifier of the inline message.</param>
        /// <param name="force">
        /// Pass <c>true</c>, if the high score is allowed to decrease. This can be useful when
        /// fixing mistakes or banning cheaters
        /// </param>
        /// <param name="disableEditMessage">
        /// Pass <c>true</c>, if the game message should not be automatically edited to include
        /// the current scoreboard
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success returns <c>true</c></returns>
        /// <see href="https://core.telegram.org/bots/api#setgamescore"/>
        public static async Task SetGameScoreAsync(
            this ITelegramBotClient botClient,
            int userId,
            int score,
            string inlineMessageId,
            bool? force = default,
            bool? disableEditMessage = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SetInlineGameScoreRequest(userId, score, inlineMessageId)
                    {
                        Force = force,
                        DisableEditMessage = disableEditMessage
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to get data for high score tables.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="userId">Unique identifier of the target user.</param>
        /// <param name="chatId">Unique identifier of the target chat.</param>
        /// <param name="messageId">Unique identifier of the sent message.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, returns an Array of <see cref="GameHighScore"/> objects</returns>
        /// <remarks>
        /// This method will currently return scores for the target user, plus two of his closest
        /// neighbors on each side. Will also return the top three users if the user and his
        /// neighbors are not among them. Please note that this behavior is subject to change.
        /// </remarks>
        /// <see href="https://core.telegram.org/bots/api#getgamehighscores"/>
        public static async Task<GameHighScore[]> GetGameHighScoresAsync(
            this ITelegramBotClient botClient,
            int userId,
            long chatId,
            int messageId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new GetGameHighScoresRequest(userId, chatId, messageId),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to get data for high score tables.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="userId">Unique identifier of the target user.</param>
        /// <param name="inlineMessageId">Unique identifier of the inline message.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, returns an Array of <see cref="GameHighScore"/> objects</returns>
        /// <remarks>
        /// This method will currently return scores for the target user, plus two of his closest
        /// neighbors on each side. Will also return the top three users if the user and his
        /// neighbors are not among them. Please note that this behavior is subject to change.
        /// </remarks>
        /// <see href="https://core.telegram.org/bots/api#getgamehighscores"/>
        public static async Task<GameHighScore[]> GetGameHighScoresAsync(
            this ITelegramBotClient botClient,
            int userId,
            string inlineMessageId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new GetInlineGameHighScoresRequest(userId, inlineMessageId),
                    cancellationToken
                );

        #endregion Games

        #region Group and channel management

        /// <summary>
        /// Use this method to export an invite link to a supergroup or a channel. The bot must
        /// be an administrator in the chat for this to work and must have the appropriate
        /// admin rights.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns exported invite link as String on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#exportchatinvitelink"/>
        public static async Task<string> ExportChatInviteLinkAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new ExportChatInviteLinkRequest(chatId),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to set a new profile photo for the chat. Photos can't be changed for
        /// private chats. The bot must be an administrator in the chat for this to work and
        /// must have the appropriate admin rights.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="photo">The new profile picture for the chat.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#setchatphoto"/>
        public static async Task SetChatPhotoAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            InputFileStream photo,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SetChatPhotoRequest(chatId, photo),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to delete a chat photo. Photos can't be changed for private chats.
        /// The bot must be an administrator in the chat for this to work and must have the
        /// appropriate admin rights.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#deletechatphoto"/>
        public static async Task DeleteChatPhotoAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new DeleteChatPhotoRequest(chatId),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to change the title of a chat. Titles can't be changed for private
        /// chats. The bot must be an administrator in the chat for this to work and must have
        /// the appropriate admin rights.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="title">New chat title, 1-255 characters</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#setchattitle"/>
        public static async Task SetChatTitleAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            string title,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SetChatTitleRequest(chatId, title),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to change the description of a supergroup or a channel. The bot
        /// must be an administrator in the chat for this to work and must have the appropriate
        /// admin rights.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="description">
        /// New chat description, 0-255 characters. Defaults to an empty string, which would
        /// clear the description.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#setchatdescription"/>
        public static async Task SetChatDescriptionAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            string? description = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SetChatDescriptionRequest(chatId, description),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to pin a message in a supergroup. The bot must be an administrator
        /// in the chat for this to work and must have the appropriate admin rights.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target supergroup
        /// </param>
        /// <param name="messageId">Identifier of a message to pin</param>
        /// <param name="disableNotification">
        /// Pass <c>true</c>, if it is not necessary to send a notification to all group members
        /// about the new pinned message
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#pinchatmessage"/>
        public static async Task PinChatMessageAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            int messageId,
            bool? disableNotification = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new PinChatMessageRequest(chatId, messageId)
                    {
                        DisableNotification = disableNotification
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to unpin a message in a supergroup chat. The bot must be an
        /// administrator in the chat for this to work and must have the appropriate admin rights.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target supergroup
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success</returns>
        /// <see href="https://core.telegram.org/bots/api#unpinchatmessage"/>
        public static async Task UnpinChatMessageAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new UnpinChatMessageRequest(chatId),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to set a new group sticker set for a supergroup.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target supergroup
        /// (in the format @supergroupusername)
        /// </param>
        /// <param name="stickerSetName">
        /// Name of the sticker set to be set as the group sticker set
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success</returns>
        /// <see href="https://core.telegram.org/bots/api#setchatstickerset"/>
        public static async Task SetChatStickerSetAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            string stickerSetName,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SetChatStickerSetRequest(chatId, stickerSetName),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to delete a group sticker set from a supergroup.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target supergroup (in the
        /// format @supergroupusername)
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success</returns>
        /// <see href="https://core.telegram.org/bots/api#deletechatstickerset"/>
        public static async Task DeleteChatStickerSetAsync(
            this ITelegramBotClient botClient,
            ChatId chatId,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new DeleteChatStickerSetRequest(chatId),
                    cancellationToken
                );

        #endregion

        #region Stickers

        /// <summary>
        /// Use this method to get a sticker set.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="name">
        /// Short name of the sticker set that is used in t.me/addstickers/ URLs (e.g., animals)
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>On success, a <see cref="StickerSet"/> object is returned.</returns>
        /// <see href="https://core.telegram.org/bots/api#getstickerset"/>
        public static async Task<StickerSet> GetStickerSetAsync(
            this ITelegramBotClient botClient,
            string name,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(new GetStickerSetRequest(name), cancellationToken);

        /// <summary>
        /// Use this method to upload a .png file with a sticker for later use in
        /// <see cref="CreateNewStickerSetRequest"/> and <see cref="AddStickerToSetRequest"/>
        /// requests (can be used multiple times).
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="userId">User identifier of sticker file owner</param>
        /// <param name="pngSticker">
        /// Png image with the sticker, must be up to 512 kilobytes in size, dimensions must
        /// not exceed 512px, and either width or height must be exactly 512px.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns the uploaded <see cref="File"/> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#uploadstickerfile"/>
        public static async Task<File> UploadStickerFileAsync(
            this ITelegramBotClient botClient,
            int userId,
            InputFileStream pngSticker,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new UploadStickerFileRequest(userId, pngSticker),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to create new sticker set owned by a user. The bot will be able to
        /// edit the created sticker set.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="userId">User identifier of created sticker set owner</param>
        /// <param name="name">
        /// Short name of sticker set, to be used in t.me/addstickers/ URLs (e.g., animals).
        /// Can contain only English letters, digits and underscores. Must begin with a letter,
        /// can't contain consecutive underscores and must end in “_by_&lt;bot_username&gt;”.
        /// &lt;bot_username&gt; is case insensitive. 1-64 characters.
        /// </param>
        /// <param name="title">Sticker set title, 1-64 characters</param>
        /// <param name="pngSticker">
        /// Png image with the sticker, must be up to 512 kilobytes in size, dimensions must
        /// not exceed 512px, and either width or height must be exactly 512px.
        /// </param>
        /// <param name="emojis">One or more emoji corresponding to the sticker</param>
        /// <param name="isMasks">
        /// Pass <c>true</c>, if a set of mask stickers should be created
        /// </param>
        /// <param name="maskPosition">Position where the mask should be placed on faces</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#createnewstickerset"/>
        public static async Task CreateNewStickerSetAsync(
            this ITelegramBotClient botClient,
            int userId,
            string name,
            string title,
            InputOnlineFile pngSticker,
            string emojis,
            bool? isMasks = default,
            MaskPosition? maskPosition = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new CreateNewStickerSetRequest(userId, name, title, pngSticker, emojis)
                    {
                        ContainsMasks = isMasks,
                        MaskPosition = maskPosition
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to add a new sticker to a set created by the bot.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="userId">User identifier of sticker set owner</param>
        /// <param name="name">Sticker set name</param>
        /// <param name="pngSticker">
        /// Png image with the sticker, must be up to 512 kilobytes in size, dimensions must
        /// not exceed 512px, and either width or height must be exactly 512px.
        /// </param>
        /// <param name="emojis">One or more emoji corresponding to the sticker</param>
        /// <param name="maskPosition">Position where the mask should be placed on faces</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success</returns>
        /// <see href="https://core.telegram.org/bots/api#addstickertoset"/>
        public static async Task AddStickerToSetAsync(
            this ITelegramBotClient botClient,
            int userId,
            string name,
            InputOnlineFile pngSticker,
            string emojis,
            MaskPosition? maskPosition = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new AddStickerToSetRequest(userId, name, pngSticker, emojis)
                    {
                        MaskPosition = maskPosition
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to create new sticker set owned by a user. The bot will be able to
        /// edit the created sticker set.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="userId">User identifier of created sticker set owner</param>
        /// <param name="name">
        /// Short name of sticker set, to be used in t.me/addstickers/ URLs (e.g., animals).
        /// Can contain only English letters, digits and underscores. Must begin with a letter,
        /// can't contain consecutive underscores and must end in “_by_&lt;bot_username&gt;”.
        /// &lt;bot_username&gt; is case insensitive. 1-64 characters.
        /// </param>
        /// <param name="title">Sticker set title, 1-64 characters</param>
        /// <param name="tgsSticker">Tgs animation with the sticker</param>
        /// <param name="emojis">One or more emoji corresponding to the sticker</param>
        /// <param name="isMasks">
        /// Pass <c>true</c>, if a set of mask stickers should be created
        /// </param>
        /// <param name="maskPosition">Position where the mask should be placed on faces</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#createnewstickerset"/>
        public static async Task CreateNewAnimatedStickerSetAsync(
            this ITelegramBotClient botClient,
            int userId,
            string name,
            string title,
            InputFileStream tgsSticker,
            string emojis,
            bool? isMasks = default,
            MaskPosition? maskPosition = default,
            CancellationToken cancellationToken = default) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new CreateNewAnimatedStickerSetRequest(userId, name, title, tgsSticker, emojis)
                    {
                        ContainsMasks = isMasks,
                        MaskPosition = maskPosition
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to add a new sticker to a set created by the bot.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="userId">User identifier of sticker set owner</param>
        /// <param name="name">Sticker set name</param>
        /// <param name="tgsSticker">Tgs animation with the sticker</param>
        /// <param name="emojis">One or more emoji corresponding to the sticker</param>
        /// <param name="maskPosition">Position where the mask should be placed on faces</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success</returns>
        /// <see href="https://core.telegram.org/bots/api#addstickertoset"/>
        public static async Task AddAnimatedStickerToSetAsync(
            this ITelegramBotClient botClient,
            int userId,
            string name,
            InputFileStream tgsSticker,
            string emojis,
            MaskPosition? maskPosition = default,
            CancellationToken cancellationToken = default) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new AddAnimatedStickerToSetRequest(userId, name, tgsSticker, emojis)
                    {
                        MaskPosition = maskPosition
                    },
                    cancellationToken
                );

        /// <summary>
        /// Use this method to move a sticker in a set created by the bot to a specific position.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="sticker">File identifier of the sticker</param>
        /// <param name="position">New sticker position in the set, zero-based</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns><c>true</c> on success</returns>
        /// <see href="https://core.telegram.org/bots/api#setstickerpositioninset"/>
        public static async Task SetStickerPositionInSetAsync(
            this ITelegramBotClient botClient,
            string sticker,
            int position,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new SetStickerPositionInSetRequest(sticker, position),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to delete a sticker from a set created by the bot.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="sticker">File identifier of the sticker</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#deletestickerfromset"/>
        public static async Task DeleteStickerFromSetAsync(
            this ITelegramBotClient botClient,
            string sticker,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient)))
                .MakeRequestAsync(
                    new DeleteStickerFromSetRequest(sticker),
                    cancellationToken
                );

        /// <summary>
        /// Use this method to set the thumbnail of a sticker set. Animated thumbnails can be set
        /// for animated sticker sets only. Returns True on success.
        /// </summary>
        /// <param name="botClient"><see cref="ITelegramBotClient"/> instance</param>
        /// <param name="name">Sticker set name</param>
        /// <param name="userId">User identifier of the sticker set owner</param>
        /// <param name="thumb">A PNG image or a TGS animation with the thumbnail</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>Returns <c>true</c> on success.</returns>
        /// <see href="https://core.telegram.org/bots/api#setstickersetthumb"/>
        public static async Task SetStickerSetThumbAsync(
            this ITelegramBotClient botClient,
            string name,
            int userId,
            InputOnlineFile? thumb = default,
            CancellationToken cancellationToken = default
        ) =>
            await (botClient ?? throw new ArgumentNullException(nameof(botClient))).MakeRequestAsync(
                new SetStickerSetThumbRequest(name, userId, thumb),
                cancellationToken
            );

        #endregion
    }
}
