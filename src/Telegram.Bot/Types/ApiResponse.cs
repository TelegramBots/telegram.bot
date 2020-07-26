using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// Represents bot API response
    /// </summary>
    /// <typeparam name="TResult">Expected type of operation result</typeparam>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ApiResponse<TResult>
    {
        /// <summary>
        /// Gets a value indicating whether the request was successful.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public bool Ok { get; set; }

        // This property has to stay without nullability declaration due to class/struct mismatch
        /// <summary>
        /// Gets the result object.
        /// </summary>
        /// <remarks>
        /// Do not assume nullability status of this property if you use nullable reference types.
        /// It's intentionally left without nullability annotations because of the clash between
        /// reference and value types with '?' syntax.
        /// </remarks>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [MaybeNull]
#pragma warning disable 8618
        public TResult Result { get; set; }
#pragma warning restore 8618

        /// <summary>
        /// Gets the error message.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ErrorCode { get; set; }

        /// <summary>
        /// Contains information about why a request was unsuccessful.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ResponseParameters? Parameters { get; set; }
    }
}
