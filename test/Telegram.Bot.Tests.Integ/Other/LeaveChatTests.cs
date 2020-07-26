using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Other
{
    [Collection(Constants.TestCollections.LeaveChat)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class LeaveChatTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public LeaveChatTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should leave chat",
            Skip = "Bot should stay in chat for other the test cases")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.LeaveChat)]
        public async Task Should_Get_Private_Chat()
        {
            // ToDo: Exception when leaving private chat
            await BotClient.LeaveChatAsync(
                chatId: _fixture.SupergroupChat
            );
        }
    }
}
