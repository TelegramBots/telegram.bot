using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Converters;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.InputFiles
{
    /// <summary>
    /// Used for sending files to Telegram
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    [JsonConverter(typeof(InputFileConverter))]
    public class InputOnlineFile : InputTelegramFile
    {
        /// <summary>
        /// HTTP URL for Telegram to get a file from the Internet
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Url { get; protected set; }

        /// <inheritdoc cref="IInputFile.FileType"/>
        public override FileType FileType
        {
            get
            {
                if (Content != null) return FileType.Stream;
                if (FileId != null) return FileType.Id;
                if (Url != null) return FileType.Url;
                throw new InvalidOperationException("Not a valid InputFile");
            }
        }

        /// <summary>
        /// Constructs an <see cref="InputOnlineFile"/> from a <see cref="Stream"/> and a file name
        /// </summary>
        /// <param name="content"><see cref="Stream"/> containing the file</param>
        /// <param name="fileName">Name of the file</param>
        public InputOnlineFile(Stream content, string? fileName = default)
        {
            Content = content;
            FileName = fileName;
        }

        /// <summary>
        /// Constructs an <see cref="InputOnlineFile"/> from a string containing a uri or file id
        /// </summary>
        /// <param name="value"><see cref="string"/> containing a url or file id</param>
        public InputOnlineFile(string value)
        {
            if (Uri.TryCreate(value, UriKind.Absolute, out _))
            {
                Url = value;
            }
            else
            {
                FileId = value;
            }
        }

        /// <summary>
        /// Constructs an <see cref="InputOnlineFile"/> from a <see cref="Uri"/>
        /// </summary>
        /// <param name="url"><see cref="Uri"/> pointing to a file</param>
        public InputOnlineFile(Uri url)
        {
            Url = url.AbsoluteUri;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Stream"/> to
        /// <see cref="InputOnlineFile"/>
        /// </summary>
        /// <param name="stream">
        /// <see cref="Stream"/> instance to be converted to <see cref="InputOnlineFile"/>
        /// </param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator InputOnlineFile?(Stream stream) =>
            stream is null
                ? default
                : new InputOnlineFile(stream);

        /// <summary>
        /// Performs an implicit conversion from a FileId or a URL to
        /// <see cref="InputOnlineFile"/>
        /// </summary>
        /// <param name="value">
        /// FileId of a URL to be converted to <see cref="InputOnlineFile"/>
        /// </param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator InputOnlineFile?(string value) =>
            value is null
                ? default
                : new InputOnlineFile(value);
    }
}
