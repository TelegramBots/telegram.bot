using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Types
{
    /// <summary>
    /// Represents a video to be sent
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InputMediaVideo : InputMediaBase, IInputMediaThumb, IAlbumInputMedia
    {
        /// <summary>
        /// Optional. Video width
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Width { get; set; }

        /// <summary>
        /// Optional. Video height
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Height { get; set; }

        /// <summary>
        /// Optional. Video duration
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Duration { get; set; }

        /// <inheritdoc />
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputMedia? Thumb { get; set; }

        /// <summary>
        /// Optional. Pass True, if the uploaded video is suitable for streaming
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? SupportsStreaming { get; set; }

        /// <summary>
        /// Initializes a new video media to send with an <see cref="InputMedia"/>
        /// </summary>
        public InputMediaVideo(InputMedia media)
            : base(media, "video")
        {
        }
    }
}
