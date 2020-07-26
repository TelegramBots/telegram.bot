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
    public class InputFileStream : IInputFile
    {
        /// <summary>
        /// File content to upload
        /// </summary>
        public Stream? Content { get; protected set; }

        /// <summary>
        /// Name of a file to upload using multipart/form-data
        /// </summary>
        public string? FileName { get; set; }

        /// <inheritdoc cref="IInputFile.FileType"/>
        public virtual FileType FileType => FileType.Stream;

        /// <summary>
        /// Constructs a new instance of <see cref="InputFileStream"/>
        /// </summary>
        protected InputFileStream()
        { }

        /// <summary>
        /// Constructs an <see cref="InputFileStream"/> from a <see cref="Stream"/> and a file name
        /// </summary>
        /// <param name="content"><see cref="Stream"/> containing the file</param>
        /// <param name="fileName">Name of the file</param>
        public InputFileStream(Stream content, string? fileName = default)
        {
            Content = content;
            FileName = fileName;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Stream"/> to
        /// <see cref="InputFileStream"/>
        /// </summary>
        /// <param name="stream">
        /// <see cref="Stream"/> instance to be converted to <see cref="InputFileStream"/>
        /// </param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator InputFileStream?(Stream stream) =>
            stream is null
                ? default
                : new InputFileStream(stream);
    }
}
