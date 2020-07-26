using System;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Tests.Integ.Framework.Fixtures;
using Telegram.Bot.Types.Payments;

namespace Telegram.Bot.Tests.Integ.Payments
{
    public class PaymentFixture : PrivateChatFixture
    {
        public string PaymentProviderToken { get; }

        public Invoice Invoice { get; set; }

        public string Payload { get; set; }

        public ShippingOption ShippingOption { get; set; }

        public PaymentFixture(TestsFixture testsFixture)
            : base(testsFixture, Constants.TestCollections.Payment)
        {
            PaymentProviderToken = ConfigurationProvider.TestConfigurations.PaymentProviderToken;
            if (PaymentProviderToken is null)
            {
                throw new ArgumentNullException(nameof(PaymentProviderToken));
            }

            if (PaymentProviderToken.Length < 5)
            {
                throw new ArgumentException("Payment provider token is invalid", nameof(PaymentProviderToken));
            }
        }
    }
}
