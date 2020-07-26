using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Xunit;
using Xunit.Abstractions;
using File = Telegram.Bot.Types.File;

namespace Telegram.Bot.Tests.Integ.Other
{
    [Collection(Constants.TestCollections.FileDownload)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class FileDownloadTests : IClassFixture<FileDownloadTests.Fixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly Fixture _classFixture;
        private readonly TestsFixture _fixture;

        private ITelegramBotClient BotClient => _fixture.BotClient;

        public FileDownloadTests(TestsFixture fixture, Fixture classFixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _classFixture = classFixture;
            _output = output;
        }

        [OrderedFact("Should get file info")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetFile)]
        public async Task Should_Get_File_Info()
        {
            int fileSize = 253736;

            #region Send Document

            await using Stream stream = System.IO.File.OpenRead(Constants.PathToFile.Documents.Hamlet);

            Message documentMessage = await BotClient.SendDocumentAsync(
                chatId: _fixture.SupergroupChat,
                document: stream
            );

            string fileId = documentMessage.Document!.FileId;

            #endregion

            File file = await BotClient.GetFileAsync(documentMessage.Document.FileId);

            Assert.Equal(fileId, file.FileId);
            Assert.NotNull(file.FileSize);
            Assert.InRange(file.FileSize.Value, fileSize - 3500, fileSize + 3500);
            Assert.NotNull(file.FilePath);
            Assert.NotEmpty(file.FilePath);

            _classFixture.File = file;
        }

        [OrderedFact("Should download file using file_path")]
        public async Task Should_Download_Using_FilePath()
        {
            int fileSize = _classFixture.File.FileSize!.Value;

            await using MemoryStream destinationStream = new MemoryStream();

            await BotClient.DownloadFileAsync(
                filePath: _classFixture.File.FilePath!,
                destination: destinationStream
            );

            Assert.InRange(destinationStream.Length, fileSize - 100, fileSize + 100);
        }

        [OrderedFact("Should download file using file_path and write it to disk")]
        public async Task Should_Download_Write_Using_FilePath()
        {
            int fileSize = _classFixture.File.FileSize!.Value;

            string destinationFilePath = $"{Path.GetTempFileName()}.{Fixture.FileType}";
            _output.WriteLine($@"Writing file to ""{destinationFilePath}""");

            await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);

            await BotClient.DownloadFileAsync(
                filePath: _classFixture.File.FilePath!,
                destination: fileStream
            );

            Assert.InRange(fileStream.Length, fileSize - 100, fileSize + 100);
        }

        [OrderedFact("Should download file using file_id and write it to disk")]
        public async Task Should_Download_Write_Using_FileId()
        {
            int fileSize = _classFixture.File.FileSize!.Value;

            string destinationFilePath = $"{Path.GetTempFileName()}.{Fixture.FileType}";
            _output.WriteLine($@"Writing file to ""{destinationFilePath}""");

            await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
            File file = await BotClient.GetInfoAndDownloadFileAsync(
                fileId: _classFixture.File.FileId,
                destination: fileStream
            );

            Assert.InRange(fileStream.Length, fileSize - 100, fileSize + 100);
            Assert.True(JToken.DeepEquals(
                JToken.FromObject(_classFixture.File),
                JToken.FromObject(file)
            ));
        }

        [OrderedFact("Should throw ApiRequestException while trying to get file using wrong file_id")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetFile)]
        public async Task Should_Throw_ApiRequestException_When_Invalid_FileId()
        {
            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(
                async () => await BotClient.GetFileAsync(fileId: "Invalid_File_id")
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Contains("file_id", exception.Message);
        }

        [OrderedFact("Should throw ApiRequestException while trying to download file using wrong file_path")]
        public async Task Should_Throw_FilePath_HttpRequestException()
        {
            await using MemoryStream destinationStream = new MemoryStream();

            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(
                async () => await BotClient.DownloadFileAsync(
                    filePath: "Invalid_File_Path",
                    destination: destinationStream
                )
            );

            Assert.Equal(0, destinationStream.Length);
            Assert.Equal(0, destinationStream.Position);

            Assert.Equal(404, exception.ErrorCode);
            Assert.Contains("Not Found", exception.Message);
        }

        public class Fixture
        {
            public static string FileType = "pdf";

            public File File { get; set; }
        }
    }
}
