using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types.Passport
{
    /// <summary>
    /// Contains data required for decrypting and authenticating <see cref="EncryptedPassportElement"/>.
    /// See the <see href="https://core.telegram.org/passport#receiving-information">Telegram Passport
    /// Documentation</see> for a complete description of the data decryption and authentication processes.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class EncryptedCredentials
    {
        /// <summary>
        /// Base64-encoded encrypted JSON-serialized data with unique user's payload, data hashes
        /// and secrets required for <see cref="EncryptedPassportElement"/> decryption and
        /// authentication.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Data { get; set; } = default!;

        /// <summary>
        /// Base64-encoded data hash for data authentication.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Hash { get; set; } = default!;

        /// <summary>
        /// Base64-encoded secret, encrypted with the bot's public RSA key, required for data
        /// decryption.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Secret { get; set; } = default!;
    }
}
