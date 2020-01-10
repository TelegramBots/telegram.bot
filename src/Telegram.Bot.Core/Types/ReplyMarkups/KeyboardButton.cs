using System.Runtime.Serialization;

namespace Telegram.Bot.Types.ReplyMarkups
{
    /// <summary>
    /// This object represents one button of the reply keyboard. For simple text buttons String can be used instead of this object to specify text of the button.
    /// </summary>
    [DataContract]
    public class KeyboardButton : IKeyboardButton
    {
        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string Text { get; set; }

        /// <summary>
        /// If <c>true</c>, the user's current location will be sent when the button is pressed. Available in private chats only
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool RequestLocation { get; set; }

        /// <summary>
        /// If <c>true</c>, the user's phone number will be sent as a contact when the button is pressed. Available in private chats only
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool RequestContact { get; set; }

        /// <summary>
        /// Initializes a new <see cref="KeyboardButton"/>
        /// </summary>
        public KeyboardButton()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardButton"/> class.
        /// </summary>
        /// <param name="text">Label text on the button</param>
        public KeyboardButton(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Generate a keyboard button to request for contact
        /// </summary>
        /// <param name="text">Button's text</param>
        /// <returns>Keyboard button</returns>
        public static KeyboardButton WithRequestContact(string text) =>
            new KeyboardButton(text) { RequestContact = true };

        /// <summary>
        /// Generate a keyboard button to request for location
        /// </summary>
        /// <param name="text">Button's text</param>
        /// <returns>Keyboard button</returns>
        public static KeyboardButton WithRequestLocation(string text) =>
            new KeyboardButton(text) { RequestLocation = true };

        /// <summary>
        /// Generate a keyboard button from text
        /// </summary>
        /// <param name="text">Button's text</param>
        /// <returns>Keyboard button</returns>
        public static implicit operator KeyboardButton(string text)
            => new KeyboardButton(text);
    }
}
