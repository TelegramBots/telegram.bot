using System.Runtime.Serialization;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents the content of a <see cref="Venue"/> message to be sent as the result of an <see cref="InlineQuery"/>.
    /// </summary>
    [DataContract]
    public class InputVenueMessageContent : InputMessageContentBase
    {
        /// <summary>
        /// Latitude of the location in degrees
        /// </summary>
        [DataMember(IsRequired = true)]
        public float Latitude { get; private set; }

        /// <summary>
        /// Longitude of the location in degrees
        /// </summary>
        [DataMember(IsRequired = true)]
        public float Longitude { get; private set; }

        /// <summary>
        /// Name of the venue
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Address of the venue
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Address { get; private set; }

        /// <summary>
        /// Optional. Foursquare identifier of the venue, if known
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FoursquareId { get; set; }

        /// <summary>
        /// Optional. Foursquare type of the venue. (For example, “arts_entertainment/default”, “arts_entertainment/aquarium” or “food/icecream”.)
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FoursquareType { get; set; }

        private InputVenueMessageContent()
        {
        }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="title">The name of the venue</param>
        /// <param name="address">The address of the venue</param>
        /// <param name="latitude">The latitude of the venue</param>
        /// <param name="longitude">The longitude of the venue</param>
        public InputVenueMessageContent(string title, string address, float latitude, float longitude)
        {
            Title = title;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
