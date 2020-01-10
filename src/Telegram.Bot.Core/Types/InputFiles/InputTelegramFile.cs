using System;
using System.IO;
using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.InputFiles
{
    /// <summary>
    /// Used for sending files to Telegram
    /// </summary>
    [DataContract]
    public class InputTelegramFile : InputFileStream
    {
        /// <summary>
        /// Id of a file that exists on Telegram servers
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string FileId { get; protected set; }

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
        /// ToDo
        /// </summary>
        protected InputTelegramFile()
        { }

        /// <summary>
        /// Constructs an <see cref="InputTelegramFile"/> from a <see cref="Stream"/>
        /// </summary>
        /// <param name="content"><see cref="Stream"/> containing the file</param>
        public InputTelegramFile(Stream content)
            : this(content, default)
        { }

        /// <summary>
        /// Constructs an <see cref="InputTelegramFile"/> from a <see cref="Stream"/> and a file name
        /// </summary>
        /// <param name="content"><see cref="Stream"/> containing the file</param>
        /// <param name="fileName">Name of the file</param>
        public InputTelegramFile(Stream content, string fileName)
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
        /// ToDo
        /// </summary>
        /// <param name="stream"></param>
        public static implicit operator InputTelegramFile(Stream stream) =>
            stream == null
                ? default
                : new InputTelegramFile(stream);

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="fileId"></param>
        public static implicit operator InputTelegramFile(string fileId) =>
            fileId == null
                ? default
                : new InputTelegramFile(fileId);
    }
}
