using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types.Passport
{
    /// <summary>
    /// Contains information about documents or other Telegram Passport elements shared with the
    /// bot by the user.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class EncryptedPassportElement
    {
        /// <summary>
        /// Element type. One of "personal_details", "passport", "driver_license", "identity_card",
        /// "internal_passport", "address", "utility_bill", "bank_statement", "rental_agreement",
        /// "passport_registration", "temporary_registration", "phone_number", "email".
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Type { get; set; } = default!;

        /// <summary>
        /// Base64-encoded element hash for using in PassportElementErrorUnspecified
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Hash { get; set; } = default!;

        /// <summary>
        /// Optional. Base64-encoded encrypted Telegram Passport element data provided by the user,
        /// available for "personal_details", "passport", "driver_license", "identity_card",
        /// "identity_passport" and "address" types. Can be decrypted and verified using the
        /// accompanying <see cref="EncryptedCredentials"/>.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Data { get; set; }

        /// <summary>
        /// Optional. User's verified phone number, available only for "phone_number" type.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Optional. User's verified email address, available only for "email" type.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Email { get; set; }

        /// <summary>
        /// Optional. Array of encrypted files with documents provided by the user, available for
        /// "utility_bill", "bank_statement", "rental_agreement", "passport_registration" and
        /// "temporary_registration" types. Files can be decrypted and verified using the
        /// accompanying <see cref="EncryptedCredentials"/>.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PassportFile[]? Files { get; set; }

        /// <summary>
        /// Optional. Encrypted file with the front side of the document, provided by the user.
        /// Available for "passport", "driver_license", "identity_card" and "internal_passport".
        /// The file can be decrypted and verified using the accompanying
        /// <see cref="EncryptedCredentials"/>.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PassportFile? FrontSide { get; set; }

        /// <summary>
        /// Optional. Encrypted file with the reverse side of the document, provided by the user.
        /// Available for "driver_license" and "identity_card". The file can be decrypted and
        /// verified using the accompanying <see cref="EncryptedCredentials"/>.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PassportFile? ReverseSide { get; set; }

        /// <summary>
        /// Optional. Encrypted file with the selfie of the user holding a document, provided by
        /// the user; available for "passport", "driver_license", "identity_card" and
        /// "internal_passport". The file can be decrypted and verified using the accompanying
        /// <see cref="EncryptedCredentials"/>.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PassportFile? Selfie { get; set; }

        /// <summary>
        /// Optional. Array of encrypted files with translated versions of documents provided by
        /// the user. Available if requested for "passport", "driver_license", "identity_card",
        /// "internal_passport", "utility_bill", "bank_statement", "rental_agreement",
        /// "passport_registration" and "temporary_registration" types. Files can be decrypted and
        /// verified using the accompanying <see cref="EncryptedCredentials"/>.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PassportFile[]? Translation { get; set; }
    }
}
