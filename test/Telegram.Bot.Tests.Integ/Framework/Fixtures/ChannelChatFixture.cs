using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Framework.Fixtures
{
    public class ChannelChatFixture : IAsyncLifetime
    {
        private readonly TestsFixture _testsFixture;
        private readonly string _collectionName;

        public Chat ChannelChat { get; private set; }
        public string ChannelChatId { get; private set; }

        public ChannelChatFixture(TestsFixture testsFixture, string collectionName)
        {
            _testsFixture = testsFixture;
            _collectionName = collectionName;
        }

        public async Task InitializeAsync()
        {
            _testsFixture.ChannelChat ??= await GetChat(_collectionName);

            ChannelChat = _testsFixture.ChannelChat;

            ChannelChatId = ChannelChat.Username is null
                ? ChannelChat.Id.ToString()
                : '@' + ChannelChat.Username;

            await _testsFixture.SendTestCollectionNotificationAsync(
                _collectionName,
                $"Tests will be executed in channel {ChannelChatId.Replace("_", @"\_")}"
            );

            await InitializeCoreAsync();
        }

        public async Task DisposeAsync() => await DisposeCoreAsync();

        private async Task<Chat> GetChat(string collectionName)
        {
            var chatId = ConfigurationProvider.TestConfigurations.ChannelChatId;
            if (chatId != null) return await _testsFixture.BotClient.GetChatAsync(chatId);

            await _testsFixture.UpdateReceiver.DiscardNewUpdatesAsync();

            var botUserName = _testsFixture.BotUser.Username;
            await _testsFixture.SendTestCollectionNotificationAsync(
                collectionName,
                "No channel is set in test settings. Tester should forward a " +
                $"message from a channel so bot can run tests there. @{botUserName} " +
                "must be an admin in that channel."
            );

            return await _testsFixture.GetChatFromTesterAsync(ChatType.Channel);
        }

        protected virtual Task InitializeCoreAsync() => Task.CompletedTask;
        protected virtual Task DisposeCoreAsync() => Task.CompletedTask;
    }
}
