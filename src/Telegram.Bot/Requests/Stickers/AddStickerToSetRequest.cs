using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Add a new sticker to a set created by the bot. Returns <c>true</c> on success.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class AddStickerToSetRequest : FileRequestBase<bool>, IUserTargetable
    {
        /// <summary>
        /// User identifier of sticker set owner
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int UserId { get; }

        /// <summary>
        /// Sticker set name
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; }

        /// <summary>
        /// Png image with the sticker, must be up to 512 kilobytes in size, dimensions must not
        /// exceed 512px, and either width or height must be exactly 512px
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public InputOnlineFile PngSticker { get; }

        /// <summary>
        /// One or more emoji corresponding to the sticker
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Emojis { get; }

        /// <summary>
        /// One or more emoji corresponding to the sticker
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MaskPosition? MaskPosition { get; set; }

        /// <summary>
        /// Initializes a new request with userId, name pngSticker and emojis
        /// </summary>
        /// <param name="userId">User identifier of sticker set owner</param>
        /// <param name="name">Sticker set name</param>
        /// <param name="pngSticker">Png image with the sticker</param>
        /// <param name="emojis">One or more emoji corresponding to the sticker</param>
        public AddStickerToSetRequest(
            int userId,
            string name,
            InputOnlineFile pngSticker,
            string emojis)
            : base("addStickerToSet")
        {
            UserId = userId;
            Name = name;
            PngSticker = pngSticker;
            Emojis = emojis;
        }

        /// <inheritdoc />
        public override HttpContent? ToHttpContent() =>
            PngSticker.FileType == FileType.Stream
                ? ToMultipartFormDataContent("png_sticker", PngSticker)
                : base.ToHttpContent();
    }
}
