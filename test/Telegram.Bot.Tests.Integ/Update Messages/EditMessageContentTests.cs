using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Update_Messages
{
    [Collection(Constants.TestCollections.EditMessage)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class EditMessageContentTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public EditMessageContentTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should edit an inline message's text")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerInlineQuery)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.EditMessageText)]
        public async Task Should_Edit_Inline_Message_Text()
        {
            await _fixture.SendTestInstructionsAsync(
                "Starting the inline query with this message...",
                startInlineQuery: true
            );

            #region Answer Inline Query with an Article

            Update inlineQUpdate = await _fixture.UpdateReceiver.GetInlineQueryUpdateAsync();

            (MessageEntityType Type, string Value)[] entityValueMappings =
            {
                (MessageEntityType.Bold, "<b>bold</b>"),
                (MessageEntityType.Italic, "<i>italic</i>"),
            };
            string originalMessagePrefix = "original\n";
            string messageText = originalMessagePrefix +
                                 string.Join("\n", entityValueMappings.Select(tuple => tuple.Value));

            string data = $"change-text{new Random().Next(2_000)}";

            InlineQueryResultBase[] inlineQueryResults =
            {
                new InlineQueryResultArticle(
                    id: "bot-api",
                    title: "Telegram Bot API",
                    inputMessageContent: new InputTextMessageContent(messageText: messageText)
                    {
                        ParseMode = ParseMode.Html
                    }
                )
                {
                    ReplyMarkup = InlineKeyboardButton.WithCallbackData("Click here to modify text", data)
                }
            };

            await BotClient.AnswerInlineQueryAsync(inlineQUpdate.InlineQuery!.Id, inlineQueryResults, 0);

            #endregion

            Update callbackQUpdate = await _fixture.UpdateReceiver
                .GetCallbackQueryUpdateAsync(data: data);

            string modifiedMessagePrefix = "✌ modified 👌\n";
            messageText = modifiedMessagePrefix +
                          string.Join("\n", entityValueMappings.Select(tuple => tuple.Value));

            await BotClient.EditMessageTextAsync(
                inlineMessageId: callbackQUpdate.CallbackQuery!.InlineMessageId!,
                text: messageText,
                parseMode: ParseMode.Html
            );
        }

        [OrderedFact("Should edit an inline message's markup")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerInlineQuery)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.EditMessageReplyMarkup)]
        public async Task Should_Edit_Inline_Message_Markup()
        {
            await _fixture.SendTestInstructionsAsync(
                "Starting the inline query with this message...",
                startInlineQuery: true
            );

            #region Answer Inline Query with an Article

            Update inlineQUpdate = await _fixture.UpdateReceiver.GetInlineQueryUpdateAsync();

            string data = $"change-me{new Random().Next(2_000)}";

            InlineKeyboardMarkup initialMarkup = new InlineKeyboardMarkup(new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Click here to change this button",
                    callbackData: data
                )
            });

            InputMessageContentBase inputMessageContent = new InputTextMessageContent(
                "https://core.telegram.org/bots/api"
            );

            InlineQueryResultBase[] inlineQueryResults =
            {
                new InlineQueryResultArticle(
                    id: "bot-api",
                    title: "Telegram Bot API",
                    inputMessageContent: inputMessageContent)
                {
                    Description = "The Bot API is an HTTP-based interface created for developers",
                    ReplyMarkup = initialMarkup,
                },
            };

            await BotClient.AnswerInlineQueryAsync(
                inlineQueryId: inlineQUpdate.InlineQuery!.Id,
                results: inlineQueryResults,
                cacheTime: 0
            );

            #endregion

            Update callbackQUpdate = await _fixture.UpdateReceiver
                .GetCallbackQueryUpdateAsync(data: data);

            await BotClient.EditMessageReplyMarkupAsync(
                inlineMessageId: callbackQUpdate.CallbackQuery!.InlineMessageId!,
                replyMarkup: "✌ Edited 👌"
            );
        }

        [OrderedFact("Should edit an inline message's caption")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerInlineQuery)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.EditMessageCaption)]
        public async Task Should_Edit_Inline_Message_Caption()
        {
            await _fixture.SendTestInstructionsAsync(
                "Starting the inline query with this message...",
                startInlineQuery: true
            );

            #region Answer Inline Query with an Article

            Update inlineQUpdate = await _fixture.UpdateReceiver.GetInlineQueryUpdateAsync();

            string data = $"change-me{new Random().Next(2_000)}";

            InlineKeyboardMarkup replyMarkup = new InlineKeyboardMarkup(new []
            {
                InlineKeyboardButton.WithCallbackData(
                    text: "Click here to change caption",
                    callbackData: data
                )
            });
            string url = "https://cdn.pixabay.com/photo/2017/08/30/12/45/girl-2696947_640.jpg";

            InlineQueryResultBase[] inlineQueryResults =
            {
                new InlineQueryResultPhoto(
                    id: "photo1",
                    photoUrl: url,
                    thumbUrl: url)
                {
                    Caption = "Message caption will be updated shortly",
                    ReplyMarkup = replyMarkup
                }
            };

            await BotClient.AnswerInlineQueryAsync(
                inlineQueryId: inlineQUpdate.InlineQuery!.Id,
                results: inlineQueryResults, cacheTime: 0
            );

            #endregion

            Update callbackQUpdate = await _fixture.UpdateReceiver
                .GetCallbackQueryUpdateAsync(data: data);

            await BotClient.EditMessageCaptionAsync(
                inlineMessageId: callbackQUpdate.CallbackQuery!.InlineMessageId!,
                caption: "_Caption is edited_ 👌",
                parseMode: ParseMode.Markdown
            );
        }
    }
}
