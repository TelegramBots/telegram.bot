using System.Runtime.Serialization;

namespace Telegram.Bot.Types.Enums
{
    /// <summary>
    /// The type of an <see cref="Update"/>
    /// </summary>
    public enum UpdateType
    {
        /// <summary>
        /// Update Type is unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The <see cref="Update"/> contains a <see cref="Types.Message"/>.
        /// </summary>
        Message,

        /// <summary>
        /// The <see cref="Update"/> contains an <see cref="Types.InlineQuery"/>.
        /// </summary>
        [EnumMember(Value = "inline_query")]
        InlineQuery,

        /// <summary>
        /// The <see cref="Update"/> contains a <see cref="Types.ChosenInlineResult"/>.
        /// </summary>
        [EnumMember(Value = "chosen_inline_result")]
        ChosenInlineResult,

        /// <summary>
        /// The <see cref="Update"/> contains a <see cref="Types.CallbackQuery"/>
        /// </summary>
        [EnumMember(Value = "callback_query")]
        CallbackQuery,

        /// <summary>
        /// The <see cref="Update"/> contains an edited <see cref="Types.Message"/>
        /// </summary>
        [EnumMember(Value = "edited_message")]
        EditedMessage,

        /// <summary>
        /// The <see cref="Update"/> contains a channel post <see cref="Types.Message"/>
        /// </summary>
        [EnumMember(Value = "channel_post")]
        ChannelPost,

        /// <summary>
        /// The <see cref="Update"/> contains an edited channel post <see cref="Types.Message"/>
        /// </summary>
        [EnumMember(Value = "edited_channel_post")]
        EditedChannelPost,

        /// <summary>
        /// The <see cref="Update"/> contains an <see cref="ShippingQuery"/>
        /// </summary>
        [EnumMember(Value = "shipping_query")]
        ShippingQuery,

        /// <summary>
        /// The <see cref="Update"/> contains an <see cref="PreCheckoutQuery"/>
        /// </summary>
        [EnumMember(Value = "pre_checkout_query")]
        PreCheckoutQuery,

        /// <summary>
        /// The <see cref="Update"/> contains an <see cref="Poll"/>
        /// </summary>
        [EnumMember(Value = "poll")]
        Poll
    }
}
