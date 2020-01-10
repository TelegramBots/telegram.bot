using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send information about a venue
    /// </summary>
    public class SendVenueRequest : RequestBase<Message>,
                                    INotifiableMessage,
                                    IReplyMessage,
                                    IReplyMarkupMessage<IReplyMarkup>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// </summary>
        public ChatId ChatId { get; }

        /// <summary>
        /// Latitude of the venue
        /// </summary>
        public float Latitude { get; }

        /// <summary>
        /// Longitude of the venue
        /// </summary>
        public float Longitude { get; }

        /// <summary>
        /// Name of the venue
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Address of the venue
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Foursquare identifier of the venue
        /// </summary>
        public string FoursquareId { get; set; }

        /// <summary>
        /// Optional. Foursquare type of the venue. (For example, "arts_entertainment/default",
        /// "arts_entertainment/aquarium" or "food/icecream".)
        /// </summary>
        public string FoursquareType { get; set; }

        /// <inheritdoc />
        public bool DisableNotification { get; set; }

        /// <inheritdoc />
        public int ReplyToMessageId { get; set; }

        /// <inheritdoc />
        public IReplyMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId, location, venue title and address
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="latitude">Latitude of the venue</param>
        /// <param name="longitude">Longitude of the venue</param>
        /// <param name="title">Name of the venue</param>
        /// <param name="address">Address of the venue</param>
        public SendVenueRequest(
            ChatId chatId,
            float latitude,
            float longitude,
            string title,
            string address
        )
            : base("sendVenue")
        {
            ChatId = chatId;
            Latitude = latitude;
            Longitude = longitude;
            Title = title;
            Address = address;
        }
    }
}
