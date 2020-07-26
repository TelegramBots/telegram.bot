using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Telegram.Bot.Tests.Integ.Framework
{
    public class TestsFixture : IDisposable
    {
        private readonly IMessageSink _diagnosticMessageSink;

        public ITelegramBotClient BotClient { get; private set; }

        public User BotUser { get; private set; }

        public UpdateReceiver UpdateReceiver { get; private set; }

        public Chat SupergroupChat { get; private set; }

        public Chat PrivateChat { get; set; }

        public Chat ChannelChat { get; set; }

        public RunSummary RunSummary { get; } = new RunSummary();

        public static TestsFixture Instance { get; private set; }

        private CancellationToken CancellationToken =>
            new CancellationTokenSource(TimeSpan.FromSeconds(45)).Token;

        public TestsFixture(IMessageSink diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
            InitAsync().GetAwaiter().GetResult();
            Instance = this;
        }

        public void Dispose()
        {
            UpdateReceiver.DiscardNewUpdatesAsync(CancellationToken).GetAwaiter().GetResult();

            int passed = RunSummary.Total - RunSummary.Skipped - RunSummary.Failed;

            BotClient.SendTextMessageAsync(
                SupergroupChat.Id,
                string.Format(
                    Constants.TestExecutionResultMessageFormat,
                    RunSummary.Total,
                    passed,
                    RunSummary.Skipped,
                    RunSummary.Failed
                ),
                ParseMode.Markdown,
                cancellationToken: CancellationToken
            ).GetAwaiter().GetResult();
        }

        public Task<Message> SendTestInstructionsAsync(
            string instructions,
            ChatId chatId = default,
            bool startInlineQuery = default
        )
        {
            string text = string.Format(Constants.InstructionsMessageFormat, instructions);
            chatId ??= SupergroupChat.Id;

            IReplyMarkup replyMarkup = startInlineQuery
                ? (InlineKeyboardMarkup) InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Start inline query")
                : default;

            return BotClient.SendTextMessageAsync(
                chatId,
                text,
                ParseMode.Markdown,
                replyMarkup: replyMarkup,
                cancellationToken: CancellationToken
            );
        }

        public async Task<Message> SendTestCaseNotificationAsync(string testCase) =>
            await SendNotificationToChatAsync(false, testCase);

        public async Task<Message> SendTestCollectionNotificationAsync(
            string collectionName,
            string instructions = default,
            ChatId chatId = default) =>
            await SendNotificationToChatAsync(
                true,
                collectionName,
                instructions,
                chatId
            );

        public async Task<Chat> GetChatFromTesterAsync(
            ChatType chatType,
            CancellationToken cancellationToken = default)
        {
            bool IsMatch(Update u) =>
            (
                u.Message?.Chat?.Type == chatType &&
                u.Message.Text?.StartsWith("/test", StringComparison.OrdinalIgnoreCase) == true
            ) || (
                ChatType.Channel == chatType &&
                ChatType.Channel == u.Message?.ForwardFromChat?.Type
            );

            await UpdateReceiver.DiscardNewUpdatesAsync(cancellationToken);

            var update = await UpdateReceiver.GetUpdateAsync(
                IsMatch,
                updateTypes: UpdateType.Message,
                cancellationToken: cancellationToken
            );

            return chatType == ChatType.Channel
                ? update.Message?.ForwardFromChat
                : update.Message?.Chat;
        }

        public async Task<Chat> GetChatFromAdminAsync()
        {
            bool IsMatch(Update u) => u.Message?.Type == MessageType.Contact ||
                                      u.Message?.ForwardFrom?.Id != null;

            var update = await UpdateReceiver.GetUpdateAsync(
                predicate: IsMatch,
                updateTypes: UpdateType.Message
            );

            await UpdateReceiver.DiscardNewUpdatesAsync();

            int? userId = update.Message?.Type == MessageType.Contact
                ? update.Message?.Contact?.UserId
                : update.Message?.ForwardFrom?.Id;

            if (userId is null) throw new InvalidOperationException("Can't find userId");

            return await BotClient.GetChatAsync(userId);
        }

        private async Task InitAsync()
        {
            string apiToken = ConfigurationProvider.TestConfigurations.ApiToken;
            var retryHttpClientHandler = new RetryHttpClientHandler(3, _diagnosticMessageSink);
            var httpClient = new HttpClient(retryHttpClientHandler);
            BotClient = new TelegramBotClient(apiToken, httpClient);
            BotUser = await BotClient.GetMeAsync(CancellationToken);
            await BotClient.DeleteWebhookAsync(CancellationToken);
            SupergroupChat = await FindSupergroupTestChatAsync();
            var allowedUserNames = await FindAllowedTesterUserNames();
            UpdateReceiver = new UpdateReceiver(BotClient, allowedUserNames);

            await BotClient.SendTextMessageAsync(
                SupergroupChat.Id,
                "```\nTest execution is starting...\n```\n" +
                "#testers\n" +
                "These users are allowed to interact with the bot:\n\n" + UpdateReceiver.GetTesters(),
                ParseMode.Markdown,
                disableNotification: true,
                cancellationToken: CancellationToken
            );

#if DEBUG
            BotClient.MakingApiRequest += OnMakingApiRequest;
            BotClient.ApiResponseReceived += OnApiResponseReceived;
#endif
        }

        private Task<Message> SendNotificationToChatAsync(
            bool isForCollection,
            string name,
            string instructions = default,
            ChatId chatId = default,
            bool switchInlineQuery = default
        )
        {
            var textFormat = isForCollection
                ? Constants.StartCollectionMessageFormat
                : Constants.StartTestCaseMessageFormat;

            string text = string.Format(textFormat, name);

            chatId ??= SupergroupChat.Id;
            if (instructions != default)
            {
                text += "\n\n" + string.Format(Constants.InstructionsMessageFormat, instructions);
            }

            IReplyMarkup replyMarkup = switchInlineQuery
                ? (InlineKeyboardMarkup) InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Start inline query")
                : default;

            var task = BotClient.SendTextMessageAsync(chatId, text, ParseMode.Markdown,
                replyMarkup: replyMarkup,
                cancellationToken: CancellationToken);
            return task;
        }

        private async Task<Chat> FindSupergroupTestChatAsync()
        {
            string supergroupChatId = ConfigurationProvider.TestConfigurations.SuperGroupChatId;
            if (string.IsNullOrWhiteSpace(supergroupChatId))
            {
                throw new InvalidOperationException("Supergroup ID is not provided or is empty.");
            }

            var supergroupChat = await BotClient.GetChatAsync(supergroupChatId, CancellationToken);

            return supergroupChat;
        }

        private async Task<IEnumerable<string>> FindAllowedTesterUserNames()
        {
            // Try to get user names from test configurations first
            string[] allowedUserNames = ConfigurationProvider.TestConfigurations.AllowedUserNames
                .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.Trim())
                .ToArray();

            if (!allowedUserNames.Any())
            {
                // Assume all chat admins are allowed testers
                ChatMember[] admins = await BotClient.GetChatAdministratorsAsync(
                    SupergroupChat,
                    CancellationToken
                );
                allowedUserNames = admins
                    .Where(member => !member.User.IsBot)
                    .Select(member => member.User.Username)
                    .ToArray();
            }

            return allowedUserNames;
        }

#if DEBUG
// Disable "The variable ‘x’ is assigned but its value is never used":
#pragma warning disable 219
        // ReSharper disable NotAccessedVariable
        // ReSharper disable RedundantAssignment
        private void OnMakingApiRequest(object sender, ApiRequestEventArgs e)
        {
            bool hasContent;
            string content;
            string[] multipartContent;
            if (e.HttpContent is null)
            {
                hasContent = false;
            }
            else if (e.HttpContent is MultipartFormDataContent multipartFormDataContent)
            {
                hasContent = true;
                multipartContent = multipartFormDataContent
                    .Select(c => c is StringContent
                        ? $"{c.Headers}\n{c.ReadAsStringAsync().Result}"
                        : c.Headers.ToString()
                    )
                    .ToArray();
            }
            else
            {
                hasContent = true;
                content = e.HttpContent.ReadAsStringAsync().Result;
            }

            /* Debugging Hint: set breakpoints with conditions here in order to investigate the HTTP request values. */
            _ = new object();
        }

        // ReSharper disable UnusedVariable
        private void OnApiResponseReceived(object sender, ApiResponseEventArgs e)
        {
            string content = e.ResponseMessage.Content.ReadAsStringAsync().Result;

            /* Debugging Hint: set breakpoints with conditions here in order to investigate the HTTP response received. */
            _ = new object();
        }
#pragma warning restore 219
#endif
        private static class Constants
        {
            public const string StartCollectionMessageFormat = "💬 Test Collection:\n*{0}*";

            public const string StartTestCaseMessageFormat = "🔹 Test Case:\n*{0}*";

            public const string InstructionsMessageFormat = "👉 _Instructions_: 👈\n{0}";

            public const string TestExecutionResultMessageFormat =
                "```\nTest execution is finished.\n```" +
                "Total: {0} tests\n" +
                "✅ `{1} passed`\n" +
                "⚠ `{2} skipped`\n" +
                "❎ `{3} failed`";
        }
    }
}
