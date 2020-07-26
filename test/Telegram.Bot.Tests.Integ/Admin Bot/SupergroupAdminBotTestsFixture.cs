using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Admin_Bot
{
    public class SupergroupAdminBotTestsFixture : IAsyncLifetime
    {
        private byte[] _oldChatPhoto;

        public TestsFixture TestsFixture { get; }
        public Chat Chat => TestsFixture.SupergroupChat;
        public Message PinnedMessage { get; set; }
        public ChatPermissions ExistingDefaultPermissions { get; private set; }

        public SupergroupAdminBotTestsFixture(TestsFixture testsFixture)
        {
            TestsFixture = testsFixture;
        }

        public async Task InitializeAsync()
        {
            Chat chat = await TestsFixture.BotClient
                .GetChatAsync(TestsFixture.SupergroupChat);

            // Save existing chat photo as byte[] to restore it later because Bot API 4.4+ invalidates old
            // file_ids after changing chat photo
            if (!string.IsNullOrEmpty(chat.Photo?.BigFileId))
            {
                await using MemoryStream stream = new MemoryStream();
                await TestsFixture.BotClient
                    .GetInfoAndDownloadFileAsync(chat.Photo.BigFileId, stream);

                _oldChatPhoto = stream.ToArray();
            }

            // Save default permissions so they can be restored
            ExistingDefaultPermissions = chat.Permissions;
        }

        public async Task DisposeAsync()
        {
            // If chat had a photo before, reset the photo back.
            if (_oldChatPhoto != null)
            {
                await using MemoryStream photoStream = new MemoryStream(_oldChatPhoto);
                await TestsFixture.BotClient.SetChatPhotoAsync(
                    chatId: Chat.Id,
                    photo: photoStream
                );
            }

            // Reset original default permissions
            await TestsFixture.BotClient.SetChatPermissionsAsync(
                TestsFixture.SupergroupChat,
                ExistingDefaultPermissions
            );
        }
    }
}
