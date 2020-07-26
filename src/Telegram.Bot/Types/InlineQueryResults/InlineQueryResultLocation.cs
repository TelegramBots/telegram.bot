using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a location on a map. By default, the location will be sent by the user.
    /// Alternatively, you can use <see cref="InputMessageContent"/> to send a message with the
    /// specified content instead of the location.
    /// </summary>
    /// <remarks>
    /// This will only work in Telegram versions released after 9 April, 2016. Older clients will
    /// ignore them.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InlineQueryResultLocation : InlineQueryResultBase
    {
        /// <summary>
        /// Latitude of the location in degrees
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude of the location in degrees
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public double Longitude { get; set; }

        /// <summary>
        /// Title of the result
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Period in seconds for which the location can be updated, should be between
        /// 60 and 86400.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? LivePeriod { get; set; }

        /// <summary>
        /// URL of the static thumbnail for the result.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ThumbUrl { get; set; }

        /// <summary>
        /// Thumbnail width.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ThumbWidth { get; set; }

        /// <summary>
        /// Thumbnail height.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ThumbHeight { get; set; }

        /// <summary>
        /// Title of the result
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputMessageContentBase? InputMessageContent { get; set; }

#pragma warning disable 8618
        private InlineQueryResultLocation()
#pragma warning restore 8618
            : base(InlineQueryResultType.Location)
        {
        }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="latitude">Latitude of the location in degrees</param>
        /// <param name="longitude">Longitude of the location in degrees</param>
        /// <param name="title">Title of the result</param>
        public InlineQueryResultLocation(string id, double latitude, double longitude, string title)
            : base(InlineQueryResultType.Location, id)
        {
            Latitude = latitude;
            Longitude = longitude;
            Title = title;
        }
    }
}
