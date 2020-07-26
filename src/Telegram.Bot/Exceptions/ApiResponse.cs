using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;

namespace Telegram.Bot.Exceptions
{
    /// <summary>
    /// Represents bot API response with an error
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ApiResponse
    {
        /// <summary>
        /// Gets a value indicating whether the request was successful.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public bool Ok { get; private set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Description { get; private set; }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Contains information about why a request was unsuccessful.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseParameters? Parameters { get; private set; }

        [JsonConstructor]
#pragma warning disable 8618
        private ApiResponse()
#pragma warning restore 8618
        { }

        /// <summary>
        /// Initializes an instance of <see cref="ApiResponse"/>
        /// </summary>
        /// <param name="ok">Indicating whether the request was successful</param>
        /// <param name="errorCode">Error code</param>
        /// <param name="description">Error message</param>
        /// <param name="parameters">Information about why a request was unsuccessful</param>
        public ApiResponse(
            bool ok,
            int errorCode,
            string description,
            ResponseParameters? parameters = default)
        {
            Ok = ok;
            ErrorCode = errorCode;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Parameters = parameters;
        }
    }
}
