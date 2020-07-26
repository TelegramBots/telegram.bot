using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Tests.Integ.Stickers
{
    public class StickersTestsFixture
    {
        public StickerSet EvilMindsStickerSet { get; set; }

        public IEnumerable<File> UploadedStickers { get; set; }

        public string TestStickerSetName { get; }

        public StickerSet TestStickerSet { get; set; }

        public int OwnerUserId { get; }

        public StickersTestsFixture(TestsFixture testsFixture)
        {
            TestStickerSetName = $"test_set_by_{testsFixture.BotUser.Username}";
            OwnerUserId = GetStickerOwnerIdAsync(testsFixture, Constants.TestCollections.Stickers)
                .GetAwaiter().GetResult();
        }

        private static async Task<int> GetStickerOwnerIdAsync(TestsFixture testsFixture, string collectionName)
        {
            int ownerId;

            if (ConfigurationProvider.TestConfigurations.StickerOwnerUserId == default)
            {
                await testsFixture.UpdateReceiver.DiscardNewUpdatesAsync();

                Message notifMessage = await testsFixture.SendTestCollectionNotificationAsync(collectionName,
                    $"\nNo value is set for `{nameof(ConfigurationProvider.TestConfigurations.StickerOwnerUserId)}` " +
                    "in test settings.\n\n" +
                    ""
                );

                const string cqData = "sticker_tests:owner";
                Message cqMessage = await testsFixture.BotClient.SendTextMessageAsync(
                    testsFixture.SupergroupChat,
                    testsFixture.UpdateReceiver.GetTesters() +
                    "\nUse the following button to become Sticker Set Owner",
                    replyToMessageId: notifMessage.MessageId,
                    replyMarkup: new InlineKeyboardMarkup(
                        InlineKeyboardButton.WithCallbackData("I am the Owner!", cqData)
                    )
                );

                Update cqUpdate = await testsFixture.UpdateReceiver
                    .GetCallbackQueryUpdateAsync(cqMessage.MessageId, cqData);

                ownerId = cqUpdate.CallbackQuery!.From.Id;
            }
            else
            {
                ownerId = ConfigurationProvider.TestConfigurations.StickerOwnerUserId.Value;
            }

            return ownerId;
        }
    }
}
