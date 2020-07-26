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
    public class InputTelegramFile : InputFileStream
    {
        /// <summary>
        /// Id of a file that exists on Telegram servers
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? FileId { get; protected set; }

        /// <inheritdoc cref="IInputFile.FileType"/>
        public override FileType FileType
        {
            get
            {
                if (Content != null) return FileType.Stream;
                if (FileId != null) return FileType.Id;
                throw new InvalidOperationException("Not a valid Input File");
            }
        }

        /// <summary>
        /// Constructs a new instance of <see cref="InputTelegramFile"/>
        /// </summary>
        protected InputTelegramFile()
        { }

        /// <summary>
        /// Constructs an <see cref="InputTelegramFile"/> from a <see cref="Stream"/> and
        /// a file name
        /// </summary>
        /// <param name="content"><see cref="Stream"/> containing the file</param>
        /// <param name="fileName">Name of the file</param>
        public InputTelegramFile(Stream content, string? fileName = default)
        {
            Content = content;
            FileName = fileName;
        }

        /// <summary>
        /// Constructs an <see cref="InputTelegramFile"/> from a file id
        /// </summary>
        /// <param name="fileId">File id on Telegram's servers</param>
        public InputTelegramFile(string fileId)
        {
            FileId = fileId;
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
        public static implicit operator InputTelegramFile?(Stream stream) =>
            stream is null
                ? default
                : new InputTelegramFile(stream);

        /// <summary>
        /// Performs an implicit conversion from a FileId or a URL to
        /// <see cref="InputOnlineFile"/>
        /// </summary>
        /// <param name="fileId">
        /// FileId to be converted to <see cref="InputOnlineFile"/>
        /// </param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator InputTelegramFile?(string fileId) =>
            fileId is null
                ? default
                : new InputTelegramFile(fileId);
    }
}
