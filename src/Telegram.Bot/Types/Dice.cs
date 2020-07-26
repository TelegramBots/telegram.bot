using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a dice with random value
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Dice
    {
        /// <summary>
        /// Emoji on which the dice throw animation is based
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Emoji { get; set; } = default!;
        /// <summary>
        /// Value of the dice, 1-6 for <see cref="Telegram.Bot.Types.Enums.Emoji.Dice" /> (“🎲”) and
        /// <see cref="Telegram.Bot.Types.Enums.Emoji.Darts" /> (“🎯”) base emoji, 1-5 for
        /// <see cref="Telegram.Bot.Types.Enums.Emoji.Basketball" /> (“🏀”) base emoji
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int Value { get; set; }
    }
}
