using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Base Class for inline results send in response to an <see cref="InlineQuery"/>
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract class InlineQueryResultBase
    {
        /// <summary>
        /// Unique identifier of this result
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Id { get; private set; }

        /// <summary>
        /// Type of the result
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public InlineQueryResultType Type { get; private set; }

        /// <summary>
        /// Inline keyboard attached to the message
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InlineKeyboardMarkup? ReplyMarkup { get; set; }

        ///  <summary>
        /// Initializes a new inline query result
        ///  </summary>
        /// <param name="type">Type of the result</param>
#pragma warning disable 8618
        protected InlineQueryResultBase(InlineQueryResultType type)
#pragma warning restore 8618
        {
            Type = type;
        }

        ///  <summary>
        /// Initializes a new inline query result
        ///  </summary>
        /// <param name="type">Type of the result</param>
        /// <param name="id">Unique identifier of this result</param>
        protected InlineQueryResultBase(InlineQueryResultType type, string id)
            : this(type)
        {
            Id = id;
        }
    }
}
