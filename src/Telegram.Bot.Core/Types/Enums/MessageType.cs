using System;
using System.Runtime.Serialization;

namespace Telegram.Bot.Types.Enums
{
    /// <summary>
    /// The type of a <see cref="Message"/>
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// The <see cref="Message"/> is unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The <see cref="Message"/> contains text
        /// </summary>
        Text,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="PhotoSize"/>
        /// </summary>
        Photo,

        /// <summary>
        /// The <see cref="Message"/> contains an <see cref="Types.Audio"/>
        /// </summary>
        Audio,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Types.Video"/>
        /// </summary>
        Video,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Types.Voice"/>
        /// </summary>
        Voice,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Types.Document"/>
        /// </summary>
        Document,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Types.Sticker"/>
        /// </summary>
        Sticker,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Types.Location"/>
        /// </summary>
        Location,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Types.Contact"/>
        /// </summary>
        Contact,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Types.Venue"/>
        /// </summary>
        Venue,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Types.Game"/>
        /// </summary>
        Game,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Types.VideoNote"/>
        /// </summary>
        [EnumMember(Value = "video_note")]
        VideoNote,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Invoice"/>
        /// </summary>
        Invoice,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="SuccessfulPayment"/>
        /// </summary>
        [EnumMember(Value = "successful_payment")]
        SuccessfulPayment,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Message.ConnectedWebsite"/>
        /// </summary>
        [EnumMember(Value = "website_connected")]
        WebsiteConnected,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Message.NewChatMembers"/>
        /// </summary>
        [EnumMember(Value = "chat_members_added")]
        ChatMembersAdded,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Message.LeftChatMember"/>
        /// </summary>
        [EnumMember(Value = "chat_member_left")]
        ChatMemberLeft,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Message.NewChatTitle"/>
        /// </summary>
        [EnumMember(Value = "chat_title_changed")]
        ChatTitleChanged,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Message.NewChatPhoto"/>
        /// </summary>
        [EnumMember(Value = "chat_photo_changed")]
        ChatPhotoChanged,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Message.PinnedMessage"/>
        /// </summary>
        [EnumMember(Value = "message_pinned")]
        MessagePinned,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Message.DeleteChatPhoto"/>
        /// </summary>
        [EnumMember(Value = "chat_photo_deleted")]
        ChatPhotoDeleted,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Message.GroupChatCreated"/>
        /// </summary>
        [EnumMember(Value = "group_created")]
        GroupCreated,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Message.SupergroupChatCreated"/>
        /// </summary>
        [EnumMember(Value = "supergroup_created")]
        SupergroupCreated,

        /// <summary>
        /// The <see cref="Message"/> contains a <see cref="Message.ChannelChatCreated"/>
        /// </summary>
        [EnumMember(Value = "channel_created")]
        ChannelCreated,

        /// <summary>
        /// The <see cref="Message"/> contains non-default <see cref="Message.MigrateFromChatId"/>
        /// </summary>
        [EnumMember(Value = "migrated_to_supergroup")]
        MigratedToSupergroup,

        /// <summary>
        /// The <see cref="Message"/> contains non-default <see cref="Message.MigrateToChatId"/>
        /// </summary>
        [EnumMember(Value = "migrated_from_group")]
        MigratedFromGroup,

        /// <summary>
        /// The <see cref="Message"/> contains non-default <see cref="Message.Animation"/>
        /// </summary>
        [Obsolete("Check if Message.Animation has value instead")]
        Animation,

        /// <summary>
        /// The <see cref="Message"/> contains <see cref="Message.Poll"/>
        /// </summary>
        [EnumMember(Value = "poll")]
        Poll,
    }
}
