using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.ReplyMarkups
{
    /// <summary>
    /// This object represents an inline keyboard that appears right next to the
    /// <see cref="Message"/> it belongs to.
    /// </summary>
    /// <remarks>
    /// Inline keyboards are currently being tested and are not available in channels yet. For
    /// now, feel free to use them in one-on-one chats or groups.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class InlineKeyboardMarkup : IReplyMarkup
    {
        /// <summary>
        /// Array of <see cref="InlineKeyboardButton"/> rows, each represented by an Array
        /// of <see cref="InlineKeyboardButton"/>.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public IEnumerable<IEnumerable<InlineKeyboardButton>> InlineKeyboard { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineKeyboardMarkup"/> class with only
        /// one keyboard button
        /// </summary>
        /// <param name="inlineKeyboardButton">Keyboard button</param>
        public InlineKeyboardMarkup(InlineKeyboardButton inlineKeyboardButton)
            : this(new[] { inlineKeyboardButton })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineKeyboardMarkup"/> class with a
        /// one-row keyboard
        /// </summary>
        /// <param name="inlineKeyboardRow">The inline keyboard row</param>
        public InlineKeyboardMarkup(IEnumerable<InlineKeyboardButton> inlineKeyboardRow)
        {
            InlineKeyboard = new[]
            {
                inlineKeyboardRow
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineKeyboardMarkup"/> class.
        /// </summary>
        /// <param name="inlineKeyboard">The inline keyboard.</param>
        [JsonConstructor]
        public InlineKeyboardMarkup(IEnumerable<IEnumerable<InlineKeyboardButton>> inlineKeyboard)
        {
            InlineKeyboard = inlineKeyboard;
        }

        /// <summary>
        /// Generate an empty inline keyboard markup
        /// </summary>
        /// <returns>Empty inline keyboard markup</returns>
        public static InlineKeyboardMarkup Empty() =>
            new InlineKeyboardMarkup(new InlineKeyboardButton[0][]);

        /// <summary>
        /// Generate an inline keyboard markup with one button
        /// </summary>
        /// <param name="button">Inline keyboard button</param>
        [return: NotNullIfNotNull("button")]
        public static implicit operator InlineKeyboardMarkup?(InlineKeyboardButton? button) =>
            button is null
                ? default
                : new InlineKeyboardMarkup(button);

        /// <summary>
        /// Generate an inline keyboard markup with one button
        /// </summary>
        /// <param name="buttonText">Text of the button</param>
        [return: NotNullIfNotNull("buttonText")]
        public static implicit operator InlineKeyboardMarkup?(string? buttonText) =>
            buttonText is null
                ? default
                // ! is needed probably because Roslyn can't correctly propagate NotNullIfNotNull
                // from implicit conversion from string to InlineKeyboardMarkup
                : new InlineKeyboardMarkup(buttonText!);

        /// <summary>
        /// Generate an inline keyboard markup from multiple buttons
        /// </summary>
        /// <param name="inlineKeyboard">Keyboard buttons</param>
        public static implicit operator InlineKeyboardMarkup?(
            IEnumerable<InlineKeyboardButton>[]? inlineKeyboard) =>
            inlineKeyboard is null
                ? null
                : new InlineKeyboardMarkup(inlineKeyboard);

        /// <summary>
        /// Generate an inline keyboard markup from multiple buttons on 1 row
        /// </summary>
        /// <param name="inlineKeyboard">Keyboard buttons</param>
        [return: NotNullIfNotNull("inlineKeyboard")]
        public static implicit operator InlineKeyboardMarkup?(
            InlineKeyboardButton[]? inlineKeyboard) =>
            inlineKeyboard is null
                ? null
                : new InlineKeyboardMarkup(inlineKeyboard);
    }
}
