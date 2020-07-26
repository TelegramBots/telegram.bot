using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Sending_Messages
{
    [Collection(Constants.TestCollections.SendVenueMessage)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class SendingVenueMessageTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public SendingVenueMessageTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should send a venue")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendVenue)]
        public async Task Should_Send_Venue()
        {
            string title = "Rubirosa Ristorante";
            string address = "235 Mulberry St";
            double latitude = 40.722728f;
            double longitude = -73.996006f;
            string foursquareId = "4cc6222106c25481d7a4a047";

            Message message = await BotClient.SendVenueAsync(
                chatId: _fixture.SupergroupChat,
                latitude: latitude,
                longitude: longitude,
                title: title,
                address: address,
                foursquareId: foursquareId
            );

            Assert.Equal(MessageType.Venue, message.Type);
            Assert.Equal(title, message.Venue!.Title);
            Assert.Equal(address, message.Venue.Address);
            Assert.Equal(foursquareId, message.Venue.FoursquareId);
            Assert.InRange(message.Venue.Location.Latitude, latitude - 0.001f, latitude + 0.001f);
            Assert.InRange(message.Venue.Location.Longitude, longitude - 0.001f, longitude + 0.001f);
        }
    }
}
