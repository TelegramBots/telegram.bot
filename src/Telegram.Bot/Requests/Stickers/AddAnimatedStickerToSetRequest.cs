﻿using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Add a new animated sticker to a set created by the bot. Returns <c>true</c> on success.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class AddAnimatedStickerToSetRequest : FileRequestBase<bool>, IUserTargetable
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
        /// Tgs animation with the sticker.
        /// See https://core.telegram.org/animated_stickers#technical-requirements for
        /// technical requirements
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public InputFileStream TgsSticker { get; }

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
        /// Initializes a new request with userId, name tgsSticker and emojis
        /// </summary>
        /// <param name="userId">User identifier of sticker set owner</param>
        /// <param name="name">Sticker set name</param>
        /// <param name="tgsSticker">Tgs animation with the sticker</param>
        /// <param name="emojis">One or more emoji corresponding to the sticker</param>
        public AddAnimatedStickerToSetRequest(
            int userId,
            string name,
            InputFileStream tgsSticker,
            string emojis)
            : base("addStickerToSet")
        {
            UserId = userId;
            Name = name;
            TgsSticker = tgsSticker;
            Emojis = emojis;
        }

        /// <inheritdoc />
        public override HttpContent? ToHttpContent()
        {
            if (TgsSticker != null) return ToMultipartFormDataContent("tgs_sticker", TgsSticker);

            return base.ToHttpContent();
        }
    }
}
