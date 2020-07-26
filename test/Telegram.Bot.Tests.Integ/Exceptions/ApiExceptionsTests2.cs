using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Exceptions
{
    [Collection(Constants.TestCollections.Exceptions2)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class ApiExceptionsTests2
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public ApiExceptionsTests2(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should throw ApiRequestException while trying to send message to an invalid chat")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_ApiRequestException_For_Non_Existent_Chat()
        {
            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await BotClient.SendTextMessageAsync(chatId: 0, text: "test")
            );

            Assert.Equal(400, exception.ErrorCode);
        }

        [OrderedFact("Should throw ApiRequestException while trying to promote an invalid user id")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_ApiRequestException_For_Invalid_User_Id()
        {
            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await BotClient.PromoteChatMemberAsync(
                    chatId: _fixture.SupergroupChat.Id,
                    userId: 123456
                )
            );

            Assert.Equal(400, exception.ErrorCode);
        }

        [OrderedFact("Should throw ApiRequestException while asking for user's phone number " +
                     "in non-private chat via reply keyboard markup")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_ApiRequestException_When_Asked_To_Share_Contact_In_Non_Private_Chat()
        {
            ReplyKeyboardMarkup replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                KeyboardButton.WithRequestContact("Share Contact"),
            });

            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await BotClient.SendTextMessageAsync(
                    chatId: _fixture.SupergroupChat.Id,
                    text: "You should never see this message",
                    replyMarkup: replyMarkup
                )
            );

            Assert.Equal(400, exception.ErrorCode);
        }

        [OrderedFact("Should throw ApiRequestException while editing previously " +
                     "sent message with the same text")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Throw_ApiRequestException_When_Message_Is_Not_Modified()
        {
            string messageTextToModify = "Message text to modify";
            Message message = await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat.Id,
                text: messageTextToModify
            );

            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await BotClient.EditMessageTextAsync(
                    chatId: _fixture.SupergroupChat.Id,
                    messageId: message.MessageId,
                    text: messageTextToModify
                )
            );

            Assert.Equal(400, exception.ErrorCode);
        }
    }
}
