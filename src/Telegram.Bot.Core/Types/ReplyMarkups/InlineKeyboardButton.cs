using System.Runtime.Serialization;

namespace Telegram.Bot.Types.ReplyMarkups
{
    /// <summary>
    /// This object represents one button of an inline keyboard.
    /// </summary>
    [DataContract]
    public class InlineKeyboardButton : IKeyboardButton
    {
        /// <inheritdoc />
        [DataMember(IsRequired = true)]
        public string Text { get; set; }

        /// <summary>
        /// Optional. HTTP url to be opened when button is pressed
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Url { get; set; }

        /// <summary>
        /// Optional. An HTTP URL used to automatically authorize the user
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public LoginUrl LoginUrl { get; set; }

        /// <summary>
        /// Optional. Data to be sent in a callback query to the bot when button is pressed
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string CallbackData { get; set; }

        /// <summary>
        /// If set, pressing the button will prompt the user to select one of their chats, open that chat and insert the bot's username and the specified <see cref="InlineQuery"/> in the input field. Can be empty, in which case just the bot's username will be inserted.
        /// </summary>
        /// <remarks>
        /// Note: This offers an easy way for users to start using your bot in inline mode when they are currently in a private chat with it. Especially useful when combined with switchPm[...] parameters (see <see cref="TelegramBotClient.AnswerInlineQueryAsync"/>)  – in this case the user will be automatically returned to the chat they switched from, skipping the chat selection screen.
        /// </remarks>
        [DataMember(EmitDefaultValue = false)]
        public string SwitchInlineQuery { get; set; }

        /// <summary>
        /// Optional. If set, pressing the button will insert the bot's username and the specified inline query in the current chat's input field. Can be empty, in which case only the bot’s username will be inserted.
        /// </summary>
        /// <remarks>
        /// Note: This offers a quick way for the user to open your bot in inline mode in the same chat – good for selecting something from multiple options.
        /// </remarks>
        [DataMember(EmitDefaultValue = false)]
        public string SwitchInlineQueryCurrentChat { get; set; }

        /// <summary>
        /// Optional. Description of the game that will be launched when the user presses the button.
        /// </summary>
        /// <remarks>
        /// Note: This type of button must always be the first button in the first row.
        /// </remarks>
        [DataMember(EmitDefaultValue = false)]
        public CallbackGame CallbackGame { get; set; }

        /// <summary>
        /// Optional. Specify True, to send a Pay button.
        /// </summary>
        /// <remarks>
        /// Note: This type of button must always be the first button in the first row.
        /// </remarks>
        [DataMember(EmitDefaultValue = false)]
        public bool Pay { get; set; }

        /// <summary>
        /// Creates an inline keyboard button that opens a HTTP url when pressed
        /// </summary>
        /// <param name="text">Label text on the button</param>
        /// <param name="url">HTTP url to be opened when button is pressed</param>
        public static InlineKeyboardButton WithUrl(string text, string url) =>
            new InlineKeyboardButton
            {
                Text = text,
                Url = url
            };

        /// <summary>
        /// Creates an inline keyboard button that opens a HTTP URL to automatically authorize the user
        /// </summary>
        /// <param name="text">Label text on the button</param>
        /// <param name="loginUrl">An HTTP URL used to automatically authorize the user</param>
        /// <returns></returns>
        public static InlineKeyboardButton WithLoginUrl(string text, LoginUrl loginUrl) =>
            new InlineKeyboardButton
            {
                Text = text,
                LoginUrl = loginUrl
            };

        /// <summary>
        /// Creates an inline keyboard button that sends <see cref="CallbackQuery"/> to bot when pressed
        /// </summary>
        /// <param name="textAndCallbackData">Text of the button and data to be sent in a <see cref="CallbackQuery"/> to the bot when button is pressed, 1-64 bytes.</param>
        public static InlineKeyboardButton WithCallbackData(string textAndCallbackData) =>
            new InlineKeyboardButton
            {
                Text = textAndCallbackData,
                CallbackData = textAndCallbackData
            };

        /// <summary>
        /// Creates an inline keyboard button that sends <see cref="CallbackQuery"/> to bot when pressed
        /// </summary>
        /// <param name="text">Label text on the button</param>
        /// <param name="callbackData">Data to be sent in a <see cref="CallbackQuery"/> to the bot when button is pressed, 1-64 bytes</param>
        public static InlineKeyboardButton WithCallbackData(string text, string callbackData) =>
            new InlineKeyboardButton
            {
                Text = text,
                CallbackData = callbackData
            };

        /// <summary>
        /// Creates an inline keyboard button. Pressing the button will prompt the user to select one of their chats, open that chat and insert the bot's username and the specified inline query in the input field.
        /// </summary>
        /// <param name="text">Label text on the button</param>
        /// <param name="query">Inline query that appears in the input field. Can be empty, in which case just the bot's username will be inserted.</param>
        /// <returns></returns>
        public static InlineKeyboardButton WithSwitchInlineQuery(string text, string query = "") =>
            new InlineKeyboardButton
            {
                Text = text,
                SwitchInlineQuery = query
            };

        /// <summary>
        /// Creates an inline keyboard button. Pressing the button will insert the bot's username and the specified inline query in the current chat's input field.
        /// </summary>
        /// <param name="text">Label text on the button</param>
        /// <param name="query">Inline query that appears in the input field. Can be empty, in which case just the bot's username will be inserted.</param>
        public static InlineKeyboardButton WithSwitchInlineQueryCurrentChat(string text, string query = "") =>
            new InlineKeyboardButton
            {
                Text = text,
                SwitchInlineQueryCurrentChat = query
            };

        /// <summary>
        /// Creates an inline keyboard button. Pressing the button will launch the game.
        /// </summary>
        /// <param name="text">Label text on the button</param>
        /// <param name="callbackGame">Description of the game that will be launched when the user presses the button.</param>
        public static InlineKeyboardButton WithCallBackGame(string text, CallbackGame callbackGame = null) =>
            new InlineKeyboardButton
            {
                Text = text,
                CallbackGame = callbackGame ?? new CallbackGame()
            };

        /// <summary>
        /// Creates an inline keyboard button for a PayButton
        /// </summary>
        /// <param name="text">Label text on the button</param>
        public static InlineKeyboardButton WithPayment(string text) =>
            new InlineKeyboardButton
            {
                Text = text,
                Pay = true
            };

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="InlineKeyboardButton"/> with callback data
        /// </summary>
        /// <param name="textAndCallbackData">Label text and callback data of the button</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator InlineKeyboardButton(string textAndCallbackData) =>
            textAndCallbackData == null
                ? default
                : WithCallbackData(textAndCallbackData);
    }
}
