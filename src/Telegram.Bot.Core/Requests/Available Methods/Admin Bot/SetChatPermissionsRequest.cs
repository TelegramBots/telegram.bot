using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Use this method to set default chat permissions for all members. The bot must be an administrator in the group or a supergroup for this to work and must have the can_restrict_members admin rights. Returns True on success.
    /// </summary>
    public sealed class SetChatPermissionsRequest : ChatIdRequestBase<bool>
    {
        /// <summary>
        /// New default chat permissions
        /// </summary>
        [NotNull]
        public ChatPermissions Permissions { get; set; }

        /// <summary>
        /// Initializes a new request with specified <see cref="ChatId"/>
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="permissions">New default chat permissions</param>
        public SetChatPermissionsRequest([NotNull] ChatId chatId, [NotNull] ChatPermissions permissions)
            : base("setChatPermissions")
        {
            ChatId = chatId;
            Permissions = permissions;
        }
    }
}
