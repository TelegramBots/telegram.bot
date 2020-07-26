using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Framework.Fixtures
{
    public class PrivateChatFixture : IAsyncLifetime
    {
        public Chat PrivateChat { get; private set; }

        private readonly TestsFixture _testsFixture;
        private readonly string _collectionName;

        public PrivateChatFixture(TestsFixture testsFixture, string collectionName)
        {
            _testsFixture = testsFixture;
            _collectionName = collectionName;
        }

        private async Task<Chat> GetChat(string collectionName)
        {
            long? chatId = ConfigurationProvider.TestConfigurations.TesterPrivateChatId;
            if (chatId != null)
            {
                return await _testsFixture.BotClient.GetChatAsync(chatId);
            }

            await _testsFixture.UpdateReceiver.DiscardNewUpdatesAsync();

            string botUserName = _testsFixture.BotUser.Username;

            var configurationName = nameof(ConfigurationProvider.TestConfigurations.TesterPrivateChatId);

            await _testsFixture.SendTestCollectionNotificationAsync(
                collectionName,
                $"No value is set for `{configurationName}` in test settings. Tester should " +
                $"send /test command in private chat with @{botUserName!.Replace("_", @"\_")}."
            );

            return await _testsFixture.GetChatFromTesterAsync(ChatType.Private);
        }

        public async Task InitializeAsync()
        {
            PrivateChat ??= await GetChat(_collectionName);
            _testsFixture.PrivateChat = PrivateChat;

            var userLink = PrivateChat.GetUserLink();

            await _testsFixture.SendTestCollectionNotificationAsync(
                _collectionName,
                $"Tests will be executed in chat with {userLink}"
            );

            await InitializeCoreAsync();
        }

        public async Task DisposeAsync() => await DisposeCoreAsync();

        protected virtual Task InitializeCoreAsync() => Task.CompletedTask;
        protected virtual Task DisposeCoreAsync() => Task.CompletedTask;
    }
}
