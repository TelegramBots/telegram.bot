using System;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Unit.Serialization
{
    public class DocumentSerializationTests
    {
        [Fact(DisplayName = "Should serialize a document message")]
        public void Should_Serialize_DocumentMessage()
        {
            var documentMessage = new Message
            {
                MessageId = 1234,
                From = new User
                {
                    Id = 123_456_789,
                    FirstName = "TelegramBots",
                    Username = "Telegram_Bots"
                },
                Chat = new Chat
                {
                    Id = -9_877_654_320_000,
                    Title = "Test Chat",
                    Type = ChatType.Supergroup,
                    CanSetStickerSet = true
                },
                Document = new Document
                {
                    FileId = "KLAHCVUydfS_jHIBildtwpmvxZg",
                    FileUniqueId = "AgADcOsAAhUdZAc",
                    FileName = "test_file.txt",
                    FileSize = 123_456,
                    MimeType = "plain/text"
                },
                Date = DateTime.UtcNow,
                Caption = "Test Document Description"
            };

            string json = JsonConvert.SerializeObject(documentMessage);

            Assert.NotNull(json);
            Assert.True(json.Length > 100);
            Assert.Contains(@"""file_id"":""KLAHCVUydfS_jHIBildtwpmvxZg""", json);
            Assert.Contains(@"""can_set_sticker_set"":true", json);
        }

        [Fact(DisplayName = "Should deserialize a document message")]
        public void Should_Deserialize_DocumentMessage()
        {
            const string json = @"
            {
                ""message_id"": 1234,
                ""from"": {
                    ""id"": 123456789,
                    ""is_bot"": false,
                    ""first_name"": ""TelegramBots"",
                    ""last_name"": null,
                    ""username"": ""Telegram_Bots"",
                    ""language_code"": null
                },
                ""date"": 1503172015,
                ""chat"": {
                    ""id"": -9877654320000,
                    ""type"": ""supergroup"",
                    ""title"": ""Test Chat"",
                    ""username"": null,
                    ""first_name"": null,
                    ""last_name"": null,
                    ""all_members_are_administrators"": false,
                    ""photo"": null,
                    ""description"": null,
                    ""invite_link"": null,
                    ""sticker_set_name"": null,
                    ""can_set_sticker_set"": null
                },
                ""forward_from"": null,
                ""forward_from_chat"": null,
                ""forward_from_message_id"": 0,
                ""forward_date"": null,
                ""reply_to_message"": null,
                ""edit_date"": null,
                ""text"": null,
                ""entities"": [],
                ""caption_entities"": [],
                ""audio"": null,
                ""document"": {
                    ""file_name"": ""test_file.txt"",
                    ""mime_type"": ""plain/text"",
                    ""file_id"": ""KLAHCVUydfS_jHIBildtwpmvxZg"",
                    ""file_unique_id"": ""AgADcOsAAhUdZAc"",
                    ""file_size"": 123456,
                    ""file_path"": null
                },
                ""game"": null,
                ""photo"": null,
                ""sticker"": null,
                ""video"": null,
                ""voice"": null,
                ""video_note"": null,
                ""caption"": ""Test Document Description"",
                ""contact"": null,
                ""location"": null,
                ""venue"": null,
                ""new_chat_member"": null,
                ""new_chat_members"": null,
                ""left_chat_member"": null,
                ""new_chat_title"": null,
                ""new_chat_photo"": null,
                ""delete_chat_photo"": false,
                ""group_chat_created"": false,
                ""supergroup_chat_created"": false,
                ""channel_chat_created"": false,
                ""migrate_to_chat_id"": 0,
                ""migrate_from_chat_id"": 0,
                ""pinned_message"": null,
                ""invoice"": null,
                ""successful_payment"": null
            }
            ";

            var message = JsonConvert.DeserializeObject<Message>(json);

            Assert.Equal(MessageType.Document, message.Type);
            Assert.NotNull(message.Document);
            Assert.Equal("test_file.txt", message.Document.FileName);
            Assert.NotNull(message.Chat);
            Assert.Null(message.Chat.CanSetStickerSet);
        }
    }
}
