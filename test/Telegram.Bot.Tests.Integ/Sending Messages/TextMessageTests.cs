using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Tests.Integ.Framework.Fixtures;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Sending_Messages
{
    [Collection(Constants.TestCollections.SendTextMessage)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class TextMessageTests : IClassFixture<TextMessageTests.Fixture>
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        private readonly Fixture _classFixture;

        public TextMessageTests(TestsFixture testsFixture, Fixture classFixture)
        {
            _fixture = testsFixture;
            _classFixture = classFixture;
        }

        [OrderedFact("Should send text message")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Send_Text_Message()
        {
            Message message = await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat.Id,
                text: "Hello world!"
            );

            Assert.Equal("Hello world!", message.Text);
            Assert.Equal(MessageType.Text, message.Type);
            Assert.Equal(_fixture.SupergroupChat.Id.ToString(), message.Chat!.Id.ToString());
            Assert.InRange(
                message.Date,
                DateTime.UtcNow.AddSeconds(-10),
                DateTime.UtcNow.AddSeconds(10)
            );
            Assert.Equal(_fixture.BotUser.Id, message.From!.Id);
            Assert.Equal(_fixture.BotUser.Username, message.From.Username);

            // getMe request returns more information than is present in received updates
            Asserts.UsersEqual(_fixture.BotUser, message.From);
        }

        [OrderedFact("Should send text message to channel")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Send_Text_Message_To_Channel()
        {
            string text = $"Hello members of channel {_classFixture.ChannelChatId}";

            Message message = await BotClient.SendTextMessageAsync(
                chatId: _classFixture.ChannelChatId,
                text: text
            );

            Assert.Equal(text, message.Text);
            Assert.Equal(MessageType.Text, message.Type);
            Assert.Equal(_classFixture.ChannelChat.Id, message.Chat!.Id);
            Assert.Equal(_classFixture.ChannelChat.Username, message.Chat.Username);
        }

        [OrderedFact("Should forward a message to same chat")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.ForwardMessage)]
        public async Task Should_Forward_Message()
        {
            Message message1 = await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat,
                text: "➡️ Message to be forwared ⬅️"
            );

            Message message2 = await BotClient.ForwardMessageAsync(
                chatId: _fixture.SupergroupChat,
                fromChatId: _fixture.SupergroupChat,
                messageId: message1.MessageId
            );

            Asserts.UsersEqual(_fixture.BotUser, message2.ForwardFrom);
            Assert.Null(message2.ForwardFromChat);
            Assert.Equal(default, message2.ForwardFromMessageId);
            Assert.Null(message2.ForwardSignature);
            Assert.NotNull(message2.ForwardDate);

            DateTime now = DateTime.UtcNow;

            Assert.InRange(
                message2.ForwardDate.Value,
                now.AddSeconds(-20),
                now.AddSeconds(10)
            );
        }

        [OrderedFact("Should send markdown formatted text message and parse its entities. " +
                     "Link preview should not appear.")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Parse_MarkDown_Entities()
        {
            string url = "https://telegram.org/";
            Dictionary<MessageEntityType, string> entityValueMappings = new Dictionary<MessageEntityType, string>
            {
                {MessageEntityType.Bold, "*bold*"},
                {MessageEntityType.Italic, "_italic_"},
                {MessageEntityType.TextLink, $"[inline url to Telegram.org]({url})"},
                {
                    MessageEntityType.TextMention,
                    $"[{_fixture.BotUser.Username!.Replace("_", @"\_")}](tg://user?id={_fixture.BotUser.Id})"
                },
                {MessageEntityType.Code, @"inline ""`fixed-width code`"""},
                {MessageEntityType.Pre, "```pre-formatted fixed-width code block```"},
            };

            Message message = await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat.Id,
                text: string.Join("\n", entityValueMappings.Values),
                parseMode: ParseMode.Markdown,
                disableWebPagePreview: true
            );

            Assert.NotNull(message.Entities);
            Assert.Equal(entityValueMappings.Keys, message.Entities.Select(e => e.Type));
            Assert.Equal(url, message.Entities.Single(e => e.Type == MessageEntityType.TextLink).Url);
            Asserts.UsersEqual(
                _fixture.BotUser,
                message.Entities.Single(e => e.Type == MessageEntityType.TextMention).User
            );
        }

        [OrderedFact("Should send HTML formatted text message and parse its entities. Link preview should not appear.")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Parse_HTML_Entities()
        {
            string url = "https://telegram.org/";
            (MessageEntityType Type, string Value)[] entityValueMappings =
            {
                (MessageEntityType.Bold, "<b>bold</b>"),
                (MessageEntityType.Bold, "<strong>&lt;strong&gt;</strong>"),
                (MessageEntityType.Italic, "<i>italic</i>"),
                (MessageEntityType.Italic, "<em>&lt;em&gt;</em>"),
                (MessageEntityType.TextLink, $@"<a href=""{url}"">inline url to Telegram.org</a>"),
                (
                    MessageEntityType.TextMention,
                    $@"<a href=""tg://user?id={_fixture.BotUser.Id}"">{_fixture.BotUser.Username}</a>"
                ),
                (MessageEntityType.Code, @"inline <code>""fixed-width code""</code>"),
                (MessageEntityType.Pre, "<pre>pre-formatted fixed-width code block</pre>"),
                (MessageEntityType.Strikethrough, "<s>strikethrough</s>"),
                (MessageEntityType.Underline, "<u>underline</u>"),
            };

            Message message = await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat.Id,
                text: string.Join("\n", entityValueMappings.Select(tuple => tuple.Value)),
                parseMode: ParseMode.Html,
                disableWebPagePreview: true
            );

            Assert.NotNull(message.Entities);
            Assert.Equal(
                entityValueMappings.Select(tuple => tuple.Type),
                message.Entities.Select(e => e.Type)
            );
            Assert.Equal(url, message.Entities.Single(e => e.Type == MessageEntityType.TextLink).Url);
            Asserts.UsersEqual(
                _fixture.BotUser,
                message.Entities.Single(e => e.Type == MessageEntityType.TextMention).User
            );
        }

        [OrderedFact("Should send text message and parse its entity values")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Parse_Message_Entities_Into_Values()
        {
            (MessageEntityType Type, string Value)[] entityValueMappings =
            {
                (MessageEntityType.PhoneNumber, "+386 12 345 678"),
                (MessageEntityType.Cashtag, "$EUR"),
                (MessageEntityType.Hashtag, "#TelegramBots"),
                (MessageEntityType.Mention, "@BotFather"),
                (MessageEntityType.Url, "http://github.com/TelegramBots"),
                (MessageEntityType.Email, "security@telegram.org"),
                (MessageEntityType.BotCommand, "/test"),
                (MessageEntityType.BotCommand, $"/test@{_fixture.BotUser.Username}"),
            };

            Message message = await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat.Id,
                text: string.Join("\n", entityValueMappings.Select(tuple => tuple.Value))
            );

            Assert.NotNull(message.Entities);
            Assert.Equal(
                entityValueMappings.Select(t => t.Type),
                message.Entities.Select(e => e.Type)
            );
            Assert.Equal(entityValueMappings.Select(t => t.Value), message.EntityValues);
        }

        [OrderedFact("Should send MarkdownV2 formatted text message and parse its entities. " +
                     "Link preview should not appear.")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Parse_MarkdownV2_Entities()
        {
            string url = "https://telegram.org/";
            string textMention = $"[{_fixture.BotUser.Username!.Replace("_", @"\_")}]" +
                                 $"(tg://user?id={_fixture.BotUser.Id})";

            (MessageEntityType Type, string Value)[] entityValueMappings =
            {
                (MessageEntityType.Bold, "*bold*"),
                (MessageEntityType.Italic, "_italic_"),
                (MessageEntityType.TextLink, $"[inline url to Telegram\\.org]({url})"),
                (MessageEntityType.TextMention, textMention),
                (MessageEntityType.Code, @"inline ""`fixed-width code`"""),
                (MessageEntityType.Pre, "```pre-formatted fixed-width code block```"),
                (MessageEntityType.Strikethrough, "~strikethrough~"),
                (MessageEntityType.Underline, "__underline__"),
            };

            Message message = await BotClient.SendTextMessageAsync(
                chatId: _fixture.SupergroupChat.Id,
                text: string.Join("\n", entityValueMappings.Select(x => x.Value)),
                parseMode: ParseMode.MarkdownV2,
                disableWebPagePreview: true
            );

            Assert.NotNull(message.Entities);
            Assert.Equal(entityValueMappings.Select(x => x.Type), message.Entities.Select(e => e.Type));
            Assert.Equal(url, message.Entities.Single(e => e.Type == MessageEntityType.TextLink).Url);
            Asserts.UsersEqual(
                _fixture.BotUser,
                message.Entities.Single(e => e.Type == MessageEntityType.TextMention).User
            );
        }

        public class Fixture : ChannelChatFixture
        {
            public Fixture(TestsFixture testsFixture)
                : base(testsFixture, Constants.TestCollections.SendTextMessage)
            {
            }
        }
    }
}
