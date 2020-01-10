using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Set a custom title for an administrator in a supergroup promoted by the bot.
    /// </summary>
    [DataContract]
    public sealed class SetChatAdministratorCustomTitleRequest : RequestBase<bool>
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
        public int UserId { get; set; }

        /// <summary>
        /// New custom title for the administrator; 0-16 characters, emoji are not allowed
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public string CustomTitle { get; }

        /// <summary>
        /// Initializes a new request of type <see cref="SetChatAdministratorCustomTitleRequest"/>
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target supergroup (in the format @supergroup_username)</param>
        /// <param name="userId">Unique identifier of the target user</param>
        /// <param name="customTitle">New custom title for the administrator; 0-16 characters, emoji are not allowed</param>
        public SetChatAdministratorCustomTitleRequest(
            [DisallowNull] ChatId chatId,
            int userId,
            [DisallowNull] string customTitle)
            : base("setChatAdministratorCustomTitle")
        {
            ChatId = chatId;
            UserId = userId;
            CustomTitle = customTitle;
        }
    }
}
