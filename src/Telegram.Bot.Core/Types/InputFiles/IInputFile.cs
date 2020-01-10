using FileType = Telegram.Bot.Types.Enums.FileType;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents the contents of a file to be uploaded. Must be posted using multipart/form-data in the usual way that files are uploaded via the browser
    /// </summary>
    public interface IInputFile
    {
        /// <summary>
        /// Type of file to send
        /// </summary>
        FileType FileType { get; }
    }
}
