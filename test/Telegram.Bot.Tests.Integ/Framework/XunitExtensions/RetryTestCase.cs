using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Telegram.Bot.Tests.Integ.Framework.XunitExtensions
{
    [Serializable]
    public class RetryTestCase : XunitTestCase
    {
        private int _maxRetries;

        private int _delaySeconds;

        private string _exceptionTypeFullName;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", true)]
        public RetryTestCase()
        {
        }

        public RetryTestCase(
            IMessageSink diagnosticMessageSink,
            TestMethodDisplay testMethodDisplay,
            ITestMethod testMethod,
            int maxRetries,
            int delaySeconds,
            string exceptionTypeFullName)
            : base(
                diagnosticMessageSink,
                testMethodDisplay,
                TestMethodDisplayOptions.All,
                testMethod)
        {
            _maxRetries = maxRetries;
            _delaySeconds = delaySeconds;
            _exceptionTypeFullName = exceptionTypeFullName ??
                                     throw new ArgumentNullException(nameof(exceptionTypeFullName));
        }

        /// <inheritdoc cref="XunitTestCase"/>
        /// <remarks>
        /// This method is called by the xUnit test framework classes to run the test case.
        /// We will do the loop here, forwarding on to the implementation in XunitTestCase to do
        /// the heavy lifting.We will continue to re-run the test until the aggregator has an
        /// error (meaning that some internal error condition happened), or the test runs without
        /// failure, or we've hit the maximum number of tries.
        /// </remarks>
        public override async Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource
        )
        {
            int runCount = 0;
            while (true)
            {
                // This is really the only tricky bit: we need to capture and delay messages
                // (since those will contain run status) until we know we've decided to accept
                // the final result;
                var delayedMessageBus = new DelayedMessageBus(messageBus);

                string testName = DisplayName;
                if (runCount > 0)
                {
                    testName += $"\n\nRETRY:{runCount}";
                }

                // Do not throw any exceptions here if can't send test case notification because
                // xunit do not expects any exceptions here and so it crashes the process.
                // Notification sending fails probably because of rate limiting by Telegram.
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        await TestsFixture.Instance.SendTestCaseNotificationAsync(testName);
                        break;
                    }
                    catch (Exception)
                    {
                        // Log any exceptions here so we could at least know if notification
                        // sending failed
                        var waitTimeout = 30;
                        var message = new DiagnosticMessage(
                            "Couldn't send test name notification for test '{0}', " +
                            "will try one more in {1} seconds.",
                            DisplayName,
                            waitTimeout
                        );
                        diagnosticMessageSink.OnMessage(message);
                        await Task.Delay(TimeSpan.FromSeconds(waitTimeout));
                    }
                }

                var summary = await base.RunAsync(
                    diagnosticMessageSink,
                    delayedMessageBus,
                    constructorArguments,
                    aggregator,
                    cancellationTokenSource
                );

                runCount += 1;

                var testRunHasUnexpectedErrors = aggregator.HasExceptions ||
                                                 summary.Failed == 0;

                var retryExceeded = runCount > _maxRetries;

                var testRunHasExpectedException = summary.Failed == 1 &&
                                                  !delayedMessageBus
                                                      .ContainsException(_exceptionTypeFullName);

                var testCaseRunShouldReturn = testRunHasUnexpectedErrors ||
                                              retryExceeded ||
                                              testRunHasExpectedException;

                if (testCaseRunShouldReturn)
                {
                    delayedMessageBus.Dispose(); // Sends all the delayed messages
                    return summary;
                }

                diagnosticMessageSink.OnMessage(new DiagnosticMessage(
                    "Execution of '{0}' failed (attempt #{1}), retrying in {2} seconds...",
                    DisplayName,
                    runCount,
                    _delaySeconds
                ));

                await Task.Delay(TimeSpan.FromSeconds(_delaySeconds));
            }
        }

        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);

            data.AddValue(nameof(OrderedFactAttribute.MaxRetries), _maxRetries);
            data.AddValue(nameof(OrderedFactAttribute.DelaySeconds), _delaySeconds);
            data.AddValue(nameof(OrderedFactAttribute.ExceptionTypeFullName), _exceptionTypeFullName);
        }

        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);

            _maxRetries = data.GetValue<int>(nameof(OrderedFactAttribute.MaxRetries));
            _delaySeconds = data.GetValue<int>(nameof(OrderedFactAttribute.DelaySeconds));
            _exceptionTypeFullName = data.GetValue<string>(nameof(OrderedFactAttribute.ExceptionTypeFullName));
        }
    }
}
