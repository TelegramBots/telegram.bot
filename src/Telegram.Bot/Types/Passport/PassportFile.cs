using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types.Passport
{
    /// <summary>
    /// This object represents a file uploaded to Telegram Passport. Currently all Telegram
    /// Passport files are in JPEG format when decrypted and don't exceed 10MB.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class PassportFile : FileBase
    {
        /// <summary>
        /// Unix time when the file was uploaded
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime FileDate { get; set; }
    }
}
