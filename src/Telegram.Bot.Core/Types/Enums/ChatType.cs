namespace Telegram.Bot.Types.Enums
{
    /// <summary>
    /// Type of a <see cref="Chat"/>
    /// </summary>
    public enum ChatType
    {
        /// <summary>
        /// Normal one to one <see cref="Chat"/>
        /// </summary>
        Private,

        /// <summary>
        /// Normal groupchat
        /// </summary>
        Group,

        /// <summary>
        /// A channel
        /// </summary>
        Channel,

        /// <summary>
        /// A supergroup
        /// </summary>
        Supergroup
    }
}
