using System.Runtime.Serialization;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a venue. By default, the venue will be sent by the user. Alternatively, you can use <see cref="InputMessageContent"/> to send a message with the specified content instead of the venue.
    /// </summary>
    /// <remarks>
    /// This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.
    /// </remarks>
    [DataContract]
    public class InlineQueryResultVenue : InlineQueryResultBase,
        IThumbnailInlineQueryResult,
        ITitleInlineQueryResult,
        IInputMessageContentResult,
        ILocationInlineQueryResult
    {
        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public float Latitude { get; set; }

        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public float Longitude { get; set; }

        /// <summary>
        /// Address of the venue
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Address { get; set; }

        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Optional. Foursquare identifier of the venue if known
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FoursquareId { get; set; }

        /// <summary>
        /// Optional. Foursquare type of the venue. (For example, "arts_entertainment/default",
        /// "arts_entertainment/aquarium" or "food/icecream".)
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FoursquareType { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public string ThumbUrl { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public int ThumbWidth { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public int ThumbHeight { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public InputMessageContentBase InputMessageContent { get; set; }

        private InlineQueryResultVenue()
            : base(InlineQueryResultType.Venue)
        {
        }

        /// <summary>
        /// Initializes a new inline query result
        /// </summary>
        /// <param name="id">Unique identifier of this result</param>
        /// <param name="latitude">Latitude of the location in degrees</param>
        /// <param name="longitude">Longitude of the location in degrees</param>
        /// <param name="title">Title of the result</param>
        /// <param name="address">Address of the venue</param>
        public InlineQueryResultVenue(
            string id,
            float latitude,
            float longitude,
            string title,
            string address)
            : base(InlineQueryResultType.Venue, id)
        {
            Latitude = latitude;
            Longitude = longitude;
            Title = title;
            Address = address;
        }
    }
}
