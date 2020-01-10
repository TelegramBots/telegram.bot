

// ReSharper disable once CheckNamespace

using System.Runtime.Serialization;

namespace Telegram.Bot.Types.Passport
{
    /// <summary>
    /// Contains information about Telegram Passport data shared with the bot by the user.
    /// </summary>
    [DataContract]
    public class PassportData
    {
        /// <summary>
        /// Array with information about documents and other Telegram Passport elements that was shared with the bot.
        /// </summary>
        [DataMember(IsRequired = true)]
        public EncryptedPassportElement[] Data { get; set; }

        /// <summary>
        /// Encrypted credentials required to decrypt the data.
        /// </summary>
        [DataMember(IsRequired = true)]
        public EncryptedCredentials Credentials { get; set; }
    }
}
