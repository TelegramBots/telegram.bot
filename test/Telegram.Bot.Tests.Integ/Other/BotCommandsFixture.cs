using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Other
{
    public class BotCommandsFixture : IAsyncLifetime
    {
        private BotCommand[] _originalCommands;

        public TestsFixture TestsFixture { get; }
        public BotCommand[] NewBotCommands { get; set; }

        public BotCommandsFixture(TestsFixture testsFixture)
        {
            TestsFixture = testsFixture;
        }

        public async Task InitializeAsync()
        {
            _originalCommands = await TestsFixture.BotClient.GetMyCommandsAsync();
        }

        public async Task DisposeAsync()
        {
            // restore original bot commands
            if (_originalCommands != null && _originalCommands.Length != 0)
            {
                await TestsFixture.BotClient.SetMyCommandsAsync(_originalCommands);
            }
        }
    }
}
