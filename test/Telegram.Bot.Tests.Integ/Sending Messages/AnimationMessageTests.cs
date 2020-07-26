using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Sending_Messages
{
    [Collection(Constants.TestCollections.SendAnimationMessage)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class AnimationMessageTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public AnimationMessageTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should send an animation with caption")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendAnimation)]
        public async Task Should_Send_Animation()
        {
            await using Stream stream = System.IO.File.OpenRead(Constants.PathToFile.Animation.Earth);
            Message message = await BotClient.SendAnimationAsync(
                chatId: _fixture.SupergroupChat.Id,
                animation: stream,
                duration: 4,
                width: 400,
                height: 400,
                thumb: null,
                caption: "<b>Rotating</b> <i>Earth</i>",
                parseMode: ParseMode.Html
            );

            // For backwards compatibility, message type is set to Document
            Assert.Equal(MessageType.Document, message.Type);
            Assert.NotNull(message.Document);
            Assert.NotNull(message.Animation);

            Assert.Equal("Rotating Earth", message.Caption);
            Assert.Equal(2, message.CaptionEntities!.Length);
            Assert.Equal(2, message.CaptionEntityValues!.Count());

            Assert.Equal(4, message.Animation.Duration);
            Assert.Equal(400, message.Animation.Width);
            Assert.Equal(400, message.Animation.Height);
            Assert.Equal("video/mp4", message.Animation.MimeType);
            Assert.NotEmpty(message.Animation.FileId);
            Assert.NotEmpty(message.Animation.FileUniqueId);
            Assert.NotNull(message.Animation.FileName);
            Assert.NotEmpty(message.Animation.FileName);
            Assert.True(message.Animation.FileSize > 80_000);
        }

        [OrderedFact("Should send an animation with thumbnail")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendAnimation)]
        public async Task Should_Send_Animation_With_Thumb()
        {
            await using Stream stream1 = System.IO.File.OpenRead(Constants.PathToFile.Animation.Earth);
            await using Stream stream2 = System.IO.File.OpenRead(Constants.PathToFile.Thumbnail.TheAbilityToBreak);

            Message message = await BotClient.SendAnimationAsync(
                chatId: _fixture.SupergroupChat,
                animation: new InputMedia(stream1, "earth.gif"),
                thumb: new InputMedia(stream2, "thumb.jpg")
            );

            Assert.NotNull(message.Animation);
            Assert.NotNull(message.Animation.Thumb);
            Assert.NotEmpty(message.Animation.Thumb.FileId);
            Assert.NotEmpty(message.Animation.Thumb.FileUniqueId);
            Assert.Equal(320, message.Animation.Thumb.Height);
            Assert.Equal(320, message.Animation.Thumb.Width);
            Assert.True(message.Animation.Thumb.FileSize > 10_000);
        }
    }
}
