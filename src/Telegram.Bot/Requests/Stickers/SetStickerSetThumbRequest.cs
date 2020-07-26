﻿using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Use this method to set the thumbnail of a sticker set. Animated thumbnails can be set for
    /// animated sticker sets only. Returns <c>true</c> on success.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SetStickerSetThumbRequest : FileRequestBase<bool>, IUserTargetable
    {
        /// <summary>
        /// Sticker set name
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; }

        /// <summary>
        /// User identifier of the sticker set owner
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int UserId { get; }

        /// <summary>
        /// A PNG image or a TGS animation with the thumbnail
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputOnlineFile? Thumb { get; set; }

        /// <summary>
        /// Initializes a new request with sticker and position
        /// </summary>
        /// <param name="name">Sticker set name</param>
        /// <param name="userId">User identifier of the sticker set owner</param>
        /// <param name="thumb">A PNG image or a TGS animation with the thumbnail</param>
        public SetStickerSetThumbRequest(string name, int userId, InputOnlineFile? thumb = default)
            : base("setStickerSetThumb")
        {
            Name = name;
            UserId = userId;
            Thumb = thumb;
        }

        /// <inheritdoc />
        public override HttpContent? ToHttpContent()
        {
            if (Thumb?.FileType == FileType.Stream)
                return ToMultipartFormDataContent("thumb", Thumb);

            return base.ToHttpContent();
        }
    }
}
