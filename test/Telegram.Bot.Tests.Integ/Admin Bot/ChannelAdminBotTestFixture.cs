using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Tests.Integ.Framework.Fixtures;
using Telegram.Bot.Types;

namespace Telegram.Bot.Tests.Integ.Admin_Bot
{
    public class ChannelAdminBotTestFixture : ChannelChatFixture
    {
        private readonly TestsFixture _fixture;
        private byte[] _oldChatPhoto;
        public Message PinnedMessage { get; set; }


        public ChannelAdminBotTestFixture(TestsFixture fixture)
            : base(fixture, Constants.TestCollections.ChannelAdminBots)
        {
            _fixture = fixture;
        }

        protected override async Task InitializeCoreAsync()
        {
            // Save existing chat photo as byte[] to restore it later because Bot
            // API 4.4+ invalidates old file_ids after changing chat photo
            if (!string.IsNullOrEmpty(ChannelChat.Photo?.BigFileId))
            {
                await using MemoryStream stream = new MemoryStream();
                await _fixture.BotClient.GetInfoAndDownloadFileAsync(
                    ChannelChat.Photo.BigFileId,
                    stream
                );
                _oldChatPhoto = stream.ToArray();
            }
        }

        protected override async Task DisposeCoreAsync()
        {
            // If chat had a photo before, reset the photo back.
            if (_oldChatPhoto != null)
            {
                await using MemoryStream photoStream = new MemoryStream(_oldChatPhoto);
                await _fixture.BotClient.SetChatPhotoAsync(
                    chatId: ChannelChat.Id,
                    photo: photoStream
                );
            }
        }
    }
}
