using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents the content of a <see cref="Venue"/> message to be sent as the result of an
    /// <see cref="InlineQuery"/>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InputVenueMessageContent : InputMessageContentBase
    {
        /// <summary>
        /// Latitude of the location in degrees
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public double Latitude { get; private set; }

        /// <summary>
        /// Longitude of the location in degrees
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public double Longitude { get; private set; }

        /// <summary>
        /// Name of the venue
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Address of the venue
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Address { get; private set; }

        /// <summary>
        /// Optional. Foursquare identifier of the venue, if known
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? FoursquareId { get; set; }

        /// <summary>
        /// Optional. Foursquare type of the venue. (For example, “arts_entertainment/default”,
        /// “arts_entertainment/aquarium” or “food/icecream”.)
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? FoursquareType { get; set; }

#pragma warning disable 8618
        private InputVenueMessageContent()
#pragma warning restore 8618
        { }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="title">The name of the venue</param>
        /// <param name="address">The address of the venue</param>
        /// <param name="latitude">The latitude of the venue</param>
        /// <param name="longitude">The longitude of the venue</param>
        public InputVenueMessageContent(
            string title,
            string address,
            double latitude,
            double longitude)
        {
            Title = title;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
