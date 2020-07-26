using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Delete a sticker from a set created by the bot. Returns <c>true</c> on success.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class DeleteStickerFromSetRequest : RequestBase<bool>
    {
        /// <summary>
        /// File identifier of the sticker
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Sticker { get; }

        /// <summary>
        /// Initializes a new request with sticker
        /// </summary>
        /// <param name="sticker">File identifier of the sticker</param>
        public DeleteStickerFromSetRequest(string sticker)
            : base("deleteStickerFromSet")
        {
            Sticker = sticker;
        }
    }
}
