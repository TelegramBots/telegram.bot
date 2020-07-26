using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// Describes actions that a non-administrator user is allowed to take in a chat.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ChatPermissions
    {
        /// <summary>
        /// Optional. True, if the user is allowed to send text messages, contacts, locations and
        /// venues
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanSendMessages { get; set; }

        /// <summary>
        /// Optional. True, if the user is allowed to send audios, documents, photos, videos, video
        /// notes and voice notes, implies <see cref="CanSendMessages"/>
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanSendMediaMessages { get; set; }

        /// <summary>
        /// Optional. True, if the user is allowed to send polls, implies
        /// <see cref="CanSendMessages"/>
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanSendPolls { get; set; }

        /// <summary>
        /// Optional. True, if the user is allowed to send animations, games, stickers and use
        /// inline bots, implies <see cref="CanSendMediaMessages"/>
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanSendOtherMessages { get; set; }

        /// <summary>
        /// Optional. True, if the user is allowed to add web page previews to their messages,
        /// implies <see cref="CanSendMediaMessages"/>
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanAddWebPagePreviews { get; set; }

        /// <summary>
        /// Optional. True, if the user is allowed to change the chat title, photo and other
        /// settings. Ignored in public supergroups
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanChangeInfo { get; set; }

        /// <summary>
        /// Optional. True, if the user is allowed to invite new users to the chat
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanInviteUsers { get; set; }

        /// <summary>
        /// Optional. True, if the user is allowed to pin messages. Ignored in public supergroups
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanPinMessages { get; set; }
    }
}
