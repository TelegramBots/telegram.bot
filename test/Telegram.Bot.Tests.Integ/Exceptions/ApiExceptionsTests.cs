using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Exceptions
{
    [Collection(Constants.TestCollections.Exceptions)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class ApiExceptionsTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public ApiExceptionsTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should throw ApiRequestException while trying to send message to a user who hasn't " +
                     "started a chat with bot but bot knows about him/her.")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_ApiRequestException_When_Chat_Not_Initiated()
        {
            await _fixture.SendTestInstructionsAsync(
                "Forward a message to this chat from a user that never started a chat with this bot"
            );

            Update forwardedMessageUpdate = await _fixture.UpdateReceiver.GetUpdateAsync(u =>
                    u.Message?.ForwardFrom != null,
                updateTypes: UpdateType.Message
            );
            await _fixture.UpdateReceiver.DiscardNewUpdatesAsync();

            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await BotClient.SendTextMessageAsync(
                    chatId: forwardedMessageUpdate!.Message!.ForwardFrom!.Id,
                    text: "Error! If you see this message, talk to" +
                          $" @{forwardedMessageUpdate.Message.From!.Username}"
                )
            );

            Assert.Equal(403, exception.ErrorCode);
        }

        [OrderedFact("Should throw ApiRequestException while trying to send message to another bot")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_ApiRequestException_When_Send_To_Another_Bot()
        {
            await _fixture.SendTestInstructionsAsync(
                "Forward a message to this chat from another bot"
            );

            Update forwardedMessageUpdate = await _fixture.UpdateReceiver.GetUpdateAsync(u =>
                    u.Message?.ForwardFrom?.IsBot == true,
                updateTypes: UpdateType.Message
            );
            await _fixture.UpdateReceiver.DiscardNewUpdatesAsync();

            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await BotClient.SendTextMessageAsync(
                    chatId: forwardedMessageUpdate!.Message!.ForwardFrom!.Id,
                    text: "Error! If you see this message, talk to" +
                          $" @{forwardedMessageUpdate.Message.From!.Username}"
                )
            );

            Assert.Equal(403, exception.ErrorCode);
            Assert.Equal("Forbidden: bot can't send messages to bots", exception.Message);
        }
    }
}
