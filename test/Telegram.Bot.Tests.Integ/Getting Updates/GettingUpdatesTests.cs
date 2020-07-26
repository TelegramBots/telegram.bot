using System;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Getting_Updates
{
    [Collection(Constants.TestCollections.GettingUpdates)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class GettingUpdatesTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public GettingUpdatesTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should pass API Token test with valid token")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetMe)]
        public async Task Should_Pass_Test_Api_Token()
        {
            bool result = await BotClient.TestApiAsync();

            Assert.True(result);
        }

        [OrderedFact("Should throw ApiRequestException with \"Not Found\" error when" +
                     " malformed API Token is provided")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetMe)]
        public async Task Should_Fail_Test_Api_Token()
        {
            string botToken = "0:1this_is_an-invalid-token_for_tests";
            ITelegramBotClient botClient = new TelegramBotClient(botToken);

            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(
                async () => await botClient.TestApiAsync()
            );

            Assert.Equal(404, exception.ErrorCode);
            Assert.Equal("Not Found", exception.Message);
        }

        [OrderedFact("Should fail API Token test with invalid token")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetMe)]
        public async Task Should_Test_Bad_BotToken()
        {
            string botToken = "123456:ABC-DEF1234ghIkl-zyx57W2v1u123ew11";
            ITelegramBotClient botClient = new TelegramBotClient(botToken);
            bool result = await botClient.TestApiAsync();

            Assert.False(result);
        }

        [OrderedFact("Should get bot user info")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetMe)]
        public async Task Should_Get_Bot_User()
        {
            User botUser = await BotClient.GetMeAsync();

            Assert.NotNull(botUser);
            Assert.True(botUser.IsBot);
            Assert.EndsWith("bot", botUser.Username!, StringComparison.OrdinalIgnoreCase);
        }
    }
}
