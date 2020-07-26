using System;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;
using File = System.IO.File;

namespace Telegram.Bot.Tests.Integ.Getting_Updates
{
    /// <remarks>
    /// Webhooks must be immediately disabled because the test framework uses getUpdates method.
    /// </remarks>
    [Collection(Constants.TestCollections.Webhook)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class WebhookTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public WebhookTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should set webhook", Skip = "setWebhook requests are rate limited")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetWebhook)]
        public async Task Should_Set_Webhook()
        {
            await BotClient.SetWebhookAsync(url: "https://www.telegram.org/");
        }

        [OrderedFact("Should set webhook with options", Skip = "setWebhook requests are rate limited")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetWebhook)]
        public async Task Should_Set_Webhook_With_Options()
        {
            await BotClient.SetWebhookAsync(
                url: "https://www.t.me/",
                maxConnections: 5,
                allowedUpdates: new [] { UpdateType.CallbackQuery, UpdateType.InlineQuery }
            );
        }

        [OrderedFact("Should delete the webhook by setting it to an empty URL")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetWebhook)]
        public async Task Should_Delete_Webhook_Using_setWebhook()
        {
            await BotClient.SetWebhookAsync(url: "");
        }

        [OrderedFact("Should set webhook with self-signed certificate")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetWebhook)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetWebhookInfo)]
        public async Task Should_Set_Webhook_With_SelfSigned_Cert()
        {
            await using (Stream stream = File.OpenRead(Constants.PathToFile.Certificate.PublicKey))
            {
                await BotClient.SetWebhookAsync(
                    url: "https://www.telegram.org/",
                    certificate: stream,
                    maxConnections: 3,
                    allowedUpdates: Array.Empty<UpdateType>() // send all types of updates
                );
            }

            WebhookInfo info = await BotClient.GetWebhookInfoAsync();

            Assert.Equal("https://www.telegram.org/", info.Url);
            Assert.True(info.HasCustomCertificate);
            Assert.Equal(3, info.MaxConnections);
            Assert.Null(info.AllowedUpdates);
        }

        [OrderedFact("Should delete webhook")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.DeleteWebhook)]
        public async Task Should_Delete_Webhook()
        {
            await BotClient.DeleteWebhookAsync();
        }

        [OrderedFact("Should get info of the deleted webhook")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetWebhookInfo)]
        public async Task Should_Get_Deleted_Webhook_Info()
        {
            WebhookInfo info = await BotClient.GetWebhookInfoAsync();

            Assert.Empty(info.Url);
            Assert.False(info.HasCustomCertificate);
            Assert.Null(info.MaxConnections);
            Assert.Null(info.AllowedUpdates);
        }
    }
}
