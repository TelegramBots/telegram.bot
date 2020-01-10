using System;
using System.Collections.Generic;

namespace Telegram.Bot.Types.Enums
{
    /// <summary>
    /// Type of a <see cref="MessageEntity"/>
    /// </summary>
    public enum MessageEntityType
    {
        /// <summary>
        /// A mentioned <see cref="User"/>
        /// </summary>
        Mention,

        /// <summary>
        /// A searchable Hashtag
        /// </summary>
        Hashtag,

        /// <summary>
        /// A Bot command
        /// </summary>
        BotCommand,

        /// <summary>
        /// An url
        /// </summary>
        Url,

        /// <summary>
        /// An email
        /// </summary>
        Email,

        /// <summary>
        /// Bold text
        /// </summary>
        Bold,

        /// <summary>
        /// Italic text
        /// </summary>
        Italic,

        /// <summary>
        /// Monowidth string
        /// </summary>
        Code,

        /// <summary>
        /// Monowidth block
        /// </summary>
        Pre,

        /// <summary>
        /// Clickable text urls
        /// </summary>
        TextLink,

        /// <summary>
        /// Mentions for a <see cref="User"/> without <see cref="User.Username"/>
        /// </summary>
        TextMention,

        /// <summary>
        /// Phone number
        /// </summary>
        PhoneNumber,

        /// <summary>
        /// A cashtag (e.g. $EUR, $USD) - $ followed by the short currency code
        /// </summary>
        Cashtag,

        /// <summary>
        /// Unknown entity type
        /// </summary>
        Unknown,

        /// <summary>
        /// Underlined text
        /// </summary>
        Underline,

        /// <summary>
        /// Strikethrough text
        /// </summary>
        Strikethrough,
    }

    internal static class MessageEntityTypeExtensions
    {
        private static readonly IDictionary<string, MessageEntityType> StringToEnum =
            new Dictionary<string, MessageEntityType>
            {
                { "mention", MessageEntityType.Mention },
                { "hashtag", MessageEntityType.Hashtag },
                { "bot_command", MessageEntityType.BotCommand },
                { "url", MessageEntityType.Url },
                { "email", MessageEntityType.Email },
                { "bold", MessageEntityType.Bold },
                { "italic", MessageEntityType.Italic },
                { "code", MessageEntityType.Code },
                { "pre", MessageEntityType.Pre },
                { "text_link", MessageEntityType.TextLink },
                { "text_mention", MessageEntityType.TextMention },
                { "phone_number", MessageEntityType.PhoneNumber },
                { "cashtag", MessageEntityType.Cashtag },
                { "underline", MessageEntityType.Underline },
                { "strikethrough", MessageEntityType.Strikethrough },
            };

        private static readonly IDictionary<MessageEntityType, string> EnumToString =
            new Dictionary<MessageEntityType, string>
            {
                { MessageEntityType.Mention, "mention" },
                { MessageEntityType.Hashtag, "hashtag" },
                { MessageEntityType.BotCommand, "bot_command" },
                { MessageEntityType.Url, "url" },
                { MessageEntityType.Email, "email" },
                { MessageEntityType.Bold, "bold" },
                { MessageEntityType.Italic, "italic" },
                { MessageEntityType.Code, "code" },
                { MessageEntityType.Pre, "pre" },
                { MessageEntityType.TextLink, "text_link" },
                { MessageEntityType.TextMention, "text_mention" },
                { MessageEntityType.PhoneNumber, "phone_number" },
                { MessageEntityType.Cashtag, "cashtag" },
                { MessageEntityType.Unknown, "unknown" },
                { MessageEntityType.Underline, "underline" },
                { MessageEntityType.Strikethrough, "strikethrough" },
            };

        internal static MessageEntityType ToMessageType(this string value) =>
            StringToEnum.TryGetValue(value, out var messageEntityType)
                ? messageEntityType
                : MessageEntityType.Unknown;

        internal static string ToStringValue(this MessageEntityType value) =>
            EnumToString.TryGetValue(value, out var messageEntityType)
                ? messageEntityType
                : throw new NotSupportedException();
    }
}
