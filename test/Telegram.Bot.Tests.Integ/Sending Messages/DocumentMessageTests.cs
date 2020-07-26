using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Sending_Messages
{
    [Collection(Constants.TestCollections.SendDocumentMessage)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class SendingDocumentMessageTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public SendingDocumentMessageTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should send a pdf document with caption")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendDocument)]
        public async Task Should_Send_Pdf_Document()
        {
            await using Stream stream = System.IO.File.OpenRead(Constants.PathToFile.Documents.Hamlet);

            Message message = await BotClient.SendDocumentAsync(
                chatId: _fixture.SupergroupChat.Id,
                document: new InputOnlineFile(content: stream, fileName: "HAMLET.pdf"),
                caption: "The Tragedy of Hamlet,\nPrince of Denmark"
            );

            Assert.Equal(MessageType.Document, message.Type);
            Assert.Equal("HAMLET.pdf", message.Document!.FileName);
            Assert.Equal("application/pdf", message.Document.MimeType);
            Assert.NotNull(message.Document.FileSize);
            Assert.InRange(message.Document.FileSize.Value, 253_000, 257_000);
            Assert.NotEmpty(message.Document.FileId);
            Assert.NotEmpty(message.Document.FileUniqueId);
            Assert.Equal("The Tragedy of Hamlet,\nPrince of Denmark", message.Caption);
        }

        // ReSharper disable StringLiteralTypo
        [OrderedFact("Should send a pdf document having a Farsi(non-ASCII) file name")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendDocument)]
        public async Task Should_Send_Document_With_Farsi_Name()
        {
            await using Stream stream = System.IO.File.OpenRead(Constants.PathToFile.Documents.Hamlet);

            Message message = await BotClient.SendDocumentAsync(
                chatId: _fixture.SupergroupChat.Id,
                document: new InputOnlineFile(content: stream, fileName: "هملت.pdf"),
                caption: "تراژدی هملت\nشاهزاده دانمارک"
            );

            Assert.Equal(MessageType.Document, message.Type);
            Assert.NotNull(message.Document);
            Assert.Equal("هملت.pdf", message.Document.FileName);
            Assert.Equal("application/pdf", message.Document.MimeType);
            Assert.NotNull(message.Document.FileSize);
            Assert.InRange(message.Document.FileSize.Value, 253_000, 257_000);
            Assert.NotEmpty(message.Document.FileId);
            Assert.NotEmpty(message.Document.FileUniqueId);
            Assert.Equal("تراژدی هملت\nشاهزاده دانمارک", message.Caption);
        }

        [OrderedFact("Should send a pdf document with thumbnail")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendDocument)]
        public async Task Should_Send_Document_With_Thumb()
        {
            await using Stream stream1 = System.IO.File.OpenRead(Constants.PathToFile.Documents.Hamlet);
            await using Stream stream2 = System.IO.File.OpenRead(Constants.PathToFile.Thumbnail.TheAbilityToBreak);

            Message message = await BotClient.SendDocumentAsync(
                chatId: _fixture.SupergroupChat,
                document: new InputMedia(content: stream1, fileName: "Hamlet.pdf"),
                thumb: new InputMedia(content: stream2, fileName: "thumb.jpg")
            );

            Assert.NotNull(message.Document);
            Assert.NotNull(message.Document.Thumb);
            Assert.NotEmpty(message.Document.Thumb.FileId);
            Assert.NotEmpty(message.Document.Thumb.FileUniqueId);
            Assert.Equal(90, message.Document.Thumb.Height);
            Assert.Equal(90, message.Document.Thumb.Width);
            Assert.NotNull(message.Document.Thumb.FileSize);
            Assert.InRange(message.Document.Thumb.FileSize.Value, 11_000, 12_000);

            Assert.Equal(MessageType.Document, message.Type);
            Assert.Equal("Hamlet.pdf", message.Document.FileName);
            Assert.Equal("application/pdf", message.Document.MimeType);
            Assert.NotNull(message.Document.FileSize);
            Assert.InRange(message.Document.FileSize.Value, 253_000, 257_000);
            Assert.NotEmpty(message.Document.FileId);
            Assert.NotEmpty(message.Document.FileUniqueId);
            Assert.Null(message.Caption);
        }
    }
}
