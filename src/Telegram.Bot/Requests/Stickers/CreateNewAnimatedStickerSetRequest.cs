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
    /// Create a new animated sticker set owned by a user. The bot will be able to edit the
    /// created sticker set. Returns True on success.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class CreateNewAnimatedStickerSetRequest : FileRequestBase<bool>, IUserTargetable
    {
        /// <summary>
        /// User identifier of sticker set owner
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int UserId { get; }

        /// <summary>
        /// Short name of sticker set, to be used in t.me/addstickers/ URLs (e.g., animals).
        /// Can contain only english letters, digits and underscores. Must begin with a letter,
        /// can't contain consecutive underscores and must end in “_by_[bot username]”.
        /// [bot_username] is case insensitive. 1-64 characters.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Name { get; }

        /// <summary>
        /// Sticker set title, 1-64 characters
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Title { get; }

        /// <summary>
        /// Tgs animation with the sticker.
        /// See https://core.telegram.org/animated_stickers#technical-requirements for technical
        /// requirements
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public InputFileStream TgsSticker { get; }

        /// <summary>
        /// One or more emoji corresponding to the sticker
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Emojis { get; }

        /// <summary>
        /// Pass <c>true</c>, if a set of mask stickers should be created
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ContainsMasks { get; set; }

        /// <summary>
        /// One or more emoji corresponding to the sticker
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MaskPosition? MaskPosition { get; set; }

        /// <summary>
        /// Initializes a new request with userId, name, tgsSticker and emojis
        /// </summary>
        /// <param name="userId">User identifier of sticker set owner</param>
        /// <param name="name">Sticker set name</param>
        /// <param name="title">Sticker set title, 1-64 characters</param>
        /// <param name="tgsSticker">Tgs animation with the sticker</param>
        /// <param name="emojis">One or more emoji corresponding to the sticker</param>
        public CreateNewAnimatedStickerSetRequest(
            int userId,
            string name,
            string title,
            InputFileStream tgsSticker,
            string emojis)
            : base("createNewStickerSet")
        {
            UserId = userId;
            Name = name;
            Title = title;
            TgsSticker = tgsSticker;
            Emojis = emojis;
        }

        /// <inheritdoc />
        public override HttpContent? ToHttpContent()
        {
            if (TgsSticker != null)
                return ToMultipartFormDataContent("tgs_sticker", TgsSticker);

            return base.ToHttpContent();
        }
    }
}
