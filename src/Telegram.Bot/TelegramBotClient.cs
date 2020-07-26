using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Helpers;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using File = Telegram.Bot.Types.File;

namespace Telegram.Bot
{
    /// <summary>
    /// A client to use the Telegram Bot API
    /// </summary>
    public class TelegramBotClient : ITelegramBotClient
    {
        private const string BaseUrl = "https://api.telegram.org/bot";
        private const string BaseFileUrl = "https://api.telegram.org/file/bot";

        private readonly string _baseRequestUrl;
        private readonly string _baseFileUrl;

        private readonly HttpClient _httpClient;

        private IExceptionParser _exceptionParser = new ExceptionParser();

        /// <inheritdoc/>
        public int BotId { get; }

        #region Config Properties

        /// <summary>
        /// Timeout for requests
        /// </summary>
        public TimeSpan Timeout
        {
            get => _httpClient.Timeout;
            set => _httpClient.Timeout = value;
        }

        /// <inheritdoc />
        public IExceptionParser ExceptionParser
        {
            get => _exceptionParser;
            set => _exceptionParser = value ?? throw new ArgumentNullException(nameof(value));
        }

        #endregion Config Properties

        #region Events

        /// <summary>
        /// Occurs before sending a request to API
        /// </summary>
        public event EventHandler<ApiRequestEventArgs>? MakingApiRequest;

        /// <summary>
        /// Occurs after receiving the response to an API request
        /// </summary>
        public event EventHandler<ApiResponseEventArgs>? ApiResponseReceived;

        #endregion

        /// <summary>
        /// Create a new <see cref="TelegramBotClient"/> instance.
        /// </summary>
        /// <param name="token">API token</param>
        /// <param name="httpClient">A custom <see cref="HttpClient"/></param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="token"/> format is invalid
        /// </exception>
        public TelegramBotClient(string token, HttpClient? httpClient = null)
        {
            if (token is null) throw new ArgumentNullException(nameof(token));

            string[] parts = token.Split(':');
            if (parts.Length > 1 && int.TryParse(parts[0], out var id))
            {
                BotId = id;
            }
            else
            {
                throw new ArgumentException(
                    "Invalid format. A valid token looks like \"1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy\".",
                    nameof(token)
                );
            }

            _baseRequestUrl = $"{BaseUrl}{token}/";
            _baseFileUrl = $"{BaseFileUrl}{token}/";
            _httpClient = httpClient ?? new HttpClient();
        }

        #region Helpers

        /// <inheritdoc />
        public async Task<TResult> MakeRequestAsync<TResult>(
            IRequest<TResult> request,
            CancellationToken cancellationToken = default)
        {
            var url = new Uri($"{_baseRequestUrl}{request.MethodName}", UriKind.Absolute);
            var httpRequest = new HttpRequestMessage(request.Method, url)
            {
                Content = request.ToHttpContent()
            };

            ApiRequestEventArgs? requestEventArgs = default;

            if (MakingApiRequest != null)
            {
                requestEventArgs = new ApiRequestEventArgs(
                    request.MethodName,
                    httpRequest.Content
                );
                MakingApiRequest.Invoke(this, requestEventArgs);
            }

            HttpResponseMessage? httpResponse;
            try
            {
                httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException exception)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }

                throw new RequestException("Request timed out", exception);
            }
            catch (Exception exception)
            {
                throw new RequestException("Exception during making request", exception);
            }

            try
            {
                if (ApiResponseReceived != null)
                {
                    requestEventArgs ??= new ApiRequestEventArgs(
                        request.MethodName,
                        httpRequest.Content
                    );
                    var responseEventArgs = new ApiResponseEventArgs(
                        httpResponse,
                        requestEventArgs
                    );
                    ApiResponseReceived.Invoke(this, responseEventArgs);
                }

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    var failedApiResponse = await httpResponse
                        .DeserializeContentAsync<ApiResponse>()
                        .ConfigureAwait(false);

                    throw ExceptionParser.Parse(failedApiResponse);
                }

                var successfulApiResponse = await httpResponse
                    .DeserializeContentAsync<SuccessfulApiResponse<TResult>>()
                    .ConfigureAwait(false);

                return successfulApiResponse.Result;
            }
            finally
            {
                httpResponse?.Dispose();
            }
        }

        /// <inheritdoc />
        public async Task<ApiResponse<TResult>> SendRequestAsync<TResult>(
            IRequest<TResult> request,
            CancellationToken cancellationToken = default)
        {
            var url = new Uri($"{_baseRequestUrl}{request.MethodName}", UriKind.Absolute);
            var httpRequest = new HttpRequestMessage(request.Method, url)
            {
                Content = request.ToHttpContent()
            };

            ApiRequestEventArgs? requestEventArgs = default;

            if (MakingApiRequest != null)
            {
                requestEventArgs = new ApiRequestEventArgs(
                    request.MethodName,
                    httpRequest.Content
                );
                MakingApiRequest.Invoke(this, requestEventArgs);
            }

            HttpResponseMessage? httpResponse;
            try
            {
                httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException exception)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }

                throw new RequestException("Request timed out", exception);
            }
            catch (Exception exception)
            {
                throw new RequestException("Exception during making request", exception);
            }

            try
            {
                if (ApiResponseReceived != null)
                {
                    requestEventArgs ??= new ApiRequestEventArgs(
                        request.MethodName,
                        httpRequest.Content
                    );
                    var responseEventArgs = new ApiResponseEventArgs(
                        httpResponse,
                        requestEventArgs
                    );
                    ApiResponseReceived.Invoke(this, responseEventArgs);
                }

                var apiResponse = await httpResponse
                    .DeserializeContentAsync<ApiResponse<TResult>>()
                    .ConfigureAwait(false);

                return apiResponse;
            }
            finally
            {
                httpResponse?.Dispose();
            }
        }

        /// <summary>
        /// Test the API token
        /// </summary>
        /// <returns><c>true</c> if token is valid</returns>
        public async Task<bool> TestApiAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await MakeRequestAsync(new GetMeRequest(), cancellationToken)
                    .ConfigureAwait(false);
                return true;
            }
            catch (ApiRequestException e)
                when (e.ErrorCode == 401)
            {
                return false;
            }
        }

        #endregion Helpers

        /// <inheritdoc />
        public async Task DownloadFileAsync(
            string filePath,
            Stream destination,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(filePath) || filePath.Length < 2)
            {
                throw new ArgumentException("Invalid file path", nameof(filePath));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var fileUri = new Uri($"{_baseFileUrl}{filePath}", UriKind.Absolute);

            HttpResponseMessage? httpResponse;
            try
            {
                httpResponse = await _httpClient
                    .GetAsync(fileUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (TaskCanceledException exception)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }

                throw new RequestException("Request timed out", exception);
            }
            catch (Exception exception)
            {
                throw new RequestException("Exception during file download", exception);
            }

            try
            {
                if (!httpResponse.IsSuccessStatusCode)
                {
                    var failedApiResponse = await httpResponse
                        .DeserializeContentAsync<ApiResponse>(includeBody: false)
                        .ConfigureAwait(false);

                    throw ExceptionParser.Parse(failedApiResponse);
                }

                if (httpResponse.Content is null)
                    throw new RequestException(
                        "Response doesn't contain any content",
                        httpResponse.StatusCode
                    );

                try
                {
                    await httpResponse.Content.CopyToAsync(destination)
                        .ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    throw new RequestException(
                        "Exception during file download",
                        httpResponse.StatusCode,
                        exception
                    );
                }
            }
            finally
            {
                httpResponse?.Dispose();
            }
        }

        /// <inheritdoc />
        public async Task<File> GetInfoAndDownloadFileAsync(
            string fileId,
            Stream destination,
            CancellationToken cancellationToken = default)
        {
            var file = await MakeRequestAsync(new GetFileRequest(fileId), cancellationToken)
                .ConfigureAwait(false);

            await DownloadFileAsync(file.FilePath!, destination, cancellationToken)
                .ConfigureAwait(false);

            return file;
        }
    }
}
