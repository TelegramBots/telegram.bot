using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a point on the map.
    /// </summary>
    [DataContract]
    public class Location
    {
        /// <summary>
        /// Longitude as defined by sender
        /// </summary>
        [DataMember(IsRequired = true)]
        public float Longitude { get; set; }

        /// <summary>
        /// Latitude as defined by sender
        /// </summary>
        [DataMember(IsRequired = true)]
        public float Latitude { get; set; }
    }
}
