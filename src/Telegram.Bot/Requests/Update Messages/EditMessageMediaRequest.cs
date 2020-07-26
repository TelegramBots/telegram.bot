using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Helpers;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Edit audio, document, photo, or video messages. On success the edited
    /// <see cref="Message"/> is returned.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class EditMessageMediaRequest : FileRequestBase<Message>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// (in the format @channelusername)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Identifier of the sent message
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int MessageId { get; }

        /// <summary>
        /// New media content of the message
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public InputMediaBase Media { get; }

        /// <summary>
        /// A JSON-serialized object for an inline keyboard
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlineKeyboardMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId, messageId and new media
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="messageId">Identifier of the sent message</param>
        /// <param name="media">New media content of the message</param>
        public EditMessageMediaRequest(ChatId chatId, int messageId, InputMediaBase media)
            : base("editMessageMedia")
        {
            ChatId = chatId;
            MessageId = messageId;
            Media = media;
        }

        /// <inheritdoc />
        public override HttpContent? ToHttpContent()
        {
            if (Media.Media.FileType == FileType.Stream)
            {
                var multipartContent = GenerateMultipartFormDataContent();
                multipartContent.AddContentIfInputFileStream(Media);
                return multipartContent;
            }

            return base.ToHttpContent();
        }
    }
}
