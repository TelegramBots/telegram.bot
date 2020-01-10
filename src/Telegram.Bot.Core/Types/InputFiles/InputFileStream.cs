using System.IO;
using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.InputFiles
{
    /// <summary>
    /// Used for sending files to Telegram
    /// </summary>
    [DataContract]
    public class InputFileStream : IInputFile
    {
        /// <summary>
        /// File content to upload
        /// </summary>
        public Stream Content { get; protected set; }

        /// <summary>
        /// Name of a file to upload using multipart/form-data
        /// </summary>
        public string FileName { get; set; }

        /// <inheritdoc cref="IInputFile.FileType"/>
        public virtual FileType FileType => FileType.Stream;

        /// <summary>
        /// ToDo
        /// </summary>
        protected InputFileStream()
        { }

        /// <summary>
        /// Constructs an <see cref="InputFileStream"/> from a <see cref="Stream"/>
        /// </summary>
        /// <param name="content"><see cref="Stream"/> containing the file</param>
        public InputFileStream(Stream content)
            : this(content, default)
        { }

        /// <summary>
        /// Constructs an <see cref="InputFileStream"/> from a <see cref="Stream"/> and a file name
        /// </summary>
        /// <param name="content"><see cref="Stream"/> containing the file</param>
        /// <param name="fileName">Name of the file</param>
        public InputFileStream(Stream content, string fileName)
        {
            Content = content;
            FileName = fileName;
        }

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="stream"></param>
        public static implicit operator InputFileStream(Stream stream) =>
            stream == null
                ? default
                : new InputFileStream(stream);
    }
}
