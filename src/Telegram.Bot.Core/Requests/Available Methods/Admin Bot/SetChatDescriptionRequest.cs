using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Change the description of a group, a supergroup or a channel.
    /// The bot must be an administrator in the chat for this to work and must have the appropriate admin rights.
    /// </summary>
    [DataContract]
    public sealed class SetChatDescriptionRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel (in the format @channel_username)
        /// </summary>
        [DataMember(IsRequired = true), NotNull]
        public ChatId ChatId { get; }

        /// <summary>
        /// New chat description, 0-255 characters
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string? Description { get; set; }

        /// <summary>
        /// Initializes a new request of type <see cref="SetChatDescriptionRequest"/>
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel (in the format @channel_username)</param>
        /// <param name="description">New chat description, 0-255 characters</param>
        public SetChatDescriptionRequest(
            [DisallowNull] ChatId chatId,
            [AllowNull] string? description = default)
            : base("setChatDescription")
        {
            ChatId = chatId;
            Description = description;
        }
    }
}
