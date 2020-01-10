namespace Telegram.Bot.Types.InlineQueryResults.Abstractions
{
    /// <summary>
    /// Represents an inline query result with tumbnail
    /// </summary>
    public interface IThumbnailInlineQueryResult : IThumbnailUrlInlineQueryResult
    {
        /// <summary>
        /// Optional. Thumbnail width.
        /// </summary>
        int ThumbWidth { get; set; }

        /// <summary>
        /// Optional. Thumbnail height.
        /// </summary>
        int ThumbHeight { get; set; }
    }
}
