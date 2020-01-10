using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Set a new group sticker set for a supergroup
    /// </summary>
    public class SetChatStickerSetRequest : ChatIdRequestBase<bool>
    {
        /// <summary>
        /// Name of the sticker set to be set as the group sticker set
        /// </summary>
        [NotNull]
        public string StickerSetName { get; set; }

        /// <summary>
        /// Initializes a new request with chatId and new stickerSetName
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="stickerSetName">Name of the sticker set to be set as the group sticker set</param>
        public SetChatStickerSetRequest([NotNull] ChatId chatId, [NotNull] string stickerSetName)
            : base("setChatStickerSet")
        {
            ChatId = chatId;
            StickerSetName = stickerSetName;
        }
    }
}
