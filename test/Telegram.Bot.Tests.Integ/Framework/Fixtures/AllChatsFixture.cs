using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Framework.Fixtures
{
    public abstract class AllChatsFixture : IAsyncLifetime
    {
        private readonly IList<IAsyncLifetime> _fixtures = new List<IAsyncLifetime>();

        private readonly ChannelChatFixture _channelFixture;
        private readonly PrivateChatFixture _privateChatFixture;

        public Chat PrivateChat => _privateChatFixture.PrivateChat;
        public Chat ChannelChat => _channelFixture.ChannelChat;
        public Chat SupergroupChat { get; }

        protected AllChatsFixture(TestsFixture testsFixture, string collectionName)
        {
            SupergroupChat = testsFixture.SupergroupChat;

            _fixtures.Add(_channelFixture = new ChannelChatFixture(testsFixture, collectionName));
            _fixtures.Add(_privateChatFixture = new PrivateChatFixture(testsFixture, collectionName));
        }

        public async Task InitializeAsync()
        {
            foreach (var fixture in _fixtures)
            {
                await fixture.InitializeAsync();
            }

            await InitializeCoreAsync();
        }

        public async Task DisposeAsync()
        {
            foreach (var fixture in _fixtures)
            {
                await fixture.DisposeAsync();
            }

            await DisposeCoreAsync();
        }

        protected virtual Task InitializeCoreAsync() => Task.CompletedTask;
        protected virtual Task DisposeCoreAsync() => Task.CompletedTask;
    }
}
