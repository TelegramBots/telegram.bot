using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Tests.Unit.Serialization
{
    public class RequestSerializationTests
    {
        [Fact(DisplayName = "Should serialize request")]
        public void Should_Serialize_Request()
        {
            GetUpdatesRequest request = new GetUpdatesRequest
            {
                Offset = 12345
            };

            string serializeRequest = JsonConvert.SerializeObject(request);

            Assert.DoesNotContain(@"""MethodName""", serializeRequest);
            Assert.DoesNotContain(@"""IsWebhookResponse""", serializeRequest);
        }

        [Fact(DisplayName = "Should properly serialize request with custom json settings")]
        public void Should_Properly_Serialize_Request_With_Custom_Json_Settings()
        {
            GetUpdatesRequest request = new GetUpdatesRequest
            {
                Offset = 12345
            };

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new CamelCasePropertyNamesContractResolver
                {
                    IgnoreSerializableAttribute = true,
                    IgnoreShouldSerializeMembers = true
                },
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified
            };

            string serializeRequest = JsonConvert.SerializeObject(request, settings);

            Assert.DoesNotContain(@"""MethodName""", serializeRequest);
            Assert.DoesNotContain(@"""method_name""", serializeRequest);
            Assert.DoesNotContain(@"""IsWebhookResponse""", serializeRequest);
            Assert.DoesNotContain(@"""is_webhook_response""", serializeRequest);
            Assert.Contains(@"""offset"":12345", serializeRequest);
            Assert.DoesNotContain(@"""allowed_updates""", serializeRequest);
        }

        [Fact]
        public void Should_Properly_Serialize_RestrictChatMemberRequest()
        {
            var request = new RestrictChatMemberRequest(
                -100123456789, 123456789, new ChatPermissions())
            {
                UntilDate = new DateTime(2020, 1, 1, 1, 1, 1, DateTimeKind.Utc)
            };

            string serializeRequest = JsonConvert.SerializeObject(request);

            Assert.Contains(@"""until_date"":1577840461", serializeRequest);
        }
    }
}
