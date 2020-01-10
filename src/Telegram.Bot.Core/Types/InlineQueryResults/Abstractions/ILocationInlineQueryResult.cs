namespace Telegram.Bot.Types.InlineQueryResults.Abstractions
{
    /// <summary>
    /// Represents an inline query result with location
    /// </summary>
    public interface ILocationInlineQueryResult
    {
        /// <summary>
        /// Latitude of the location in degrees
        /// </summary>
        float Latitude { get; set; }

        /// <summary>
        /// Longitude of the location in degrees
        /// </summary>
        float Longitude { get; set; }
    }
}
