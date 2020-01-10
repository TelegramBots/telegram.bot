using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types.InlineQueryResults.Abstractions
{
    /// <summary>
    /// Represents an inline query result with caption
    /// </summary>
    public interface ICaptionInlineQueryResult
    {
        /// <summary>
        /// Optional. Caption of the result to be sent, 0-1024 characters.
        /// </summary>
        string Caption { get; set; }

        /// <summary>
        /// Optional. Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in the media caption.
        /// </summary>
        ParseMode ParseMode { get; set; }
    }
}
