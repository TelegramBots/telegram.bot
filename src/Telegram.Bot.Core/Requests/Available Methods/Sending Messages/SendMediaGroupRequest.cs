using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Helpers;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send a group of photos or videos as an album. On success, an array of the sent Messages is returned.
    /// </summary>
    public sealed class SendMediaGroupRequest : FileRequestBase<Message[]>,
                                                IChatMessage,
                                                INotifiableMessage,
                                                IReplyMessage
    {
        /// <inheritdoc />
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// A JSON-serialized array describing photos and videos to be sent, must include 2–10 items
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public IEnumerable<IAlbumInputMedia> Media { get; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public bool DisableNotification { get; set; }

        /// <inheritdoc />
        [DataMember(EmitDefaultValue = false)]
        public int ReplyToMessageId { get; set; }

        /// <summary>
        /// Initializes a request
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel</param>
        /// <param name="media">Media items to send</param>
        public SendMediaGroupRequest([DisallowNull] ChatId chatId, [DisallowNull] IEnumerable<IAlbumInputMedia> media)
            : base("sendMediaGroup")
        {
            ChatId = chatId;
            Media = media;
        }

        // ToDo: If there is no file stream in the request, request content should be string
        /// <inheritdoc />
        public override async ValueTask<HttpContent> ToHttpContentAsync(
            [DisallowNull] ITelegramBotJsonConverter jsonConverter,
            CancellationToken cancellationToken)
        {
            var httpContent = await GenerateMultipartFormDataContent(jsonConverter, cancellationToken);
            httpContent.AddContentIfInputFileStream(Media.Cast<IInputMedia>().ToArray());
            return httpContent;
        }
    }
}
