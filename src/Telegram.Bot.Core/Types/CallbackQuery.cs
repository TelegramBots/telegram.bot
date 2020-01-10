using System.Runtime.Serialization;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents an incoming callback query from a <see cref="InlineKeyboardButton"/>. If the button that originated the query was attached to a <see cref="Message"/> sent by the bot, the field message will be presented. If the button was attached to a message sent via the bot (in inline mode), the field <see cref="InlineMessageId"/> will be presented.
    /// </summary>
    [DataContract]
    public class CallbackQuery
    {
        /// <summary>
        /// Unique identifier for this query
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Id { get; set; }

        /// <summary>
        /// Sender
        /// </summary>
        [DataMember(IsRequired = true)]
        public User From { get; set; }

        /// <summary>
        /// Optional. Description with the callback button that originated the query. Note that message content and message date will not be available if the message is too old
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Message Message { get; set; }

        /// <summary>
        /// Optional. Identifier of the message sent via the bot in inline mode, that originated the query
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string InlineMessageId { get; set; }

        /// <summary>
        /// Identifier, uniquely corresponding to the chat to which the message with the callback button was sent. Useful for high scores in games.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string ChatInstance { get; set; }

        /// <summary>
        /// Data associated with the callback button.
        /// </summary>
        /// <remarks>
        /// Be aware that a bad client can send arbitrary data in this field.
        /// </remarks>
        [DataMember(EmitDefaultValue = false)]
        public string Data { get; set; }

        /// <summary>
        /// Optional. Short name of a <see cref="Game"/> to be returned, serves as the unique identifier for the game.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string GameShortName { get; set; }

        /// <summary>
        /// Indicates if the User requests a Game
        /// </summary>
        public bool IsGameQuery => GameShortName != default;
    }
}
