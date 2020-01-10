using System;
using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a Telegram user or bot.
    /// </summary>
    [DataContract]
    public class User : IEquatable<User>
    {
        /// <summary>
        /// Unique identifier for this user or bot
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Id { get; set; }

        /// <summary>
        /// True, if this user is a bot
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool IsBot { get; set; }

        /// <summary>
        /// User's or bot's first name
        /// </summary>
        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }

        /// <summary>
        /// Optional. User's or bot's last name
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string LastName { get; set; }

        /// <summary>
        /// Optional. User's or bot's username
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Username { get; set; }

        /// <summary>
        /// Optional. IETF language tag of the user's language
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string LanguageCode { get; set; }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as User);

        /// <inheritdoc />
        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Id == other.Id &&
                   IsBot == other.IsBot &&
                   FirstName == other.FirstName &&
                   LastName == other.LastName &&
                   Username == other.Username &&
                   LanguageCode == other.LanguageCode;
        }

        /// <summary>
        /// Compares two users for equality
        /// </summary>
        /// <param name="lhs">Left-hand side user in expression</param>
        /// <param name="rhs">Right-hand side user in expression</param>
        /// <returns>true if users are equal, otherwise false</returns>
        public static bool operator ==(User lhs, User rhs) =>
            lhs?.Equals(rhs) ?? ReferenceEquals(rhs, null);

        /// <summary>
        /// Compares two users for inequality
        /// </summary>
        /// <param name="lhs">Left-hand side user in expression</param>
        /// <param name="rhs">Right-hand side user in expression</param>
        /// <returns>true if users are not equal, otherwise false</returns>
        public static bool operator !=(User lhs, User rhs) => !(lhs == rhs);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyMemberInGetHashCode
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ IsBot.GetHashCode();
                hashCode = (hashCode * 397) ^ (FirstName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (LastName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Username?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (LanguageCode?.GetHashCode() ?? 0);
                // ReSharper restore NonReadonlyMemberInGetHashCode
                return hashCode;
            }
        }

        /// <inheritdoc/>
        public override string ToString() => (Username == null
                                                 ? FirstName + LastName?.Insert(0, " ")
                                                 : $"@{Username}") +
                                             $" ({Id})";
    }
}
