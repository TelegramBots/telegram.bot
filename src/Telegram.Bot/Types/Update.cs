using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents an incoming update.
    /// </summary>
    /// <remarks>Only one of the optional parameters can be present in any given update.</remarks>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Update
    {
        /// <summary>
        /// The update's unique identifier. Update identifiers start from a certain positive number
        /// and increase sequentially. This ID becomes especially handy if you're using Webhooks,
        /// since it allows you to ignore repeated updates or to restore the correct update
        /// sequence, should they get out of order.
        /// </summary>
        [JsonProperty("update_id", Required = Required.Always)]
        public int Id { get; set; }

        /// <summary>
        /// Optional. New incoming message of any kind — text, photo, sticker, etc.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message? Message { get; set; }

        /// <summary>
        /// Optional. New version of a message that is known to the bot and was edited
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message? EditedMessage { get; set; }

        /// <summary>
        /// Optional. New incoming inline query
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlineQuery? InlineQuery { get; set; }

        /// <summary>
        /// Optional. The result of a inline query that was chosen by a user and sent to their chat
        /// partner
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ChosenInlineResult? ChosenInlineResult { get; set; }

        /// <summary>
        /// Optional. New incoming callback query
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CallbackQuery? CallbackQuery { get; set; }

        /// <summary>
        /// Optional. New incoming channel post of any kind — text, photo, sticker, etc.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message? ChannelPost { get; set; }

        /// <summary>
        /// Optional. New version of a channel post that is known to the bot and was edited
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message? EditedChannelPost { get; set; }

        /// <summary>
        /// Optional. New incoming shipping query. Only for invoices with flexible price
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ShippingQuery? ShippingQuery { get; set; }

        /// <summary>
        /// Optional. New incoming pre-checkout query. Contains full information about checkout
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PreCheckoutQuery? PreCheckoutQuery { get; set; }

        /// <summary>
        /// New poll state. Bots receive only updates about polls, which are sent or stopped by the
        /// bot
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Poll? Poll { get; set; }

        /// <summary>
        /// Optional. A user changed their answer in a non-anonymous poll. Bots receive new votes
        /// only in polls that were sent by the bot itself.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PollAnswer? PollAnswer { get; set; }

        /// <summary>
        /// Gets the update type.
        /// </summary>
        /// <value>
        /// The update type.
        /// </value>
        public UpdateType Type
        {
            get
            {
                if (Message != null) return UpdateType.Message;
                if (InlineQuery != null) return UpdateType.InlineQuery;
                if (ChosenInlineResult != null) return UpdateType.ChosenInlineResult;
                if (CallbackQuery != null) return UpdateType.CallbackQuery;
                if (EditedMessage != null) return UpdateType.EditedMessage;
                if (ChannelPost != null) return UpdateType.ChannelPost;
                if (EditedChannelPost != null) return UpdateType.EditedChannelPost;
                if (ShippingQuery != null) return UpdateType.ShippingQuery;
                if (PreCheckoutQuery != null) return UpdateType.PreCheckoutQuery;
                if (Poll != null) return UpdateType.Poll;
                if (PollAnswer != null) return UpdateType.PollAnswer;

                return UpdateType.Unknown;
            }
        }
    }
}
