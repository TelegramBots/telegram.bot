using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Helpers;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send video files, Telegram clients support mp4 videos
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SendVideoRequest : FileRequestBase<Message>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// (in the format @channelusername)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Video file to send
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public InputOnlineFile Video { get; }

        /// <summary>
        /// Duration of the video in seconds
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Duration { get; set; }

        /// <summary>
        /// Video width
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Width { get; set; }

        /// <summary>
        /// Video height
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Height { get; set; }

        /// <summary>
        /// Video caption (may also be used when resending videos by file_id), 0-1024 characters
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Caption { get; set; }

        /// <summary>
        /// Change, if you want Telegram apps to show bold, italic, fixed-width text or inline
        /// URLs in your bot's message
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ParseMode? ParseMode { get; set; }

        /// <summary>
        /// Pass True, if the uploaded video is suitable for streaming
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? SupportsStreaming { get; set; }

        /// <summary>
        /// Thumbnail of the file sent. The thumbnail should be in JPEG format and less than
        /// 200KB in size. A thumbnail's width and height should not exceed 90. Thumbnails can't
        /// be reused and can be only uploaded as a new file.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InputMedia? Thumb { get; set; }

        /// <summary>
        /// Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? DisableNotification { get; set; }

        /// <summary>
        /// If the message is a reply, ID of the original message.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ReplyToMessageId { get; set; }

        /// <summary>
        /// A JSON-serialized object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReplyMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId and video
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="video">Video to send</param>
        public SendVideoRequest(ChatId chatId, InputOnlineFile video)
            : base("sendVideo")
        {
            ChatId = chatId;
            Video = video;
        }

        /// <inheritdoc />
        public override HttpContent? ToHttpContent()
        {
            HttpContent? httpContent;
            if (Video.FileType == FileType.Stream || Thumb?.FileType == FileType.Stream)
            {
                var multipartContent = GenerateMultipartFormDataContent("video", "thumb");
                if (Video.FileType == FileType.Stream)
                {
                    multipartContent.AddStreamContent(Video.Content!, "video", Video.FileName!);
                }

                if (Thumb?.FileType == FileType.Stream)
                {
                    multipartContent.AddStreamContent(Thumb.Content!, "thumb", Thumb.FileName!);
                }

                httpContent = multipartContent;
            }
            else
            {
                httpContent = base.ToHttpContent();
            }

            return httpContent;
        }
    }
}
