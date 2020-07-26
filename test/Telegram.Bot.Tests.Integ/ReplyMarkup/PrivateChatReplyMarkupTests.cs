using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Tests.Integ.Framework.Fixtures;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace Telegram.Bot.Tests.Integ.ReplyMarkup
{
    [Collection(Constants.TestCollections.PrivateChatReplyMarkup)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class PrivateChatReplyMarkupTests : IClassFixture<PrivateChatReplyMarkupTests.Fixture>
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly Fixture _classFixture;

        private readonly TestsFixture _fixture;

        public PrivateChatReplyMarkupTests(TestsFixture testsFixture, Fixture fixture)
        {
            _fixture = testsFixture;
            _classFixture = fixture;
        }

        [OrderedFact("Should get contact info from keyboard reply markup",
            Skip = "Due to unexpected rate limiting errors")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Receive_Contact_Info()
        {
            await BotClient.SendTextMessageAsync(
                chatId: _classFixture.PrivateChat,
                text: "Share your contact info using the keyboard reply markup provided.",
                replyMarkup: new ReplyKeyboardMarkup(
                    keyboardRow: new [] { KeyboardButton.WithRequestContact(text: "Share Contact") },
                    resizeKeyboard: true,
                    oneTimeKeyboard: true
                )
            );

            Message contactMessage = await GetMessageFromChat(MessageType.Contact);

            Assert.NotEmpty(contactMessage.Contact!.FirstName);
            Assert.NotEmpty(contactMessage.Contact.PhoneNumber);

            if (contactMessage.Contact.UserId != null)
            {
                Assert.Equal(
                    _classFixture.PrivateChat.Id,
                    contactMessage.Contact.UserId.Value
                );
            }

            await BotClient.SendTextMessageAsync(
                chatId: _classFixture.PrivateChat,
                text: "Got it. Removing reply keyboard markup...",
                replyMarkup: new ReplyKeyboardRemove()
            );
        }

        [OrderedFact("Should get location from keyboard reply markup")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMessage)]
        public async Task Should_Receive_Location()
        {
            await BotClient.SendTextMessageAsync(
                chatId: _classFixture.PrivateChat,
                text: "Share your location using the keyboard reply markup",
                replyMarkup: new ReplyKeyboardMarkup(
                    KeyboardButton.WithRequestLocation(text: "Share Location")
                )
            );

            Message locationMessage = await GetMessageFromChat(MessageType.Location);

            Assert.NotNull(locationMessage.Location);

            await BotClient.SendTextMessageAsync(
                chatId: _classFixture.PrivateChat,
                text: "Got it. Removing reply keyboard markup...",
                replyMarkup: new ReplyKeyboardRemove()
            );
        }

        private async Task<Message> GetMessageFromChat(MessageType messageType) =>
            (await _fixture.UpdateReceiver.GetUpdateAsync(
                predicate: u => u.Message!.Type == messageType &&
                                u.Message.Chat!.Id == _classFixture.PrivateChat.Id,
                updateTypes: UpdateType.Message
            )).Message;

        public class Fixture : PrivateChatFixture
        {
            public Fixture(TestsFixture testsFixture)
                : base(testsFixture, Constants.TestCollections.ReplyMarkup)
            {
            }
        }
    }
}
