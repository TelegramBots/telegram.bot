using System.Collections.Generic;
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
    public class SendInvoiceRequest : RequestBase<Message>,
                                      INotifiableMessage,
                                      IReplyMessage,
                                      IInlineReplyMarkupMessage
    {
        /// <summary>
        /// Unique identifier for the target private chat
        /// </summary>
        public long ChatId { get; }

        /// <summary>
        /// Product name, 1-32 characters
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Product description, 1-255 characters
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Bot-defined invoice payload, 1-128 bytes. This will not be displayed to the user, use for your internal processes.
        /// </summary>
        public string Payload { get; }

        /// <summary>
        /// Payments provider token, obtained via Botfather
        /// </summary>
        public string ProviderToken { get; }

        /// <summary>
        /// Unique deep-linking parameter that can be used to generate this invoice when used as a start parameter
        /// </summary>
        public string StartParameter { get; }

        /// <summary>
        /// Three-letter ISO 4217 currency code
        /// </summary>
        public string Currency { get; }

        /// <summary>
        /// Price breakdown, a list of components (e.g. product price, tax, discount, delivery cost, delivery tax, bonus, etc.)
        /// </summary>
        public IEnumerable<LabeledPrice> Prices { get; }

        /// <summary>
        /// JSON-encoded data about the invoice, which will be shared with the payment provider. A detailed description of required fields should be provided by the payment provider.
        /// </summary>
        public string ProviderData { get; set; }

        /// <summary>
        /// URL of the product photo for the invoice. Can be a photo of the goods or a marketing image for a service. People like it better when they see what they are paying for.
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Photo size
        /// </summary>
        public int PhotoSize { get; set; }

        /// <summary>
        /// Photo width
        /// </summary>
        public int PhotoWidth { get; set; }

        /// <summary>
        /// Photo height
        /// </summary>
        public int PhotoHeight { get; set; }

        /// <summary>
        /// Pass True, if you require the user's full name to complete the order
        /// </summary>
        public bool NeedName { get; set; }

        /// <summary>
        /// Pass True, if you require the user's phone number to complete the order
        /// </summary>
        public bool NeedPhoneNumber { get; set; }

        /// <summary>
        /// Pass True, if you require the user's email to complete the order
        /// </summary>
        public bool NeedEmail { get; set; }

        /// <summary>
        /// Pass True, if you require the user's shipping address to complete the order
        /// </summary>
        public bool NeedShippingAddress { get; set; }

        /// <summary>
        /// Pass True, if you require the user's email to complete the order
        /// </summary>
        public bool IsFlexible { get; set; }

        /// <inheritdoc />
        public bool DisableNotification { get; set; }

        /// <inheritdoc />
        public int ReplyToMessageId { get; set; }

        /// <summary>
        /// A JSON-serialized object for an inline keyboard. If empty, one 'Pay total price' button will be shown. If not empty, the first button must be a Pay button.
        /// </summary>
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId, title, description, payload, providerToken, sStartParameter, currency and an array of <see cref="LabeledPrice"/>
        /// </summary>
        /// <param name="chatId">Unique identifier for the target private chat</param>
        /// <param name="title">Product name, 1-32 characters</param>
        /// <param name="description">Product description, 1-255 characters</param>
        /// <param name="payload">Bot-defined invoice payload, 1-128 bytes</param>
        /// <param name="providerToken">Payments provider token, obtained via Botfather</param>
        /// <param name="startParameter">Unique deep-linking parameter that can be used to generate this invoice when used as a start parameter</param>
        /// <param name="currency">Three-letter ISO 4217 currency code</param>
        /// <param name="prices">Price breakdown, a list of components (e.g. product price, tax, discount, delivery cost, delivery tax, bonus, etc.)</param>
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
