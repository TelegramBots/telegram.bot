using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a game. Use BotFather to create and edit games, their short names
    /// will act as unique identifiers.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Game
    {
        /// <summary>
        /// Title of the game.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; } = default!;

        /// <summary>
        /// Description of the game.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Description { get; set; } = default!;

        /// <summary>
        /// Photo that will be displayed in the game message in chats.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public PhotoSize[] Photo { get; set; } = default!;

        /// <summary>
        /// Brief description of the game or high scores included in the game message. Can be
        /// automatically edited to include current high scores for the game when the bot calls
        /// <see cref="SetGameScoreRequest"/>>, or manually edited using editMessageText.
        /// 0-4096 characters.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Text { get; set; }

        /// <summary>
        /// Special entities that appear in text, such as usernames, URLs, bot commands, etc.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MessageEntity[]? TextEntities { get; set; }

        /// <summary>
        /// Animation that will be displayed in the game message in chats. Upload via BotFather.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Animation? Animation { get; set; }
    }
}
