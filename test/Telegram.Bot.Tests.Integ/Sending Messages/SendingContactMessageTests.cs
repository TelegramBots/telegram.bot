using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Sending_Messages
{
    [Collection(Constants.TestCollections.SendContactMessage)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class SendingContactMessageTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public SendingContactMessageTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should send a contact", Skip = "Due to unexpected rate limiting errors")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendContact)]
        public async Task Should_Send_Contact()
        {
            string phoneNumber = "+1234567890";
            string firstName = "Han";
            string lastName = "Solo";

            Message message = await BotClient.SendContactAsync(
                chatId: _fixture.SupergroupChat,
                phoneNumber: phoneNumber,
                firstName: firstName,
                lastName: lastName
            );

            Assert.Equal(MessageType.Contact, message.Type);
            Assert.Equal(phoneNumber, message.Contact!.PhoneNumber);
            Assert.Equal(firstName, message.Contact.FirstName);
            Assert.Equal(lastName, message.Contact.LastName);
        }

        [OrderedFact("Should send a contact including his vCard",
            Skip = "Due to unexpected rate limiting errors")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendContact)]
        public async Task Should_Send_Contact_With_VCard()
        {
            const string vcard =
                "BEGIN:VCARD" + "\n" +
                "VERSION:2.1" + "\n" +
                "N:Gump;Forrest;;Mr." + "\n" +
                "FN:Forrest Gump" + "\n" +
                "ORG:Bubba Gump Shrimp Co." + "\n" +
                "TITLE:Shrimp Man" + "\n" +
                "PHOTO;JPEG:https://upload.wikimedia.org/wikipedia/commons/9/95/TomHanksForrestGump94.jpg" + "\n" +
                "TEL;WORK;VOICE:(111) 555-1212" + "\n" +
                "TEL;HOME;VOICE:(404) 555-1212" + "\n" +
                "ADR;HOME:;;42 Plantation St.;Baytown;LA;30314;United States of America" + "\n" +
                "LABEL;HOME;ENCODING=QUOTED-PRINTABLE;CHARSET=UTF-8:42 Plantation St.=0D=0A=" + "\n" +
                " Baytown, LA 30314=0D=0AUnited States of America" + "\n" +
                "EMAIL:forrestgump@example.org" + "\n" +
                "REV:20080424T195243Z" + "\n" +
                "END:VCARD";

            Message message = await BotClient.SendContactAsync(
                chatId: _fixture.SupergroupChat,
                phoneNumber: "+11115551212",
                firstName: "Forrest",
                vCard: vcard
            );

            Assert.Equal(MessageType.Contact, message.Type);
            Assert.Equal(vcard, message.Contact!.Vcard);
        }
    }
}
