using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send invoices. On success, the sent <see cref="Message"/> is returned.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SendInvoiceRequest : RequestBase<Message>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target private chat
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public long ChatId { get; }

        /// <summary>
        /// Product name, 1-32 characters
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Title { get; }

        /// <summary>
        /// Product description, 1-255 characters
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Description { get; }

        /// <summary>
        /// Bot-defined invoice payload, 1-128 bytes. This will not be displayed to the user, use
        /// for your internal processes.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Payload { get; }

        /// <summary>
        /// Payments provider token, obtained via @Botfather
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string ProviderToken { get; }

        /// <summary>
        /// Unique deep-linking parameter that can be used to generate this invoice when used as
        /// a start parameter
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string StartParameter { get; }

        /// <summary>
        /// Three-letter ISO 4217 currency code
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Currency { get; }

        /// <summary>
        /// Price breakdown, a list of components (e.g. product price, tax, discount, delivery
        /// cost, delivery tax, bonus, etc.)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public IEnumerable<LabeledPrice> Prices { get; }

        /// <summary>
        /// JSON-encoded data about the invoice, which will be shared with the payment provider.
        /// A detailed description of required fields should be provided by the payment provider.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? ProviderData { get; set; }

        /// <summary>
        /// URL of the product photo for the invoice. Can be a photo of the goods or a marketing
        /// image for a service. People like it better when they see what they are paying for.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? PhotoUrl { get; set; }

        /// <summary>
        /// Photo size
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? PhotoSize { get; set; }

        /// <summary>
        /// Photo width
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? PhotoWidth { get; set; }

        /// <summary>
        /// Photo height
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? PhotoHeight { get; set; }

        /// <summary>
        /// Pass True, if you require the user's full name to complete the order
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? NeedName { get; set; }

        /// <summary>
        /// Pass True, if you require the user's phone number to complete the order
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? NeedPhoneNumber { get; set; }

        /// <summary>
        /// Pass True, if you require the user's email to complete the order
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? NeedEmail { get; set; }

        /// <summary>
        /// Pass True, if you require the user's shipping address to complete the order
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? NeedShippingAddress { get; set; }

        /// <summary>
        /// Pass True, if user's phone number should be sent to provider
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? SendPhoneNumberToProvider { get; set; }

        /// <summary>
        /// Pass True, if user's email address should be sent to provider
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? SendEmailToProvider { get; set; }

        /// <summary>
        /// Pass True, if you require the user's email to complete the order
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? IsFlexible { get; set; }

        /// <summary>
        /// Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? DisableNotification { get; set; }

        /// <summary>
        /// If the message is a reply, ID of the original message.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ReplyToMessageId { get; set; }

        /// <summary>
        /// A JSON-serialized object for an inline keyboard. If empty, one 'Pay total price'
        /// button will be shown. If not empty, the first button must be a Pay button.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlineKeyboardMarkup? ReplyMarkup { get; set; }

        [JsonIgnore]
        ChatId IChatTargetable.ChatId => ChatId;

        /// <summary>
        /// Initializes a new request with chatId, title, description, payload, providerToken,
        /// startParameter, currency and an array of <see cref="LabeledPrice"/>
        /// </summary>
        /// <param name="chatId">Unique identifier for the target private chat</param>
        /// <param name="title">Product name, 1-32 characters</param>
        /// <param name="description">Product description, 1-255 characters</param>
        /// <param name="payload">Bot-defined invoice payload, 1-128 bytes</param>
        /// <param name="providerToken">Payments provider token, obtained via @Botfather</param>
        /// <param name="startParameter">
        /// Unique deep-linking parameter that can be used to generate this invoice when used as
        /// a start parameter
        /// </param>
        /// <param name="currency">Three-letter ISO 4217 currency code</param>
        /// <param name="prices">
        /// Price breakdown, a list of components (e.g. product price, tax, discount,
        /// delivery cost, delivery tax, bonus, etc.)
        /// </param>
        public SendInvoiceRequest(
            int chatId,
            string title,
            string description,
            string payload,
            string providerToken,
            string startParameter,
            string currency,
            IEnumerable<LabeledPrice> prices
        )
            : base("sendInvoice")
        {
            ChatId = chatId;
            Title = title;
            Description = description;
            Payload = payload;
            ProviderToken = providerToken;
            StartParameter = startParameter;
            Currency = currency;
            Prices = prices;
        }
    }
}
