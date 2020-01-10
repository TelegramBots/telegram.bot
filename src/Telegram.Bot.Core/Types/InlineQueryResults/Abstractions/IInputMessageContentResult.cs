namespace Telegram.Bot.Types.InlineQueryResults.Abstractions
{
    /// <summary>
    /// Represents an inline query result with message content
    /// </summary>
    public interface IInputMessageContentResult
    {
        /// <summary>
        /// Content of the message to be sent
        /// </summary>
        InputMessageContentBase InputMessageContent { get; set; }
    }
}
