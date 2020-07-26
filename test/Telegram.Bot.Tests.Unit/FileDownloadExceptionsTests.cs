using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Exceptions;
using Xunit;

namespace Telegram.Bot.Tests.Unit
{
    public class FileDownloadExceptionsTests
    {
        [Fact]
        public async Task Should_Throw_RequestException_On_Incorrect_Proxy()
        {
            var httpClientHandler = new HttpClientHandler
            {
                Proxy = new WebProxy("http://proxy.test")
            };
            var httpClient = new HttpClient(httpClientHandler);
            var botClient = new TelegramBotClient(
                "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy",
                httpClient
            );

            await using var memoryStream = new MemoryStream();

            var requestException =  await Assert.ThrowsAsync<RequestException>(
                async () => await botClient.DownloadFileAsync(
                    filePath: "file/path",
                    destination: memoryStream
                )
            );

            Assert.Null(requestException.HttpStatusCode);
            Assert.Null(requestException.Body);
            Assert.Equal("Exception during file download", requestException.Message);
            Assert.Equal(0, memoryStream.Length);
            Assert.Equal(0, memoryStream.Position);

            Assert.NotNull(requestException.InnerException);
            Assert.IsType<HttpRequestException>(requestException.InnerException);
        }

        [Fact]
        public async Task Should_Throw_TaskCancelledException_On_Cancelled_Token()
        {
            var botClient = new TelegramBotClient("1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy");

            using var cts = new CancellationTokenSource();
            var token = cts.Token;
            cts.Cancel();

            await using var memoryStream = new MemoryStream();

            await Assert.ThrowsAsync<TaskCanceledException>(
                async () => await botClient.DownloadFileAsync(
                    filePath: "file/path",
                    destination: memoryStream,
                    cancellationToken: token
                )
            );

            Assert.Equal(0, memoryStream.Length);
            Assert.Equal(0, memoryStream.Position);
        }

        [Fact]
        public async Task Should_Throw_RequestException_On_Timed_Out_Request()
        {
            var httpClientHandler = new MockHttpMessageHandler(
                _ => throw new TaskCanceledException()
            );
            var httpClient = new HttpClient(httpClientHandler);
            var botClient = new TelegramBotClient(
                "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy",
                httpClient
            );

            await using var memoryStream = new MemoryStream();

            var requestException = await Assert.ThrowsAsync<RequestException>(
                async () => await botClient.DownloadFileAsync(
                    filePath: "file/path",
                    destination: memoryStream
                )
            );

            Assert.NotNull(requestException.InnerException);
            Assert.IsType<TaskCanceledException>(requestException.InnerException);
            Assert.Equal("Request timed out", requestException.Message);
            Assert.Equal(0, memoryStream.Length);
            Assert.Equal(0, memoryStream.Position);
        }

        [Fact]
        public async Task Should_Throw_RequestException_With_Null_Response_Content()
        {
            var httpClientHandler = new MockHttpMessageHandler(HttpStatusCode.OK);
            var httpClient = new HttpClient(httpClientHandler);

            var botClient = new TelegramBotClient(
                "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy",
                httpClient
            );

            await using var memoryStream = new MemoryStream();

            var requestException = await Assert.ThrowsAsync<RequestException>(
                async () => await botClient.DownloadFileAsync(
                    filePath: "file/path",
                    destination: memoryStream
                )
            );

            Assert.Equal(HttpStatusCode.OK, requestException.HttpStatusCode);
            Assert.Null(requestException.Body);
            Assert.NotNull(requestException.Message);
            Assert.Equal("Response doesn't contain any content", requestException.Message);
            Assert.Null(requestException.InnerException);
        }

        [Fact]
        public async Task Should_Throw_RequestException_With_JsonSerializationException_On_Failed_Response()
        {
            var httpClientHandler = new MockHttpMessageHandler(
                HttpStatusCode.NotFound,
                new StringContent("{}")
            );

            var httpClient = new HttpClient(httpClientHandler);
            var botClient = new TelegramBotClient(
                "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy",
                httpClient
            );

            await using var memoryStream = new MemoryStream();

            var requestException = await Assert.ThrowsAsync<RequestException>(
                async () => await botClient.DownloadFileAsync(
                    filePath: "file/path",
                    destination: memoryStream
                )
            );

            Assert.Equal(HttpStatusCode.NotFound, requestException.HttpStatusCode);
            Assert.Null(requestException.Body);
            Assert.Null(requestException.Body);

            Assert.NotNull(requestException.InnerException);
            Assert.IsType<JsonSerializationException>(requestException.InnerException);
        }

        [Fact]
        public async Task Should_Throw_ApiRequestException_With_JsonSerializationException_On_Failed_Response()
        {
            var httpClientHandler = new MockHttpMessageHandler(
                HttpStatusCode.NotFound,
                new StringContent(@"{""ok"":false,""description"":""Not Found"",""error_code"":404}")
            );

            var httpClient = new HttpClient(httpClientHandler);
            var botClient = new TelegramBotClient(
                "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy",
                httpClient
            );

            var apiRequestException =  await Assert.ThrowsAsync<ApiRequestException>(
                async () => await botClient.GetUpdatesAsync()
            );

            Assert.Equal(404, apiRequestException.ErrorCode);
            Assert.Equal("Not Found", apiRequestException.Message);
        }
    }
}
