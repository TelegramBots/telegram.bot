using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Promote or demote a user in a supergroup or a channel.
    /// The bot must be an administrator in the chat for this to work and must have the appropriate admin rights.
    /// Pass False for all boolean parameters to demote a user.
    /// </summary>
    [DataContract]
    public sealed class PromoteChatMemberRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel (in the format @channel_username)
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// Unique identifier of the target user
        /// </summary>
        [DataMember(IsRequired = true)]
        public int UserId { get; }

        /// <summary>
        /// Pass True, if the administrator can change chat title, photo and other settings
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool? CanChangeInfo { get; set; }

        /// <summary>
        /// Pass True, if the administrator can create channel posts, channels only
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool? CanPostMessages { get; set; }

        /// <summary>
        /// Pass True, if the administrator can edit messages of other users and can pin messages, channels only
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool? CanEditMessages { get; set; }

        /// <summary>
        /// Pass True, if the administrator can delete messages of other users
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool? CanDeleteMessages { get; set; }

        /// <summary>
        /// Pass True, if the administrator can invite new users to the chat
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool? CanInviteUsers { get; set; }

        /// <summary>
        /// Pass True, if the administrator can restrict, ban or unban chat members
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool? CanRestrictMembers { get; set; }

        /// <summary>
        /// Pass True, if the administrator can pin messages, supergroups only
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool? CanPinMessages { get; set; }

        /// <summary>
        /// Pass True, if the administrator can add new administrators with a subset of his own privileges or
        /// demote administrators that he has promoted, directly or indirectly (promoted by administrators
        /// that were appointed by him)
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool? CanPromoteMembers { get; set; }

        /// <summary>
        /// Initializes a new request of type <see cref="PromoteChatMemberRequest"/>
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel (in the format @channel_username)</param>
        /// <param name="userId">Unique identifier of the target user</param>
        public PromoteChatMemberRequest(
            [DisallowNull] ChatId chatId,
            int userId)
            : base("promoteChatMember")
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}
