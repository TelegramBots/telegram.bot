using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Passport;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object represents a message.
    /// </summary>
    [DataContract]
    public class Message
    {
        /// <summary>
        /// Unique message identifier
        /// </summary>
        [DataMember(IsRequired = true)]
        public int MessageId { get; set; }

        /// <summary>
        /// Sender
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public User From { get; set; }

        /// <summary>
        /// Date the message was sent
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Conversation the message belongs to
        /// </summary>
        [DataMember(IsRequired = true)]
        public Chat Chat { get; set; }

        /// <summary>
        /// Indicates whether this message is a forwarded message
        /// </summary>
        [Obsolete("Check ForwardFrom and ForwardFromChat properties instead")]
        public bool IsForwarded => ForwardFrom != null;

        /// <summary>
        /// Optional. For forwarded messages, sender of the original message
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public User ForwardFrom { get; set; }

        /// <summary>
        /// Optional. For messages forwarded from a channel, information about the original channel
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Chat ForwardFromChat { get; set; }

        /// <summary>
        /// Optional. For forwarded channel posts, identifier of the original message in the channel
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int ForwardFromMessageId { get; set; }

        /// <summary>
        /// Optional. For messages forwarded from channels, signature of the post author if present
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string ForwardSignature { get; set; }

        /// <summary>
        /// Optional. Sender's name for messages forwarded from users who disallow adding a link to their account in forwarded messages
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string ForwardSenderName { get; set; }

        /// <summary>
        /// Optional. For forwarded messages, date the original message was sent in Unix time
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public DateTime? ForwardDate { get; set; }

        /// <summary>
        /// Optional. For replies, the original message. Note that the Description object in this field will not contain further reply_to_message fields even if it itself is a reply.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Message ReplyToMessage { get; set; }

        /// <summary>
        /// Optional. Date the message was last edited in Unix time
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public DateTime? EditDate { get; set; }

        /// <summary>
        /// Optional. The unique identifier of a media message group this message belongs to
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string MediaGroupId { get; set; }

        /// <summary>
        /// Optional. Signature of the post author for messages in channels
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string AuthorSignature { get; set; }

        /// <summary>
        /// Optional. For text messages, the actual UTF-8 text of the message
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Text { get; set; }

        /// <summary>
        /// Optional. For text messages, special entities like usernames, URLs, bot commands, etc. that appear in the text
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MessageEntity[] Entities { get; set; }

        /// <summary>
        /// Gets the entity values.
        /// </summary>
        /// <value>
        /// The entity contents.
        /// </value>
        public IEnumerable<string> EntityValues =>
            Entities?.Select(entity => Text.Substring(entity.Offset, entity.Length));

        /// <summary>
        /// Optional. For messages with a caption, special entities like usernames, URLs, bot commands, etc. that appear in the caption
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public MessageEntity[] CaptionEntities { get; set; }

        /// <summary>
        /// Gets the caption entity values.
        /// </summary>
        /// <value>
        /// The caption entity contents.
        /// </value>
        public IEnumerable<string> CaptionEntityValues =>
            CaptionEntities?.Select(entity => Caption.Substring(entity.Offset, entity.Length));

        /// <summary>
        /// Optional. Description is an audio file, information about the file
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Audio Audio { get; set; }

        /// <summary>
        /// Optional. Description is a general file, information about the file
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Document Document { get; set; }

        /// <summary>
        /// Optional. Message is an animation, information about the animation. For backward compatibility, when this
        /// field is set, the document field will also be set
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Animation Animation { get; set; }

        /// <summary>
        /// Description is a game, information about the game.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Game Game { get; set; }

        /// <summary>
        /// Optional. Description is a photo, available sizes of the photo
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public PhotoSize[] Photo { get; set; }

        /// <summary>
        /// Optional. Description is a sticker, information about the sticker
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Sticker Sticker { get; set; }

        /// <summary>
        /// Optional. Description is a video, information about the video
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Video Video { get; set; }

        /// <summary>
        /// Description is a voice message, information about the file
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Voice Voice { get; set; }

        /// <summary>
        /// Optional. Description is a <see cref="VideoNote"/>, information about the video message
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public VideoNote VideoNote { get; set; }

        /// <summary>
        /// Caption for the photo or video
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Caption { get; set; }

        /// <summary>
        /// Optional. Description is a shared contact, information about the contact
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Contact Contact { get; set; }

        /// <summary>
        /// Optional. Description is a shared location, information about the location
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Location Location { get; set; }

        /// <summary>
        /// Optional. Description is a venue, information about the venue
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Venue Venue { get; set; }

        /// <summary>
        /// Optional. Message is a native poll, information about the poll
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Poll Poll { get; set; }

        /// <summary>
        /// Optional. New members that were added to the group or supergroup and information about them (the bot itself may be one of these members)
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public User[] NewChatMembers { get; set; }

        /// <summary>
        /// Optional. A member was removed from the group, information about them (this member may be bot itself)
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public User LeftChatMember { get; set; }

        /// <summary>
        /// Optional. A group title was changed to this value
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string NewChatTitle { get; set; }

        /// <summary>
        /// Optional. A group photo was change to this value
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public PhotoSize[] NewChatPhoto { get; set; }

        /// <summary>
        /// Optional. Informs that the group photo was deleted
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool DeleteChatPhoto { get; set; }

        /// <summary>
        /// Optional. Informs that the group has been created
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool GroupChatCreated { get; set; }

        /// <summary>
        /// Optional. Service message: the supergroup has been created
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool SupergroupChatCreated { get; set; }

        /// <summary>
        /// Optional. Service message: the channel has been created
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool ChannelChatCreated { get; set; }

        /// <summary>
        /// Optional. The group has been migrated to a supergroup with the specified identifier
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public long MigrateToChatId { get; set; }

        /// <summary>
        /// Optional. The supergroup has been migrated from a group with the specified identifier
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public long MigrateFromChatId { get; set; }

        /// <summary>
        /// Optional. Specified message was pinned. Note that the Description object in this field will not contain further reply_to_message fields even if it is itself a reply
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Message PinnedMessage { get; set; }

        /// <summary>
        /// Optional. Description is an invoice for a payment
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Invoice Invoice { get; set; }

        /// <summary>
        /// Optional. Description is a service message about a successful payment
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public SuccessfulPayment SuccessfulPayment { get; set; }

        /// <summary>
        /// Optional. The domain name of the website on which the user has logged in
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string ConnectedWebsite { get; set; }

        /// <summary>
        /// Optional. Telegram Passport data
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public PassportData PassportData { get; set; }

        /// <summary>
        /// Optional. Inline keyboard attached to the message
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Gets the <see cref="MessageType"/> of the <see cref="Message"/>
        /// </summary>
        /// <value>
        /// The <see cref="MessageType"/> of the <see cref="Message"/>
        /// </value>
        public MessageType Type
        {
            get
            {
                if (Audio != null)
                    return MessageType.Audio;

                if (Document != null)
                    return MessageType.Document;

                if (Game != null)
                    return MessageType.Game;

                if (Photo != null)
                    return MessageType.Photo;

                if (Sticker != null)
                    return MessageType.Sticker;

                if (Video != null)
                    return MessageType.Video;

                if (Voice != null)
                    return MessageType.Voice;

                if (Contact != null)
                    return MessageType.Contact;

                if (Venue != null)
                    return MessageType.Venue;

                if (Location != null)
                    return MessageType.Location;

                if (Text != null)
                    return MessageType.Text;

                if (Invoice != null)
                    return MessageType.Invoice;

                if (SuccessfulPayment != null)
                    return MessageType.SuccessfulPayment;

                if (VideoNote != null)
                    return MessageType.VideoNote;

                if (ConnectedWebsite != null)
                    return MessageType.WebsiteConnected;

                if (NewChatMembers?.Any() == true)
                    return MessageType.ChatMembersAdded;

                if (LeftChatMember != null)
                    return MessageType.ChatMemberLeft;

                if (NewChatTitle != null)
                    return MessageType.ChatTitleChanged;

                if (NewChatPhoto != null)
                    return MessageType.ChatPhotoChanged;

                if (PinnedMessage != null)
                    return MessageType.MessagePinned;

                if (DeleteChatPhoto)
                    return MessageType.ChatPhotoDeleted;

                if (GroupChatCreated)
                    return MessageType.GroupCreated;

                if (SupergroupChatCreated)
                    return MessageType.SupergroupCreated;

                if (ChannelChatCreated)
                    return MessageType.ChannelCreated;

                if (MigrateFromChatId != default)
                    return MessageType.MigratedFromGroup;

                if (MigrateToChatId != default)
                    return MessageType.MigratedToSupergroup;

                if (Poll != null)
                    return MessageType.Poll;

                return MessageType.Unknown;
            }
        }
    }
}
