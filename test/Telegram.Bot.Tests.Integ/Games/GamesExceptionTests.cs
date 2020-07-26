using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Games
{
    [Collection(Constants.TestCollections.GameException)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class GamesExceptionTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public GamesExceptionTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should throw ApiRequestException")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendGame)]
        public async Task Should_Throw_ApiRequestException_When_GameShortName_Invalid()
        {
            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await BotClient.SendGameAsync(
                    chatId: _fixture.SupergroupChat.Id,
                    gameShortName: "my game"
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("GAME_SHORTNAME_INVALID", exception.Message);
        }

        [OrderedFact("Should throw ApiRequestException for empty name")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendGame)]
        public async Task Should_Throw_ApiRequestException_When_GameShortName_Invalid_2()
        {
            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await BotClient.SendGameAsync(
                    chatId: _fixture.SupergroupChat.Id,
                    gameShortName: string.Empty
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("game_short_name", exception.Message);
        }

        [OrderedFact("Should throw ApiRequestException for non-existent game")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendGame)]
        public async Task Should_Throw_ApiRequestException_When_GameShortName_Invalid_3()
        {
            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await BotClient.SendGameAsync(
                    chatId: _fixture.SupergroupChat.Id,
                    gameShortName: "non_existing_game"
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("wrong game short name specified", exception.Message);
        }

        [OrderedFact("Should throw ApiRequestException when a callback game button is not first")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendGame)]
        public async Task Should_Throw_ApiRequestException_When_Game_Button_Not_First()
        {
            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await BotClient.SendGameAsync(
                    chatId: _fixture.SupergroupChat.Id,
                    gameShortName: "game1",
                    replyMarkup: new InlineKeyboardMarkup(new []
                    {
                       new [] { InlineKeyboardButton.WithCallbackData("Should never be seen") },
                       new [] { InlineKeyboardButton.WithCallBackGame("Should never be seen") }
                    })
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("BUTTON_POS_INVALID", exception.Message);
        }

        [OrderedFact("Should throw ApiRequestException when an inline markup is empty")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendGame)]
        public async Task Should_Throw_ApiRequestException_When_Keyboard_Empty()
        {
            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await BotClient.SendGameAsync(
                    chatId: _fixture.SupergroupChat.Id,
                    gameShortName: "game1",
                    replyMarkup: new InlineKeyboardMarkup(
                        InlineKeyboardButton.WithCallbackData("Should never be seen")
                    )
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("REPLY_MARKUP_GAME_EMPTY", exception.Message);
        }
    }
}
