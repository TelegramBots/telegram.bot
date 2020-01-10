namespace Telegram.Bot.Types.InlineQueryResults.Abstractions
{
    /// <summary>
    /// Represents an inline query result with thumbnail URL
    /// </summary>
    public interface IThumbnailUrlInlineQueryResult
    {
        /// <summary>
        /// URL of the static thumbnail for the result.
        /// </summary>
        string ThumbUrl { get; set; }
    }
}
