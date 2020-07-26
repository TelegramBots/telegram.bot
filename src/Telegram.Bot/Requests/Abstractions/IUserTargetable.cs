namespace Telegram.Bot.Requests.Abstractions
{
    /// <summary>
    /// Represents a request having <see cref="UserId"/> parameter
    /// </summary>
    public interface IUserTargetable
    {
        /// <summary>
        /// User identifier
        /// </summary>
        int UserId { get; }
    }
}
