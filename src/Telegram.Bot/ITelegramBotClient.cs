using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using File = Telegram.Bot.Types.File;

namespace Telegram.Bot
{
    /// <summary>
    /// A client interface to use the Telegram Bot API
    /// </summary>
    public interface ITelegramBotClient
    {
        /// <summary>
        /// Unique identifier for the bot from bot token. For example, for the bot token
        /// "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy", the bot id is "1234567".
        /// </summary>
        int BotId { get; }

        #region Config Properties

        /// <summary>
        /// Timeout for requests
        /// </summary>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// Instance of <see cref="IExceptionParser"/> to parse errors from Bot API into
        /// <see cref="ApiRequestException"/>
        /// </summary>
        /// <remarks>This property is not thread safe</remarks>
        IExceptionParser ExceptionParser { get; set; }

        #endregion  Config Properties

        #region Events

        /// <summary>
        /// Occurs before sending a request to API
        /// </summary>
        event EventHandler<ApiRequestEventArgs> MakingApiRequest;

        /// <summary>
        /// Occurs after receiving the response to an API request
        /// </summary>
        /// <remarks>
        /// Don't mutate <see cref="HttpResponseMessage"/> received from <see cref="ApiResponseEventArgs"/>. If you
        /// render it unserializable <see cref="RequestException"/> will be thrown.
        /// </remarks>
        event EventHandler<ApiResponseEventArgs> ApiResponseReceived;

        #endregion Events

        #region Helpers

        /// <summary>
        /// Send a request to Bot API
        /// </summary>
        /// <typeparam name="TResult">Type of expected result in the response object</typeparam>
        /// <param name="request">API request object</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result of the API request</returns>
        /// <exception cref="ApiRequestException">
        /// Thrown when the response contains a valid JSON string with an error and description
        /// </exception>
        /// <exception cref="RequestException">
        /// Thrown when the response doesn't contain valid JSON string or on any other exception
        /// </exception>
        /// <exception cref="TaskCanceledException">
        /// Thrown when cancellation is triggered
        /// </exception>
        Task<TResult> MakeRequestAsync<TResult>(
            IRequest<TResult> request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a request to Bot API
        /// </summary>
        /// <typeparam name="TResult">Type of expected result in the response object</typeparam>
        /// <param name="request">API request object</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Result of the API request</returns>
        /// <returns>
        /// <see cref="ApiResponse{TResult}"/> instance or <c>null</c> if there isn't any response
        /// </returns>
        /// <exception cref="RequestException">
        /// Thrown when the response doesn't contain valid JSON string or on any other exception
        /// </exception>
        /// <exception cref="TaskCanceledException">
        /// Thrown when cancellation is triggered
        /// </exception>
        Task<ApiResponse<TResult>> SendRequestAsync<TResult>(
            IRequest<TResult> request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Test the API token
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns><c>true</c> if token is valid</returns>
        Task<bool> TestApiAsync(CancellationToken cancellationToken = default);

        #endregion Helpers

        /// <summary>
        /// Use this method to download a file. Get <paramref name="filePath"/> by sending
        /// <see cref="Telegram.Bot.Requests.GetFileRequest"/>
        /// </summary>
        /// <param name="filePath">Path to file on server</param>
        /// <param name="destination">Destination stream to write file to</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="filePath"/> is null or invalid
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="destination"/> is <c>null</c>
        /// </exception>
        /// <exception cref="ApiRequestException">
        /// Thrown when error response contains a valid JSON string with an error and description
        /// </exception>
        /// <exception cref="RequestException">
        /// Thrown when response is not successful
        /// </exception>
        /// <exception cref="TaskCanceledException">
        /// Thrown when cancellation is requested
        /// </exception>
        Task DownloadFileAsync(
            string filePath,
            Stream destination,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Use this method to get basic info about a file and download it.
        /// </summary>
        /// <param name="fileId">File identifier to get info about</param>
        /// <param name="destination">Destination stream to write file to</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice
        /// of cancellation.
        /// </param>
        /// <returns>File info</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when file path received from Bot API is <c>null</c> or invalid
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="destination"/> is <c>null</c>
        /// </exception>
        /// <exception cref="ApiRequestException">
        /// Thrown when error response contains a valid JSON string with an error and description
        /// </exception>
        /// <exception cref="RequestException">
        /// Thrown when the response is not successful
        /// </exception>
        /// <exception cref="TaskCanceledException">
        /// Thrown when cancellation is requested
        /// </exception>
        Task<File> GetInfoAndDownloadFileAsync(
            string fileId,
            Stream destination,
            CancellationToken cancellationToken = default);
    }
}
