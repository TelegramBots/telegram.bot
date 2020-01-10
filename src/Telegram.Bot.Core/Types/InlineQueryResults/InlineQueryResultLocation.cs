using System.Runtime.Serialization;
using Telegram.Bot.Types.InlineQueryResults.Abstractions;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents a location on a map. By default, the location will be sent by the user. Alternatively, you can use input_message_content to send a message with the specified content instead of the location.
    /// </summary>
    /// <remarks>
    /// This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.
    /// </remarks>
    [DataContract]
    public class InlineQueryResultLocation : InlineQueryResultBase,
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

        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Period in seconds for which the location can be updated, should be between 60 and 86400.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int LivePeriod { get; set; }

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

        private InlineQueryResultLocation()
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
        public InlineQueryResultLocation(string id, float latitude, float longitude, string title)
            : base(InlineQueryResultType.Location, id)
        {
            Latitude = latitude;
            Longitude = longitude;
            Title = title;
        }
    }
}
