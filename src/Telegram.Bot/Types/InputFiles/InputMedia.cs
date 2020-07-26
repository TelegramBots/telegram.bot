using System.IO;
using Newtonsoft.Json;
using Telegram.Bot.Converters;
using Telegram.Bot.Types.InputFiles;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types
{
    /// <summary>
    /// Media to send in request that could be a file_id, HTTP url, or a file
    /// </summary>
    [JsonConverter(typeof(InputMediaConverter))]
    public class InputMedia : InputOnlineFile
    {
        /// <summary>
        /// Initializes media with a file to send
        /// </summary>
        /// <param name="content">File content to upload</param>
        /// <param name="fileName">Name of the file to send</param>
        public InputMedia(Stream content, string fileName)
            : base(content, fileName)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="InputMedia"/> with either a file_id or a HTTP URL
        /// </summary>
        /// <param name="value">
        /// file_id to send a file that exists on the Telegram servers or an HTTP URL for Telegram
        /// to get a file from the Internet
        /// </param>
        public InputMedia(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="InputMedia"/> with either a file_id or a HTTP URL
        /// </summary>
        /// <param name="value">
        /// file_id to send a file that exists on the Telegram servers or an HTTP URL for Telegram
        /// to get a file from the Internet
        /// </param>
        public static implicit operator InputMedia?(string? value) =>
            value is null
                ? null
                : new InputMedia(value);
    }
}
