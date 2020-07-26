using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Set a new group sticker set for a supergroup
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SetChatStickerSetRequest : RequestBase<bool>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// (in the format @channelusername)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; set; }

        /// <summary>
        /// Name of the sticker set to be set as the group sticker set
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string StickerSetName { get; set; }

        /// <summary>
        /// Initializes a new request with chatId and new stickerSetName
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="stickerSetName">
        /// Name of the sticker set to be set as the group sticker set
        /// </param>
        public SetChatStickerSetRequest(ChatId chatId, string stickerSetName)
            : base("setChatStickerSet")
        {
            ChatId = chatId;
            StickerSetName = stickerSetName;
        }
    }
}
