using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a venue.
    /// </summary>
    [DataContract]
    public class Venue
    {
        /// <summary>
        /// Venue location
        /// </summary>
        [DataMember(IsRequired = true)]
        public Location Location { get; set; }

        /// <summary>
        /// Title of the result
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Address of the venue
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Address { get; set; }

        /// <summary>
        /// Optional. Foursquare identifier of the venue
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FoursquareId { get; set; }

        /// <summary>
        /// Optional. Foursquare type of the venue. (For example, "arts_entertainment/default",
        /// "arts_entertainment/aquarium" or "food/icecream".)
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FoursquareType { get; set; }
    }
}
