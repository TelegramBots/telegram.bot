using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Telegram.Bot.Converters;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// Represents a ChatId
    /// </summary>
    [JsonConverter(typeof(ChatIdConverter))]
    public class ChatId
    {
        private static readonly Regex NameValidation = new Regex("^@[a-zA-Z0-9_]{5,32}$");

        /// <summary>
        /// Unique identifier for the chat
        /// </summary>
        public readonly long Identifier;

        /// <summary>
        /// Username of the channel (in the format @channelusername)
        /// </summary>
        public readonly string? Username;

        /// <summary>
        /// Create a <see cref="ChatId"/> using an identifier
        /// </summary>
        /// <param name="identifier">The Identifier</param>
        public ChatId(long identifier)
        {
            Identifier = identifier;
        }

        /// <summary>
        /// Create a <see cref="ChatId"/> using an identifier
        /// </summary>
        /// <param name="chatId">The Identifier</param>
        public ChatId(int chatId)
        {
            Identifier = chatId;
        }

        /// <summary>
        /// Create a <see cref="ChatId"/> using an user name
        /// </summary>
        /// <param name="username">The user name</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ChatId(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (NameValidation.IsMatch(username))
            {
                Username = username;
            }
            else if (long.TryParse(username, out long identifier))
            {
                Identifier = identifier;
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    nameof(username),
                    username,
                    $"{nameof(username)} value has to start with '@' symbol and be 5 to 32 characters long or be a valid long value."
                );
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to the current object; otherwise,
        /// <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => ((string)this).Equals(obj);

        /// <summary>
        /// Gets the hash code of this object
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => ((string)this).GetHashCode();

        /// <summary>
        /// Create a <c>string</c> out of a <see cref="ChatId"/>
        /// </summary>
        /// <returns>The <see cref="ChatId"/> as <c>string</c></returns>
        public override string ToString() => this;

        /// <summary>
        /// Create a <see cref="ChatId"/> out of an identifier
        /// </summary>
        /// <param name="identifier">The identifier</param>
        public static implicit operator ChatId(long identifier) => new ChatId(identifier);

        /// <summary>
        /// Create a <see cref="ChatId"/> out of an identifier
        /// </summary>
        /// <param name="chatId">The identifier</param>
        public static implicit operator ChatId(int chatId) => new ChatId(chatId);

        /// <summary>
        /// Create a <see cref="ChatId"/> out of an user name
        /// </summary>
        /// <param name="username">The user name</param>
        public static implicit operator ChatId(string username) => new ChatId(username);

        /// <summary>
        /// Create a <c>string</c> out of a <see cref="ChatId"/>
        /// </summary>
        /// <param name="chatId">The <see cref="ChatId"/>The ChatId</param>
        public static implicit operator string(ChatId chatId) =>
            chatId.Username ?? chatId.Identifier.ToString();

        /// <summary>
        /// Convert a Chat Object to a <see cref="ChatId"/>
        /// </summary>
        /// <param name="chat"></param>
        public static implicit operator ChatId(Chat chat) =>
            chat.Id != default ? chat.Id : new ChatId($"@{chat.Username}");
    }
}
