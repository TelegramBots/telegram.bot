// using System;
// using System.IO;
// using System.Threading;
// using System.Threading.Tasks;
// using Telegram.Bot.Args;
// using Telegram.Bot.Exceptions;
// using Telegram.Bot.Requests.Abstractions;
// using Telegram.Bot.Types;
// using Xunit.Abstractions;
// using Xunit.Sdk;
// using File = Telegram.Bot.Types.File;
//
// namespace Telegram.Bot.Tests.Integ.Framework
// {
//     public class RetryTelegramBotClient : ITelegramBotClient
//     {
//         private readonly ITelegramBotClient _botClient;
//         private readonly int _retryCount;
//         private readonly IMessageSink _messageSink;
//         private readonly int _defaultWaitTime;
//         public int BotId => _botClient.BotId;
//
//         public TimeSpan Timeout
//         {
//             get => _botClient.Timeout;
//             set => _botClient.Timeout = value;
//         }
//
//         public IExceptionParser ExceptionParser
//         {
//             get => _botClient.ExceptionParser;
//             set => _botClient.ExceptionParser = value;
//         }
//
//         public event EventHandler<ApiRequestEventArgs> MakingApiRequest
//         {
//             add => _botClient.MakingApiRequest += value;
//             remove => _botClient.MakingApiRequest -= value;
//         }
//
//         public event EventHandler<ApiResponseEventArgs> ApiResponseReceived
//         {
//             add => _botClient.ApiResponseReceived += value;
//             remove => _botClient.ApiResponseReceived -= value;
//         }
//
//         // 30 seconds are chosen because it's a an average amount of that has
//         // been seen in integration tests
//         public RetryTelegramBotClient(
//             ITelegramBotClient botClient,
//             int retryCount,
//             IMessageSink messageSink,
//             int defaultWaitTime = 30)
//         {
//             _botClient = botClient;
//             _retryCount = retryCount;
//             _messageSink = messageSink;
//             _defaultWaitTime = defaultWaitTime;
//         }
//
//         public async Task<TResult> MakeRequestAsync<TResult>(
//             IRequest<TResult> request,
//             CancellationToken cancellationToken = default)
//         {
//             ApiRequestException lastException = default;
//
//             for (var i = 0; i < _retryCount; i++)
//             {
//                 try
//                 {
//                     return await _botClient.MakeRequestAsync(request, cancellationToken);
//                 }
//                 catch (ApiRequestException exception) when (exception.ErrorCode == 429)
//                 {
//                     lastException = exception;
//
//                     var effectiveSeconds = exception.Parameters?.RetryAfter ?? _defaultWaitTime;
//                     _messageSink.OnMessage(
//                         new DiagnosticMessage(
//                             $"Request was rate limited. Retry attempt {i + 1}. Waiting for {effectiveSeconds} " +
//                             "seconds before retrying."
//                         )
//                     );
//
//                     var timeToWait = TimeSpan.FromSeconds(effectiveSeconds);
//                     await Task.Delay(timeToWait, cancellationToken);
//                 }
//             }
//
//             throw lastException!;
//         }
//
//         public async Task<ApiResponse<TResult>> SendRequestAsync<TResult>(
//             IRequest<TResult> request,
//             CancellationToken cancellationToken = default) =>
//             await _botClient.SendRequestAsync(request, cancellationToken);
//
//         public async Task<bool> TestApiAsync(CancellationToken cancellationToken = default) =>
//             await _botClient.TestApiAsync(cancellationToken);
//
//         public async Task DownloadFileAsync(
//             string filePath,
//             Stream destination,
//             CancellationToken cancellationToken = default) =>
//             await _botClient.DownloadFileAsync(filePath, destination, cancellationToken);
//
//         public async Task<File> GetInfoAndDownloadFileAsync(
//             string fileId,
//             Stream destination,
//             CancellationToken cancellationToken = default) =>
//             await _botClient.GetInfoAndDownloadFileAsync(fileId, destination, cancellationToken);
//     }
// }
