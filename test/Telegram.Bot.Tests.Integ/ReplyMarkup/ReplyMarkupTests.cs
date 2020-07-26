using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace Telegram.Bot.Tests.Integ.ReplyMarkup
{
    [Collection(Constants.TestCollections.ReplyMarkup)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class ReplyMarkupTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public ReplyMarkupTests(TestsFixture testsFixture)
        {
            _fixture = testsFixture;
        }

        [OrderedFact("Should send a message with force reply markup")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Force_Reply()
        {
            await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat,
                text: "Message with force_reply",
                replyMarkup: new ForceReplyMarkup()
            );
        }

        [OrderedFact("Should send a message multi-row keyboard reply markup")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Send_MultiRow_Keyboard()
        {
            ReplyKeyboardMarkup replyMarkup = new []
            {
                new [] {    "Top-Left",   "Top" , "Top-Right"    },
                new [] {        "Left", "Center", "Right"        },
                new [] { "Bottom-Left", "Bottom", "Bottom-Right" },
            };

            await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat,
                text: "Message with 3x3 keyboard",
                replyMarkup: replyMarkup
            );
        }

        [OrderedFact("Should remove reply keyboard")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Remove_Reply_Keyboard()
        {
            await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat,
                text: "Message to remove keyboard",
                replyMarkup: new ReplyKeyboardRemove()
            );
        }

        [OrderedFact("Should send a message with multiple inline keyboard markup")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Send_Inline_Keyboard()
        {
            Message sentMessage = await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat,
                text: "Message with inline keyboard markup",
                replyMarkup: new InlineKeyboardMarkup(new []
                {
                    new []
                    {
                        InlineKeyboardButton.WithUrl(
                            text: "Link to Repository",
                            url: "https://github.com/TelegramBots/Telegram.Bot"
                        ),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(textAndCallbackData: "callback_data1"),
                        InlineKeyboardButton.WithCallbackData(text: "callback_data2", callbackData: "data"),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithSwitchInlineQuery(text: "switch_inline_query"),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(
                            text: "switch_inline_query_current_chat"
                        ),
                    },
                })
            );

            Assert.True(
                JToken.DeepEquals(
                    JToken.FromObject(sentMessage.ReplyMarkup),
                    JToken.FromObject(
                        new InlineKeyboardMarkup(
                            new []
                            {
                                new []
                                {
                                    InlineKeyboardButton.WithUrl(
                                        text: "Link to Repository",
                                        url: "https://github.com/TelegramBots/Telegram.Bot"
                                    ),
                                },
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData(textAndCallbackData: "callback_data1"),
                                    InlineKeyboardButton.WithCallbackData(
                                        text: "callback_data2",
                                        callbackData: "data"
                                    ),
                                },
                                new []
                                {
                                    InlineKeyboardButton.WithSwitchInlineQuery(text: "switch_inline_query"),
                                },
                                new []
                                {
                                    InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(
                                        text: "switch_inline_query_current_chat"
                                    ),
                                },
                            }
                        )
                    )
                )
            );
        }
    }
}
