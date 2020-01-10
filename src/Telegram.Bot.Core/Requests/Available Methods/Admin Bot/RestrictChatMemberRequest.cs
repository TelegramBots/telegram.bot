using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Restrict a user in a supergroup.
    /// The bot must be an administrator in the supergroup for this to work and must have the appropriate admin rights.
    /// Pass True for all permissions to lift restrictions from a user.
    /// </summary>
    [DataContract]
    public sealed class RestrictChatMemberRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target supergroup (in the format @supergroup_username)
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// Unique identifier of the target user
        /// </summary>
        [DataMember(IsRequired = true)]
        public int UserId { get; }

        /// <summary>
        /// New user permissions
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public ChatPermissions Permissions { get; }

        /// <summary>
        /// Date when restrictions will be lifted for the user.
        /// If user is restricted for more than 366 days or less than 30 seconds from the current time,
        /// they are considered to be restricted forever.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public DateTime UntilDate { get; set; }

        /// <summary>
        /// Initializes a new request of type <see cref="RestrictChatMemberRequest"/>
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target supergroup (in the format @supergroup_username)</param>
        /// <param name="userId">Unique identifier of the target user</param>
        /// <param name="permissions">New user permissions</param>
        public RestrictChatMemberRequest(
            [DisallowNull] ChatId chatId,
            int userId,
            [DisallowNull] ChatPermissions permissions)
            : base("restrictChatMember")
        {
            ChatId = chatId;
            UserId = userId;
            Permissions = permissions;
        }
    }
}
