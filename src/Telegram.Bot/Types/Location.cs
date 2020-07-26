using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a point on the map.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Location
    {
        /// <summary>
        /// Longitude as defined by sender
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public double Longitude { get; set; }

        /// <summary>
        /// Latitude as defined by sender
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public double Latitude { get; set; }
    }
}
