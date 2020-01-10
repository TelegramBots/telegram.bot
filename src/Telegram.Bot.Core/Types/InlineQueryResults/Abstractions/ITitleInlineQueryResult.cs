namespace Telegram.Bot.Types.InlineQueryResults.Abstractions
{
    /// <summary>
    /// Represents an inline query result with title
    /// </summary>
    public interface ITitleInlineQueryResult
    {
        /// <summary>
        /// Title of the result
        /// </summary>
        string Title { get; set; }
    }
}
