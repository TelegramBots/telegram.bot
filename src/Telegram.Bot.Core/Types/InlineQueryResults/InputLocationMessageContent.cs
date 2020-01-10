using System.Runtime.Serialization;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents the content of a location message to be sent as the result of an <see cref="InlineQuery"/>.
    /// </summary>
    /// <remarks>
    /// This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.
    /// </remarks>
    [DataContract]
    public class InputLocationMessageContent : InputMessageContentBase
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
        /// How long the live location will be updated
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int LivePeriod { get; set; }

        private InputLocationMessageContent()
        { }

        /// <summary>
        /// Initializes a new input location message content
        /// </summary>
        /// <param name="latitude">The latitude of the location</param>
        /// <param name="longitude">The longitude of the location</param>
        public InputLocationMessageContent(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
