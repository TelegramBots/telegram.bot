using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Sending_Messages
{
    [Collection(Constants.TestCollections.SendAudioMessage)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class AudioMessageTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;
        private readonly TestsFixture _fixture;

        public AudioMessageTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should send an audio with caption")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendAudio)]
        public async Task Should_Send_Audio()
        {
            const string performer = "Jackson F. Smith";
            const string title = "Cantina Rag";
            const int duration = 201;
            const string caption = "Audio File in .mp3 format";

            Message message;
            await using (Stream stream = System.IO.File.OpenRead(Constants.PathToFile.Audio.CantinaRagMp3))
            {
                message = await BotClient.SendAudioAsync(
                    chatId: _fixture.SupergroupChat,
                    audio: stream,
                    caption: caption,
                    duration: duration,
                    performer: performer,
                    title: title);
            }

            Assert.Equal(MessageType.Audio, message.Type);
            Assert.Equal(caption, message.Caption);
            Assert.Equal(performer, message.Audio!.Performer);
            Assert.Equal(title, message.Audio.Title);
            Assert.Equal(duration, message.Audio.Duration);
            Assert.Equal("audio/mpeg", message.Audio.MimeType);
            Assert.NotEmpty(message.Audio.FileId);
            Assert.NotEmpty(message.Audio.FileUniqueId);
            Assert.True(message.Audio.FileSize > 200);
        }

        [OrderedFact("Should send an audio with its thumbnail")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendAudio)]
        public async Task Should_Send_Audio_With_Thumb()
        {
            // Both audio file and its thumbnail should be uploaded
            await using Stream stream1 = System.IO.File.OpenRead(Constants.PathToFile.Audio.AStateOfDespairMp3);
            await using Stream stream2 = System.IO.File.OpenRead(Constants.PathToFile.Thumbnail.TheAbilityToBreak);

            Message message = await BotClient.SendAudioAsync(
                chatId: _fixture.SupergroupChat,
                audio: new InputMedia(content: stream1, fileName: "Ask Again - A State of Despair.mp3"),
                thumb: new InputMedia(content: stream2, fileName: "thumb.jpg")
            );

            Assert.NotNull(message.Audio!.Thumb);
            Assert.NotEmpty(message.Audio.Thumb.FileId);
            Assert.NotEmpty(message.Audio.Thumb.FileUniqueId);
            Assert.Equal(90, message.Audio.Thumb.Height);
            Assert.Equal(90, message.Audio.Thumb.Width);
            Assert.True(message.Audio.Thumb.FileSize > 10_000);
        }

        [OrderedFact("Should send a voice with caption")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendVoice)]
        public async Task Should_Send_Voice()
        {
            int duration = 24;
            string caption = "Test Voice in .ogg format";

            await using Stream stream = System.IO.File.OpenRead(Constants.PathToFile.Audio.TestOgg);

            Message message = await BotClient.SendVoiceAsync(
                chatId: _fixture.SupergroupChat,
                voice: stream,
                caption: caption,
                duration: duration
            );

            Assert.Equal(MessageType.Voice, message.Type);
            Assert.Equal(caption, message.Caption);
            Assert.Equal(duration, message.Voice!.Duration);
            Assert.Equal("audio/ogg", message.Voice.MimeType);
            Assert.NotEmpty(message.Voice.FileId);
            Assert.NotEmpty(message.Voice.FileUniqueId);
            Assert.True(message.Voice.FileSize > 200);
        }
    }
}
