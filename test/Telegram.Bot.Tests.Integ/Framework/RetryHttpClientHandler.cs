using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Telegram.Bot.Tests.Integ.Framework
{
    internal class RetryHttpClientHandler : HttpClientHandler
    {
        private readonly int _retryCount;
        private readonly IMessageSink _diagnosticMessageSink;

        internal RetryHttpClientHandler(int retryCount, IMessageSink diagnosticMessageSink)
        {
            _retryCount = retryCount;
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponseMessage = default;

            for (var i = 0; i < _retryCount; i++)
            {
                httpResponseMessage = await base.SendAsync(request, cancellationToken);

                if (httpResponseMessage.StatusCode != (HttpStatusCode)429)
                {
                    break;
                }

                _diagnosticMessageSink.OnMessage(new DiagnosticMessage("Request was rate limited"));

                var body = await httpResponseMessage.Content.ReadAsStringAsync();

                // Deserializing with an arbitrary type parameter since Result property should
                // be empty at this stage
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<int>>(body);

                if (apiResponse.Parameters != null)
                {
                    var seconds = apiResponse.Parameters.RetryAfter;
                    // 30 seconds are chosen because it's a an average amount of that has
                    // been seen in integration tests
                    int effectiveSeconds = seconds ?? 30;

                    _diagnosticMessageSink.OnMessage(
                        new DiagnosticMessage(
                            $"Retry attempt {i + 1}. Waiting for {effectiveSeconds} seconds before retrying."
                        )
                    );

                    var timeToWait = TimeSpan.FromSeconds(effectiveSeconds);
                    await Task.Delay(timeToWait, cancellationToken);
                }
            }

            return httpResponseMessage;
        }
    }
}
