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
    public class InputOnlineFile : InputTelegramFile
    {
        /// <summary>
        /// HTTP URL for Telegram to get a file from the Internet
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Url { get; protected set; }

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
        /// Constructs an <see cref="InputOnlineFile"/> from a <see cref="Stream"/>
        /// </summary>
        /// <param name="content"><see cref="Stream"/> containing the file</param>
        public InputOnlineFile(Stream content)
            : this(content, default)
        {
        }

        /// <summary>
        /// Constructs an <see cref="InputOnlineFile"/> from a <see cref="Stream"/> and a file name
        /// </summary>
        /// <param name="content"><see cref="Stream"/> containing the file</param>
        /// <param name="fileName">Name of the file</param>
        public InputOnlineFile(Stream content, string fileName)
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
            if (Uri.TryCreate(value, UriKind.Absolute, out Uri _))
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
        /// ToDo
        /// </summary>
        /// <param name="stream"></param>
        public static implicit operator InputOnlineFile(Stream stream) =>
            stream == null
                ? default
                : new InputOnlineFile(stream);

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator InputOnlineFile(string value) =>
            value == null
                ? default
                : new InputOnlineFile(value);
    }
}
