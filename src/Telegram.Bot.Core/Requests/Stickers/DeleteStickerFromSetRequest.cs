

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Delete a sticker from a set created by the bot. Returns True on success.
    /// </summary>
    public class DeleteStickerFromSetRequest : RequestBase<bool>
    {
        /// <summary>
        /// File identifier of the sticker
        /// </summary>
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
