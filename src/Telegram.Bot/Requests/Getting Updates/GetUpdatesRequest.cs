using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Receive incoming updates using long polling. An Array of <see cref="Update" /> objects
    /// is returned.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class GetUpdatesRequest : RequestBase<Update[]>
    {
        /// <summary>
        /// Identifier of the first update to be returned
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Offset { get; set; }

        /// <summary>
        /// Limits the number of updates to be retrieved. Values between 1—100 are accepted.
        /// Defaults to 100.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Limit { get; set; }

        /// <summary>
        /// Timeout in seconds for long polling. Defaults to 0, i.e. usual short polling.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Timeout { get; set; }

        /// <summary>
        /// List the types of updates you want your bot to receive. Specify an empty list to
        /// receive all updates regardless of type (default). If not specified, the previous
        /// setting will be used.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<UpdateType>? AllowedUpdates { get; set; }

        /// <summary>
        /// Initializes a new GetUpdates request
        /// </summary>
        public GetUpdatesRequest()
            : base("getUpdates")
        {
        }
    }
}
