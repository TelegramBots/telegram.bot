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
    /// Set a new profile photo for the chat. Photos can't be changed for private chats. The bot
    /// must be an administrator in the chat for this to work and must have the appropriate admin
    /// rights.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SetChatPhotoRequest : FileRequestBase<bool>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// (in the format @channelusername)
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// New chat photo
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public InputFileStream Photo { get; }

        /// <summary>
        /// Initializes a new request with chatId and photo
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="photo">New chat photo</param>
        public SetChatPhotoRequest(ChatId chatId, InputFileStream photo)
            : base("setChatPhoto")
        {
            ChatId = chatId;
            Photo = photo;
        }

        /// <inheritdoc />
        public override HttpContent? ToHttpContent() =>
            Photo.FileType == FileType.Stream
                ? ToMultipartFormDataContent("photo", Photo)
                : base.ToHttpContent();
    }
}
