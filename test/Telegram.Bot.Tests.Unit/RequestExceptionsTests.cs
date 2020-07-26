using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Xunit;

namespace Telegram.Bot.Tests.Unit
{
    public class RequestExceptionsTests
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

            var requestException =  await Assert.ThrowsAsync<RequestException>(
                async () => await botClient.GetUpdatesAsync()
            );

            Assert.NotNull(requestException.InnerException);
            Assert.Null(requestException.HttpStatusCode);
            Assert.Null(requestException.Body);

            Assert.IsType<HttpRequestException>(requestException.InnerException);
        }

        [Fact]
        public async Task Should_Throw_TaskCancelledException_On_Cancelled_Token()
        {
            var botClient = new TelegramBotClient("1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy");

            using var cts = new CancellationTokenSource();
            var token = cts.Token;
            cts.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(
                async () => await botClient.GetUpdatesAsync(cancellationToken: token)
            );
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

            var requestException = await Assert.ThrowsAsync<RequestException>(
                async () => await botClient.GetUpdatesAsync()
            );

            Assert.NotNull(requestException.InnerException);
            Assert.IsType<TaskCanceledException>(requestException.InnerException);
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

            var requestException =  await Assert.ThrowsAsync<RequestException>(
                async () => await botClient.GetUpdatesAsync()
            );

            Assert.Equal(HttpStatusCode.OK, requestException.HttpStatusCode);
            Assert.Null(requestException.Body);
            Assert.Null(requestException.InnerException);
        }

        [Fact]
        public async Task Should_Throw_RequestException_Due_To_Empty_Or_Null_Content()
        {
            var httpClientHandler = new MockHttpMessageHandler(
                HttpStatusCode.OK,
                new StringContent(string.Empty)
            );

            var httpClient = new HttpClient(httpClientHandler);
            var botClient = new TelegramBotClient(
                "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy",
                httpClient
            );

            var requestException =  await Assert.ThrowsAsync<RequestException>(
                async () => await botClient.GetUpdatesAsync()
            );

            Assert.Equal(HttpStatusCode.OK, requestException.HttpStatusCode);
            Assert.NotNull(requestException.Body);
            Assert.Empty(requestException.Body);

            Assert.Null(requestException.InnerException);
        }

        [Fact]
        public async Task Should_Throw_RequestException_With_JsonSerializationException_On_Successful_Response()
        {
            var httpClientHandler = new MockHttpMessageHandler(
                HttpStatusCode.OK,
                new StringContent("{}")
            );

            var httpClient = new HttpClient(httpClientHandler);
            var botClient = new TelegramBotClient(
                "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy",
                httpClient
            );

            var requestException =  await Assert.ThrowsAsync<RequestException>(
                async () => await botClient.GetUpdatesAsync()
            );

            Assert.Equal(HttpStatusCode.OK, requestException.HttpStatusCode);
            Assert.NotNull(requestException.Body);
            Assert.Equal("{}", requestException.Body);

            Assert.NotNull(requestException.InnerException);
            Assert.IsType<JsonSerializationException>(requestException.InnerException);
        }

        [Fact]
        public async Task Should_Throw_RequestException_With_JsonSerializationException_On_Failed_Response()
        {
            var httpClientHandler = new MockHttpMessageHandler(
                HttpStatusCode.BadRequest,
                new StringContent("{}")
            );

            var httpClient = new HttpClient(httpClientHandler);
            var botClient = new TelegramBotClient(
                "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy",
                httpClient
            );

            var requestException =  await Assert.ThrowsAsync<RequestException>(
                async () => await botClient.GetUpdatesAsync()
            );

            Assert.Equal(HttpStatusCode.BadRequest, requestException.HttpStatusCode);
            Assert.NotNull(requestException.Body);
            Assert.Equal("{}", requestException.Body);

            Assert.NotNull(requestException.InnerException);
            Assert.IsType<JsonSerializationException>(requestException.InnerException);
        }

        [Fact]
        public async Task Should_Throw_ApiRequestException_With_JsonSerializationException_On_Failed_Response()
        {
            var httpClientHandler = new MockHttpMessageHandler(
                HttpStatusCode.BadRequest,
                new StringContent(@"{""ok"":false,""description"":""Bad Request: chat_id is incorrect"",""error_code"":400}")
            );

            var httpClient = new HttpClient(httpClientHandler);
            var botClient = new TelegramBotClient(
                "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy",
                httpClient
            );

            var apiRequestException =  await Assert.ThrowsAsync<ApiRequestException>(
                async () => await botClient.GetUpdatesAsync()
            );

            Assert.Equal(400, apiRequestException.ErrorCode);
            Assert.Equal("Bad Request: chat_id is incorrect", apiRequestException.Message);
        }

        [Fact]
        public async Task Should_Pass_Same_Instance_Of_Request_Event_Args()
        {
            var httpClientHandler = new MockHttpMessageHandler(
                HttpStatusCode.OK,
                new StringContent(@"{""ok"":true,""result"":[]}}")
            );

            var httpClient = new HttpClient(httpClientHandler);
            var botClient = new TelegramBotClient(
                "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy",
                httpClient
            );

            ApiRequestEventArgs requestEventArgs1 = default;
            ApiRequestEventArgs requestEventArgs2 = default;

            botClient.MakingApiRequest += (sender, args) => requestEventArgs1 = args;
            botClient.ApiResponseReceived += (sender, args) => requestEventArgs2 = args.ApiRequestEventArgs;

            await botClient.GetUpdatesAsync();

            Assert.NotNull(requestEventArgs1);
            Assert.NotNull(requestEventArgs2);
            Assert.Equal(requestEventArgs1, requestEventArgs2);
        }
    }
}
