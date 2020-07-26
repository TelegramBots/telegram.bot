using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Tests.Integ.Framework.Fixtures;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Sending_Messages
{
    [Collection(Constants.TestCollections.AlbumMessage)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class AlbumMessageTests : IClassFixture<EntitiesFixture<Message>>
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly EntitiesFixture<Message> _classFixture;

        private readonly TestsFixture _fixture;

        public AlbumMessageTests(TestsFixture testsFixture, EntitiesFixture<Message> classFixture)
        {
            _fixture = testsFixture;
            _classFixture = classFixture;
        }

        [OrderedFact("Should upload 2 photos with captions and send them in an album")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMediaGroup)]
        public async Task Should_Upload_2_Photos_Album()
        {
            await using Stream stream1 = System.IO.File.OpenRead(Constants.PathToFile.Photos.Logo);
            await using Stream stream2 = System.IO.File.OpenRead(Constants.PathToFile.Photos.Bot);

            IAlbumInputMedia[] inputMedia =
            {
                new InputMediaPhoto(new InputMedia(content: stream1, fileName: "logo.png"))
                {
                    Caption = "Logo"
                },
                new InputMediaPhoto(new InputMedia(content: stream2, fileName: "bot.gif"))
                {
                    Caption = "Bot"
                },
            };

            Message[] messages = await BotClient.SendMediaGroupAsync(
                 chatId:  _fixture.SupergroupChat.Id,
                 media:  inputMedia,
                 disableNotification:  true
            );

            Assert.Equal(2, messages.Length);
            Assert.All(messages, msg => Assert.Equal(MessageType.Photo, msg.Type));

            // All media messages have the same mediaGroupId
            Assert.NotEmpty(messages.Select(m => m.MediaGroupId));
            Assert.True(messages.Select(msg => msg.MediaGroupId).Distinct().Count() == 1);

            Assert.Equal("Logo", messages[0].Caption);
            Assert.Equal("Bot", messages[1].Caption);

            _classFixture.Entities = messages.ToList();
        }

        [OrderedFact("Should send an album with 3 photos using their file_id")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMediaGroup)]
        public async Task Should_Send_3_Photos_Album_Using_FileId()
        {
            // Take file_id of photos uploaded in previous test case
            string[] fileIds = _classFixture.Entities
                .Select(msg => msg.Photo!.First().FileId)
                .ToArray();

            Message[] messages = await BotClient.SendMediaGroupAsync(
                 chatId: _fixture.SupergroupChat.Id,
                 media: new []
                {
                    new InputMediaPhoto(media: fileIds[0]),
                    new InputMediaPhoto(media: fileIds[1]),
                    new InputMediaPhoto(media: fileIds[0]),
                }
            );

            Assert.Equal(3, messages.Length);
            Assert.All(messages, msg => Assert.Equal(MessageType.Photo, msg.Type));
        }

        [OrderedFact("Should send an album using HTTP urls in reply to 1st album message")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMediaGroup)]
        public async Task Should_Send_Photo_Album_Using_Url()
        {
            int replyToMessageId = _classFixture.Entities.First().MessageId;

            Message[] messages = await BotClient.SendMediaGroupAsync(
                chatId: _fixture.SupergroupChat.Id,
                media: new []
                {
                    new InputMediaPhoto(
                        media: "https://cdn.pixabay.com/photo/2017/06/20/19/22/fuchs-2424369_640.jpg"
                    ),
                    new InputMediaPhoto(
                        media: "https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg"
                    ),
                },
                replyToMessageId: replyToMessageId
            );

            Assert.Equal(2, messages.Length);
            Assert.All(messages, msg => Assert.Equal(MessageType.Photo, msg.Type));
            Assert.All(messages, msg => Assert.Equal(replyToMessageId, msg.ReplyToMessage!.MessageId));
        }

        [OrderedFact("Should upload 2 videos and a photo with captions and send them in an album")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMediaGroup)]
        public async Task Should_Upload_2_Videos_Album()
        {
            await using Stream stream0 = System.IO.File.OpenRead(Constants.PathToFile.Videos.GoldenRatio);
            await using Stream stream1 = System.IO.File.OpenRead(Constants.PathToFile.Videos.MoonLanding);
            await using Stream stream2 = System.IO.File.OpenRead(Constants.PathToFile.Photos.Bot);

            IAlbumInputMedia[] media =
            {
                new InputMediaVideo(new InputMedia(content: stream0, fileName: "GoldenRatio.mp4"))
                {
                    Caption = "Golden Ratio",
                    Height = 240,
                    Width = 240,
                    Duration = 28,
                },
                new InputMediaVideo(new InputMedia(content: stream1, fileName: "MoonLanding.mp4"))
                {
                    Caption = "Moon Landing"
                },
                new InputMediaPhoto(new InputMedia(content: stream2, fileName: "bot.gif"))
                {
                    Caption = "Bot"
                },
            };

            Message[] messages = await BotClient.SendMediaGroupAsync(
                chatId: _fixture.SupergroupChat.Id,
                media: media
            );

            Assert.Equal(3, messages.Length);

            Assert.Equal(MessageType.Video, messages[0].Type);
            Assert.Equal("Golden Ratio", messages[0].Caption);
            Assert.Equal(240, messages[0].Video!.Width);
            Assert.Equal(240, messages[0].Video.Height);
            Assert.InRange(messages[0].Video.Duration, 28 - 2, 28 + 2);

            Assert.Equal(MessageType.Video, messages[1].Type);
            Assert.Equal("Moon Landing", messages[1].Caption);

            Assert.Equal(MessageType.Photo, messages[2].Type);
            Assert.Equal("Bot", messages[2].Caption);
        }

        [OrderedFact("Should upload 2 photos with markdown encoded captions and send them in an album")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMediaGroup)]
        public async Task Should_Upload_2_Photos_Album_With_Markdown_Encoded_Captions()
        {
            await using Stream stream1 = System.IO.File.OpenRead(Constants.PathToFile.Photos.Logo);
            await using Stream stream2 = System.IO.File.OpenRead(Constants.PathToFile.Photos.Bot);

            IAlbumInputMedia[] media =
            {
                new InputMediaPhoto(new InputMedia(content: stream1, fileName: "logo.png"))
                {
                    Caption = "*Logo*",
                    ParseMode = ParseMode.Markdown
                },
                new InputMediaPhoto(new InputMedia(content: stream2, fileName: "bot.gif"))
                {
                    Caption = "_Bot_",
                    ParseMode = ParseMode.Markdown
                },
            };

            Message[] messages = await BotClient.SendMediaGroupAsync(
                chatId: _fixture.SupergroupChat.Id,
                media: media
            );

            Assert.Equal("Logo", messages[0].CaptionEntityValues!.Single());
            Assert.Equal(MessageEntityType.Bold, messages[0].CaptionEntities!.Single().Type);

            Assert.Equal("Bot", messages[1].CaptionEntityValues!.Single());
            Assert.Equal(MessageEntityType.Italic, messages[1].CaptionEntities!.Single().Type);
        }

        [OrderedFact("Should send a video with thumbnail in an album")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendMediaGroup)]
        public async Task Should_Video_With_Thumbnail_In_Album()
        {
            await using Stream stream1 = System.IO.File.OpenRead(Constants.PathToFile.Videos.GoldenRatio);
            await using Stream stream2 = System.IO.File.OpenRead(Constants.PathToFile.Thumbnail.Video);

            IAlbumInputMedia[] media =
            {
                new InputMediaVideo(new InputMedia(content: stream1, fileName: "GoldenRatio.mp4"))
                {
                    Thumb = new InputMedia(content: stream2, fileName: "thumbnail.jpg"),
                    SupportsStreaming = true,
                },
                new InputMediaPhoto(
                    media: "https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg"
                ),
            };

            Message[] messages = await BotClient.SendMediaGroupAsync(
                chatId: _fixture.SupergroupChat.Id,
                media: media
            );

            Assert.Equal(MessageType.Video, messages[0].Type);
            Assert.NotNull(messages[0].Video!.Thumb);
        }
    }
}
