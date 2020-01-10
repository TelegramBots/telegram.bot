using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a game. Use BotFather to create and edit games, their short names will act as unique identifiers.
    /// </summary>
    [DataContract]
    public class Game
    {
        /// <summary>
        /// Title of the game.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Description of the game.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Description { get; set; }

        /// <summary>
        /// Photo that will be displayed in the game message in chats.
        /// </summary>
        [DataMember(IsRequired = true)]
        public PhotoSize[] Photo { get; set; }

        /// <summary>
        /// Brief description of the game or high scores included in the game message.
        /// Can be automatically edited to include current high scores for the game when the bot calls setGameScore, or manually edited using editMessageText. 0-4096 characters.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Text { get; set; }

        /// <summary>
        /// Special entities that appear in text, such as usernames, URLs, bot commands, etc.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MessageEntity[] TextEntities { get; set; }

        /// <summary>
        /// Animation that will be displayed in the game message in chats. Upload via BotFather.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Animation Animation { get; set; }
    }
}
