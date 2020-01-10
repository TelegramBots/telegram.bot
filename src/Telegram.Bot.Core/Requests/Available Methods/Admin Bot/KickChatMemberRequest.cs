using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Kick a user from a group, a supergroup or a channel.
    /// In the case of supergroups and channels, the user will not be able to return to the group on their own
    /// using invite links, etc., unless unbanned first.
    /// The bot must be an administrator in the chat for this to work and must have the appropriate admin rights.
    /// </summary>
    [DataContract]
    public sealed class KickChatMemberRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the target group or username of the target supergroup or channel (in the format @channel_username)
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// Unique identifier of the target user
        /// </summary>
        [DataMember(IsRequired = true)]
        public int UserId { get; }

        /// <summary>
        /// Date when the user will be unbanned.
        /// If user is banned for more than 366 days or less than 30 seconds from the current time
        /// they are considered to be banned forever
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public DateTime UntilDate { get; set; }

        /// <summary>
        /// Initializes a new request of type <see cref="KickChatMemberRequest"/>
        /// </summary>
        /// <param name="chatId">Unique identifier for the target group or username of the target supergroup or channel (in the format @channel_username)</param>
        /// <param name="userId">Unique identifier of the target user</param>
        public KickChatMemberRequest(
            [DisallowNull] ChatId chatId,
            int userId)
            : base("kickChatMember")
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
