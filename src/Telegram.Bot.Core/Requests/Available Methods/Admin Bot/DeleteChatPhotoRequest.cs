using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Delete a chat photo.
    /// Photos can't be changed for private chats.
    /// The bot must be an administrator in the chat for this to work and must have the appropriate admin rights.
    /// </summary>
    [DataContract]
    public sealed class DeleteChatPhotoRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel (in the format @channel_username)
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// Initializes a new request of type <see cref="DeleteChatPhotoRequest"/>
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel (in the format @channel_username)</param>
        public DeleteChatPhotoRequest(
            [DisallowNull] ChatId chatId)
            : base("deleteChatPhoto")
        {
            ChatId = chatId;
        }
    }
}
