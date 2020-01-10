using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Delete a group sticker set from a supergroup.
    /// The bot must be an administrator in the chat for this to work and must have the appropriate admin rights.
    /// Use the field can_set_sticker_set optionally returned in getChat requests to check if the bot can use this method.
    /// </summary>
    [DataContract]
    public sealed class DeleteChatStickerSetRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target supergroup (in the format @supergroup_username)
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// Initializes a new request of type <see cref="DeleteChatStickerSetRequest"/>
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target supergroup (in the format @supergroup_username)</param>
        public DeleteChatStickerSetRequest(
            [DisallowNull] ChatId chatId)
            : base("deleteChatStickerSet")
        {
            ChatId = chatId;
        }
    }
}
