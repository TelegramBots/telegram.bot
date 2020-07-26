using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot
{
    /// <summary>
    /// Represents bot API response
    /// </summary>
    /// <typeparam name="TResult">Expected type of operation result</typeparam>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal class SuccessfulApiResponse<TResult>
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
        [JsonProperty(Required = Required.Always)]
#pragma warning disable 8618
        public TResult Result { get; set; }
#pragma warning restore 8618
    }
}
