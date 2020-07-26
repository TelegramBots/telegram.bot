using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Represents the content of a contact message to be sent as the result of an
    /// <see cref="InlineQuery"/>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InputContactMessageContent : InputMessageContentBase
    {
        /// <summary>
        /// Contact's phone number
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string PhoneNumber { get; private set; }

        /// <summary>
        /// Contact's first name
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string FirstName { get; private set; }

        /// <summary>
        /// Optional. Contact's last name
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? LastName { get; set; }

        /// <summary>
        /// Optional. Additional data about the contact in the form of a vCard, 0-2048 bytes
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Vcard { get; set; }

#pragma warning disable 8618
        private InputContactMessageContent()
#pragma warning restore 8618
        { }

        /// <summary>
        /// Initializes a new input contact message content
        /// </summary>
        /// <param name="phoneNumber">The phone number of the contact</param>
        /// <param name="firstName">The first name of the contact</param>
        public InputContactMessageContent(string phoneNumber, string firstName)
        {
            PhoneNumber = phoneNumber;
            FirstName = firstName;
        }
    }
}
