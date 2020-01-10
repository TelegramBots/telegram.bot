namespace Telegram.Bot.Types.ReplyMarkups
{
    /// <summary>
    /// Marker interface for a regular or inline button of the reply keyboard
    /// </summary>
    public interface IKeyboardButton
    {
        /// <summary>
        /// Label text on the button
        /// </summary>
        string Text { get; set; }
    }
}
