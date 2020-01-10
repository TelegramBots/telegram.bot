using System.Runtime.Serialization;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// Base Class for inline results send in response to an <see cref="InlineQuery"/>
    /// </summary>
    [DataContract]
    public abstract class InlineQueryResultBase
    {
        /// <summary>
        /// Unique identifier of this result
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Id { get; private set; }

        /// <summary>
        /// Type of the result
        /// </summary>
        [DataMember(IsRequired = true)]
        public InlineQueryResultType Type { get; private set; }

        /// <summary>
        /// Inline keyboard attached to the message
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        ///  <summary>
        /// Initializes a new inline query result
        ///  </summary>
        /// <param name="type">Type of the result</param>
        protected InlineQueryResultBase(InlineQueryResultType type)
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
