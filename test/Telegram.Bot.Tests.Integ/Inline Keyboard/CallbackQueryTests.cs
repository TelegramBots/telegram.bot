using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Tests.Integ.Interactive
{
    [Collection(Constants.TestCollections.CallbackQuery)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class CallbackQueryTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public CallbackQueryTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should receive and answer callback query result with a notification")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerCallbackQuery)]
        public async Task Should_Answer_With_Notification()
        {
            string callbackQueryData = $"a{new Random().Next(5_000).ToString()}";

            Message message = await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat.Id,
                text: "Please click on *OK* button.",
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(new []
                {
                    InlineKeyboardButton.WithCallbackData(
                        text: "OK",
                        callbackData: callbackQueryData
                    )
                })
            );

            Update responseUpdate = await _fixture.UpdateReceiver.GetCallbackQueryUpdateAsync(message.MessageId);
            CallbackQuery callbackQuery = responseUpdate.CallbackQuery;

            await BotClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery!.Id,
                text: "You clicked on OK"
            );

            Assert.Equal(UpdateType.CallbackQuery, responseUpdate.Type);
            Assert.Equal(callbackQueryData, callbackQuery.Data);
            Assert.NotEmpty(callbackQuery.ChatInstance);
            Assert.Null(callbackQuery.InlineMessageId);
            Assert.Null(callbackQuery.GameShortName);
            Assert.False(callbackQuery.IsGameQuery);
            Assert.False(callbackQuery.From.IsBot);
            Assert.NotNull(callbackQuery.From.Username);
            Assert.NotEmpty(callbackQuery.From.Username);
            Assert.True(JToken.DeepEquals(
                JToken.FromObject(message),
                JToken.FromObject(callbackQuery.Message)
            ));
        }

        [OrderedFact("Should receive and answer callback query result with an alert")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerCallbackQuery)]
        public async Task Should_Answer_With_Alert()
        {
            string callbackQueryData = $"b{new Random().Next(5_000)}";

            Message message = await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat.Id,
                text: "Please click on *Notify* button.",
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData(
                        text: "Notify",
                        callbackData: callbackQueryData
                    )
                )
            );

            Update responseUpdate = await _fixture.UpdateReceiver.GetCallbackQueryUpdateAsync(message.MessageId);
            CallbackQuery callbackQuery = responseUpdate.CallbackQuery;

            await BotClient.AnswerCallbackQueryAsync(
                callbackQueryId: responseUpdate.CallbackQuery!.Id,
                text: "Got it!",
                showAlert: true
            );

            Assert.Equal(UpdateType.CallbackQuery, responseUpdate.Type);
            Assert.NotNull(callbackQuery);
            Assert.Equal(callbackQueryData, callbackQuery.Data);
            Assert.NotEmpty(callbackQuery.ChatInstance);
            Assert.Null(callbackQuery.InlineMessageId);
            Assert.Null(callbackQuery.GameShortName);
            Assert.False(callbackQuery.IsGameQuery);
            Assert.False(callbackQuery.From.IsBot);
            Assert.NotNull(callbackQuery.From.Username);
            Assert.NotEmpty(callbackQuery.From.Username);
            Assert.True(JToken.DeepEquals(
                JToken.FromObject(message),
                JToken.FromObject(callbackQuery.Message)
            ));
        }
    }
}
