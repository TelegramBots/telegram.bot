using System;
using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types.Passport
{
    /// <summary>
    /// This object represents a file uploaded to Telegram Passport. Currently all Telegram Passport files are in JPEG format when decrypted and don't exceed 10MB.
    /// </summary>
    [DataContract]
    public class PassportFile : FileBase
    {
        /// <summary>
        /// Unix time when the file was uploaded
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime FileDate { get; set; }
    }
}
