using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Payments
{
    [Collection(Constants.TestCollections.Payment)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class PaymentTests : IClassFixture<PaymentFixture>
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        private readonly PaymentFixture _classFixture;

        public PaymentTests(TestsFixture fixture, PaymentFixture classFixture)
        {
            _fixture = fixture;
            _classFixture = classFixture;
        }

        [OrderedFact("Should send an invoice")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendInvoice)]
        public async Task Should_Send_Invoice()
        {
            await _fixture.SendTestInstructionsAsync(
                "Click on *Pay <amount>* and send your shipping address. " +
                "You should see shipment options afterwards. " +
                "Transaction should be completed.",
                chatId: _classFixture.PrivateChat.Id
            );

            _classFixture.Payload = "my-payload";
            string url = "https://cdn.pixabay.com/photo/2017/09/07/08/54/money-2724241_640.jpg";

            LabeledPrice[] productPrices =
            {
                new LabeledPrice(label: "PART_OF_PRODUCT_PRICE_1", amount: 150),
                new LabeledPrice(label: "PART_OF_PRODUCT_PRICE_2", amount: 2029),
            };

            Invoice invoice = new Invoice
            {
                Title = "PRODUCT_TITLE",
                Currency = "CAD",
                StartParameter = "start_param",
                TotalAmount = productPrices.Sum(p => p.Amount),
                Description = "PRODUCT_DESCRIPTION",
            };

            Message message = await BotClient.SendInvoiceAsync(
                chatId: (int) _classFixture.PrivateChat.Id,
                title: invoice.Title,
                description: invoice.Description,
                payload: _classFixture.Payload,
                providerToken: _classFixture.PaymentProviderToken,
                startParameter: invoice.StartParameter,
                currency: invoice.Currency,
                prices: productPrices,
                photoUrl: url,
                photoWidth: 600,
                photoHeight: 400,
                needShippingAddress: true,
                isFlexible: true,
                needName: true,
                needEmail: true,
                needPhoneNumber: true,
                sendEmailToProvider: true,
                sendPhoneNumberToProvider: true
            );

            Assert.Equal(MessageType.Invoice, message.Type);
            Assert.Equal(invoice.Title, message.Invoice!.Title);
            Assert.Equal(invoice.Currency, message.Invoice.Currency);
            Assert.Equal(invoice.TotalAmount, message.Invoice.TotalAmount);
            Assert.Equal(invoice.Description, message.Invoice.Description);

            _classFixture.Invoice = message.Invoice;
        }

        [OrderedFact("Should receive shipping address query and reply with shipping options")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendInvoice)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerShippingQuery)]
        public async Task Should_Answer_Shipping_Query_With_Ok()
        {
            LabeledPrice[] shippingPrices =
            {
                new LabeledPrice(label: "PART_OF_SHIPPING_TOTAL_PRICE_1", amount: 500),
                new LabeledPrice(label: "PART_OF_SHIPPING_TOTAL_PRICE_2", amount: 299),
            };

            ShippingOption shippingOption = new ShippingOption
            {
                Id = "option1",
                Title = "OPTION-1",
                Prices = shippingPrices,
            };

            ShippingOption[] shippingOptions =
            {
                shippingOption
            };

            _classFixture.ShippingOption = shippingOption;

            Update shippingUpdate = await GetShippingQueryUpdate();

            await _fixture.BotClient.AnswerShippingQueryAsync(
                shippingQueryId: shippingUpdate.ShippingQuery!.Id,
                shippingOptions: shippingOptions
            );

            Assert.Equal(UpdateType.ShippingQuery, shippingUpdate.Type);
            Assert.Equal(_classFixture.Payload, shippingUpdate.ShippingQuery.InvoicePayload);
            Assert.NotNull(shippingUpdate.ShippingQuery.ShippingAddress.CountryCode);
            Assert.NotNull(shippingUpdate.ShippingQuery.ShippingAddress.City);
            Assert.NotNull(shippingUpdate.ShippingQuery.ShippingAddress.State);
            Assert.NotNull(shippingUpdate.ShippingQuery.ShippingAddress.StreetLine1);
            Assert.NotNull(shippingUpdate.ShippingQuery.ShippingAddress.PostCode);
        }

        [OrderedFact("Should send invoice for no shipment option, and reply pre-checkout query with OK.")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendInvoice)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerPreCheckoutQuery)]
        public async Task Should_Answer_PreCheckout_Query_With_Ok_And_Shipment_Option()
        {
            Update preCheckoutUpdate = await GetPreCheckoutQueryUpdate();
            PreCheckoutQuery query = preCheckoutUpdate.PreCheckoutQuery;

            await _fixture.BotClient.AnswerPreCheckoutQueryAsync(
                preCheckoutQueryId: query!.Id
            );

            int totalAmount = _classFixture.Invoice.TotalAmount +
                              _classFixture.ShippingOption.Prices.Sum(p => p.Amount);

            Assert.Equal(UpdateType.PreCheckoutQuery, preCheckoutUpdate.Type);
            Assert.NotNull(query.Id);
            Assert.Equal(_classFixture.Payload, query.InvoicePayload);
            Assert.Equal(totalAmount, query.TotalAmount);
            Assert.Equal(_classFixture.Invoice.Currency, query.Currency);
            Assert.Contains(query.From.Username, _fixture.UpdateReceiver.AllowedUsernames);
            Assert.NotNull(query.OrderInfo);
            Assert.NotNull(query.OrderInfo.Email);
            Assert.NotNull(query.OrderInfo.Name);
            Assert.NotNull(query.OrderInfo.PhoneNumber);
            Assert.Equal(_classFixture.ShippingOption.Id, query.ShippingOptionId);
        }

        [OrderedFact("Should receive successful payment.")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendInvoice)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerPreCheckoutQuery)]
        public async Task Should_Receive_Successful_Payment_With_Shipment_Option()
        {
            Update successfulPaymentUpdate = await GetSuccessfulPaymentUpdate();
            SuccessfulPayment successfulPayment = successfulPaymentUpdate.Message!.SuccessfulPayment;

            int totalAmount = _classFixture.Invoice.TotalAmount +
                              _classFixture.ShippingOption.Prices.Sum(p => p.Amount);

            Assert.Equal(totalAmount, successfulPayment!.TotalAmount);
            Assert.Equal(_classFixture.Payload, successfulPayment.InvoicePayload);
            Assert.Equal(_classFixture.Invoice.Currency, successfulPayment.Currency);
            Assert.Equal(_classFixture.ShippingOption.Id, successfulPayment.ShippingOptionId);
        }

        [OrderedFact("Should receive shipping address query and reply with an error")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendInvoice)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerShippingQuery)]
        public async Task Should_Answer_Shipping_Query_With_Error()
        {
            await _fixture.SendTestInstructionsAsync(
                "Click on *Pay <amount>* and send your shipping address. You should receive an error afterwards.",
                chatId: _classFixture.PrivateChat.Id
            );

            string payload = "shipping_query-error-payload";

            LabeledPrice[] productPrices =
            {
                new LabeledPrice(label: "PART_OF_PRODUCT_PRICE_1", amount: 150),
                new LabeledPrice(label: "PART_OF_PRODUCT_PRICE_2", amount: 2029),
            };
            Invoice invoice = new Invoice
            {
                Title = "PRODUCT_TITLE",
                Currency = "CAD",
                StartParameter = "start_param",
                TotalAmount = productPrices.Sum(p => p.Amount),
                Description = "PRODUCT_DESCRIPTION",
            };

            await _fixture.BotClient.SendInvoiceAsync(
                chatId: (int) _classFixture.PrivateChat.Id,
                title: invoice.Title,
                description: invoice.Description,
                payload: payload,
                providerToken: _classFixture.PaymentProviderToken,
                startParameter: invoice.StartParameter,
                currency: invoice.Currency,
                prices: productPrices,
                needShippingAddress: true,
                isFlexible: true
            );

            Update shippingUpdate = await GetShippingQueryUpdate();

            await _fixture.BotClient.AnswerShippingQueryAsync(
                shippingQueryId: shippingUpdate.ShippingQuery!.Id,
                errorMessage: "HUMAN_FRIENDLY_DELIVERY_ERROR_MESSAGE"
            );
        }

        [OrderedFact("Should send invoice for no shipment option, and reply pre-checkout query with an error.")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendInvoice)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerPreCheckoutQuery)]
        public async Task Should_Answer_PreCheckout_Query_With_Error_For_No_Shipment_Option()
        {
            await _fixture.SendTestInstructionsAsync(
                "Click on *Pay <amount>* and confirm payment.",
                chatId: _classFixture.PrivateChat.Id
            );

            string payload = "pre_checkout-error-payload";

            LabeledPrice[] productPrices =
            {
                new LabeledPrice(label: "PART_OF_PRODUCT_PRICE_1", amount: 150),
                new LabeledPrice(label: "PART_OF_PRODUCT_PRICE_2", amount: 2029),
            };
            Invoice invoice = new Invoice
            {
                Title = "PRODUCT_TITLE",
                Currency = "USD",
                StartParameter = "start_param",
                TotalAmount = productPrices.Sum(p => p.Amount),
                Description = "PRODUCT_DESCRIPTION",
            };

            await _fixture.BotClient.SendInvoiceAsync(
                chatId: (int) _classFixture.PrivateChat.Id,
                title: invoice.Title,
                description: invoice.Description,
                payload: payload,
                providerToken: _classFixture.PaymentProviderToken,
                startParameter: invoice.StartParameter,
                currency: invoice.Currency,
                prices: productPrices
            );

            Update preCheckoutUpdate = await GetPreCheckoutQueryUpdate();
            PreCheckoutQuery query = preCheckoutUpdate.PreCheckoutQuery;

            await _fixture.BotClient.AnswerPreCheckoutQueryAsync(
                preCheckoutQueryId: query!.Id,
                errorMessage: "HUMAN_FRIENDLY_ERROR_MESSAGE"
            );
        }

        [OrderedFact("Should throw exception when sending invoice with invalid provider data")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendInvoice)]
        public async Task Should_Throw_When_Send_Invoice_Invalid_Provider_Data()
        {
            string payload = "my-payload";

            LabeledPrice[] prices =
            {
                new LabeledPrice(label: "PART_OF_PRODUCT_PRICE_1", amount: 150),
                new LabeledPrice(label: "PART_OF_PRODUCT_PRICE_2", amount: 2029),
            };
            Invoice invoice = new Invoice
            {
                Title = "PRODUCT_TITLE",
                Currency = "CAD",
                StartParameter = "start_param",
                TotalAmount = prices.Sum(p => p.Amount),
                Description = "PRODUCT_DESCRIPTION",
            };

            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await BotClient.SendInvoiceAsync(
                    chatId: (int) _classFixture.PrivateChat.Id,
                    title: invoice.Title,
                    description: invoice.Description,
                    payload: payload,
                    providerToken: _classFixture.PaymentProviderToken,
                    startParameter: invoice.StartParameter,
                    currency: invoice.Currency,
                    prices: prices,
                    providerData: "INVALID-JSON"
                ));

            Assert.Equal(400, exception.ErrorCode);
            Assert.Equal("Bad Request: DATA_JSON_INVALID", exception.Message);
        }

        [OrderedFact("Should throw exception when answering shipping query with duplicate shipping Id")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendInvoice)]
        public async Task Should_Throw_When_Answer_Shipping_Query_With_Duplicate_Shipping_Id()
        {
            string payload = "my-payload";

            LabeledPrice[] productPrices =
            {
                new LabeledPrice(label: "PART_OF_PRODUCT_PRICE_1", amount: 150),
                new LabeledPrice(label: "PART_OF_PRODUCT_PRICE_2", amount: 2029),
            };
            Invoice invoice = new Invoice
            {
                Title = "PRODUCT_TITLE",
                Currency = "USD",
                StartParameter = "start_param",
                TotalAmount = productPrices.Sum(p => p.Amount),
                Description = "PRODUCT_DESCRIPTION",
            };

            await _fixture.BotClient.SendInvoiceAsync(
                chatId: (int) _classFixture.PrivateChat.Id,
                title: invoice.Title,
                description: invoice.Description,
                payload: payload,
                providerToken: _classFixture.PaymentProviderToken,
                startParameter: invoice.StartParameter,
                currency: invoice.Currency,
                prices: productPrices,
                isFlexible: true
            );

            LabeledPrice[] shippingPrices =
            {
                new LabeledPrice(label: "PART_OF_SHIPPING_TOTAL_PRICE_1", amount: 500),
                new LabeledPrice(label: "PART_OF_SHIPPING_TOTAL_PRICE_2", amount: 299),
            };

            ShippingOption shippingOption = new ShippingOption
            {
                Id = "option1",
                Title = "OPTION-1",
                Prices = shippingPrices,
            };

            Update shippingUpdate = await GetShippingQueryUpdate();

            ApiRequestException exception = await Assert.ThrowsAsync<ApiRequestException>(async () =>
                await _fixture.BotClient.AnswerShippingQueryAsync(
                    shippingQueryId: shippingUpdate.ShippingQuery!.Id,
                    shippingOptions: new [] { shippingOption, shippingOption }
                )
            );

            Assert.Equal(400, exception.ErrorCode);
            Assert.Equal("Bad Request: SHIPPING_ID_DUPLICATE", exception.Message);

            await _fixture.BotClient.AnswerShippingQueryAsync(
                shippingQueryId: shippingUpdate.ShippingQuery!.Id,
                errorMessage: "✅ Test Passed"
            );
        }

        [OrderedFact("Should send an invoice with custom reply markup")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendInvoice)]
        public async Task Should_Send_Invoice_With_Reply_Markup()
        {
            await BotClient.SendInvoiceAsync(
                chatId: (int) _classFixture.PrivateChat.Id,
                title: "Product",
                description: "product description",
                payload: "test payload",
                providerToken: _classFixture.PaymentProviderToken,
                startParameter: "start_parameter",
                currency: "USD",
                prices: new []
                {
                    new LabeledPrice(label: "price", amount: 150),
                },
                replyMarkup: new InlineKeyboardMarkup(new []
                {
                    new []
                    {
                        InlineKeyboardButton.WithPayment(text: "Pay this invoice"),
                        InlineKeyboardButton.WithUrl(
                            text: "Repository",
                            url: "https://github.com/TelegramBots/Telegram.Bot"
                        )
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(textAndCallbackData: "Some other button")
                    }
                })
            );
        }

        private async Task<Update> GetShippingQueryUpdate(CancellationToken cancellationToken = default)
        {
            Update update = await _fixture.UpdateReceiver.GetUpdateAsync(
                cancellationToken: cancellationToken,
                updateTypes: UpdateType.ShippingQuery
            );

            await _fixture.UpdateReceiver.DiscardNewUpdatesAsync(cancellationToken);

            return update;
        }

        private async Task<Update> GetPreCheckoutQueryUpdate(CancellationToken cancellationToken = default)
        {
            Update update = await _fixture.UpdateReceiver.GetUpdateAsync(
                cancellationToken: cancellationToken,
                updateTypes: UpdateType.PreCheckoutQuery
            );
            await _fixture.UpdateReceiver.DiscardNewUpdatesAsync(cancellationToken);

            return update;
        }

        private async Task<Update> GetSuccessfulPaymentUpdate(CancellationToken cancellationToken = default)
        {
            Update update = await _fixture.UpdateReceiver.GetUpdateAsync(
                predicate: u => u.Message!.Type == MessageType.SuccessfulPayment,
                cancellationToken: cancellationToken,
                updateTypes: UpdateType.Message
            );

            await _fixture.UpdateReceiver.DiscardNewUpdatesAsync(cancellationToken);

            return update;
        }
    }
}
