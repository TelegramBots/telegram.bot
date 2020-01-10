namespace Telegram.Bot.Types.Enums
{
    /// <summary>
    /// ChatMember status
    /// </summary>
    public enum ChatMemberStatus
    {
        /// <summary>
        /// Creator of the <see cref="Chat"/>
        /// </summary>
        Creator,

        /// <summary>
        /// Administrator of the <see cref="Chat"/>
        /// </summary>
        Administrator,

        /// <summary>
        /// Normal member of the <see cref="Chat"/>
        /// </summary>
        Member,

        /// <summary>
        /// A <see cref="User"/> who left the <see cref="Chat"/>
        /// </summary>
        Left,

        /// <summary>
        /// A <see cref="User"/> who was kicked from the <see cref="Chat"/>
        /// </summary>
        Kicked,

        /// <summary>
        /// A <see cref="User"/> who is restricted in the <see cref="Chat"/>
        /// </summary>
        Restricted
    }
}
