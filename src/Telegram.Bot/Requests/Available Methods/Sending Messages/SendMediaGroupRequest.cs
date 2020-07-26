using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Helpers;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send a group of photos or videos as an album. On success, an array of the sent
    /// <see cref="Message"/>s is returned.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SendMediaGroupRequest : FileRequestBase<Message[]>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// (in the format @channelusername)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// A JSON-serialized array describing photos and videos to be sent, must include
        /// 2–10 items
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public IEnumerable<IAlbumInputMedia> Media { get; }

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
        /// Initializes a request with chat_id and media
        /// </summary>
        /// <param name="chatId">ID of target chat</param>
        /// <param name="media">Media items to send</param>
        public SendMediaGroupRequest(ChatId chatId, IEnumerable<IAlbumInputMedia> media)
            : base("sendMediaGroup")
        {
            ChatId = chatId;
            Media = media;
        }

        /// <inheritdoc />
        public override HttpContent? ToHttpContent()
        {
            if (Media.All(x => x.Media.FileType != FileType.Stream))
            {
                return base.ToHttpContent();
            }

            var httpContent = GenerateMultipartFormDataContent();
            httpContent.AddContentIfInputFileStream(Media.Cast<IInputMedia>().ToArray());
            return httpContent;
        }
    }
}
