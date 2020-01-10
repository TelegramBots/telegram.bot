using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send point on the map
    /// </summary>
    public sealed class SendLocationRequest : RequestBase<Message>,
                                              IChatMessage,
                                              INotifiableMessage,
                                              IReplyMessage,
                                              IReplyMarkupMessage<IReplyMarkup>
    {
        /// <inheritdoc />
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// Latitude of the location
        /// </summary>
        [DataMember(IsRequired = true)]
        public float Latitude { get; }

        /// <summary>
        /// Longitude of the location
        /// </summary>
        [DataMember(IsRequired = true)]
        public float Longitude { get; }

        /// <summary>
        /// Period in seconds for which the location will be updated, should be between 60 and 86400
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int LivePeriod { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public bool DisableNotification { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public int ReplyToMessageId { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public IReplyMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="latitude">Latitude of the location</param>
        /// <param name="longitude">Longitude of the location</param>
        public SendLocationRequest([DisallowNull] ChatId chatId, float latitude, float longitude)
            : base("sendLocation")
        {
            ChatId = chatId;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
