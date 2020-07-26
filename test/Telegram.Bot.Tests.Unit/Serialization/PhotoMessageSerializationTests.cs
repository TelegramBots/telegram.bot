using System;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;
using Newtonsoft.Json;

namespace Telegram.Bot.Tests.Unit.Serialization
{
    public class PhotoMessageSerializationTests
    {
        [Fact(DisplayName = "Should deserialize a photo message")]
        public void Should_Deserialize_PhotoMessage()
        {
            const string json = @"{
                ""message_id"": 1234,
                ""from"": {
                    ""id"": 1234567,
                    ""is_bot"": false,
                    ""first_name"": ""Telegram_Bots"",
                    ""last_name"": null,
                    ""username"": ""TelegramBots"",
                    ""language_code"": null
                },
                ""chat"": {
                    ""id"": 1234567,
                    ""first_name"": ""Telegram_Bots"",
                    ""last_name"": null,
                    ""username"": ""TelegramBots"",
                    ""type"": ""private""
                },
                ""date"": 1526315997,
                ""photo"": [
                    {
                        ""file_id"": ""AgADAgADvKgxGxW80EtRgjrTaWNmy7UerQ4ABN7x5HqnrHW_wp4BAAEC"",
                        ""file_unique_id"": ""AgADcOsAAhUdZAc"",
                        ""file_size"": 3134,
                        ""width"": 90,
                        ""height"": 90
                    },
                    {
                        ""file_id"": ""AgADAgADvKgxGxW80EtRgjrTaWNmy7UerQ4ABIrxzSBLXOQYw54BAAEC"",
                        ""file_unique_id"": ""AgADcOsAAhUdZAc"",
                        ""file_size"": 52433,
                        ""width"": 320,
                        ""height"": 320
                    },
                    {
                        ""file_id"": ""AgADAgADvKgxGxW80EtRgjrTaWNmy7UerQ4ABIJONRZpTJFnxJ4BAAEC"",
                        ""file_unique_id"": ""AgADcOsAAhUdZAc"",
                        ""file_size"": 231019,
                        ""width"": 800,
                        ""height"": 800
                    },
                    {
                        ""file_id"": ""AgADAgADvKgxGxW80EtRgjrTaWNmy7UerQ4ABP6uRLtwe8Z8wZ4BAAEC"",
                        ""file_unique_id"": ""AgADcOsAAhUdZAc"",
                        ""file_size"": 489108,
                        ""width"": 1280,
                        ""height"": 1280
                    }
                ]
            }";

            var message = JsonConvert.DeserializeObject<Message>(json);

            Assert.Equal(MessageType.Photo, message.Type);
            Assert.NotNull(message.Photo);
            Assert.NotEmpty(message.Photo);
            Assert.All(message.Photo.Select(ps => ps.FileId), Assert.NotEmpty);
            Assert.All(message.Photo.Select(ps => ps.Width), w => Assert.NotEqual(default, w));
            Assert.All(message.Photo.Select(ps => ps.Height), h => Assert.NotEqual(default, h));
        }

        [Fact(DisplayName = "Should serialize a photo message")]
        public void Should_Serialize_PhotoMessage()
        {
            Message message = new Message
            {
                MessageId = 1234,
                From = new User
                {
                    Id = 1234567,
                    FirstName = "Telegram_Bots",
                    Username = "TelegramBots",
                },
                Chat = new Chat
                {
                    Id = 1234567,
                    FirstName = "Telegram_Bots",
                    Username = "TelegramBots",
                    Type = ChatType.Private
                },
                Date = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Photo = new[]
                {
                    new PhotoSize
                    {
                        FileId = "AgADAgADvKgxGxW80EtRgjrTaWNmy7UerQ4ABN7x5HqnrHW_wp4BAAEC",
                        FileUniqueId = "AgADcOsAAhUdZAc",
                        FileSize = 3134,
                        Width = 90,
                        Height = 90,
                    },
                    new PhotoSize
                    {
                        FileId = "AgADAgADvKgxGxW80EtRgjrTaWNmy7UerQ4ABIrxzSBLXOQYw54BAAEC",
                        FileUniqueId = "AgADcOsAAhUdZAc",
                        FileSize = 52433,
                        Width = 320,
                        Height = 320,
                    },
                    new PhotoSize
                    {
                        FileId = "AgADAgADvKgxGxW80EtRgjrTaWNmy7UerQ4ABIJONRZpTJFnxJ4BAAEC",
                        FileUniqueId = "AgADcOsAAhUdZAc",
                        FileSize = 231019,
                        Width = 800,
                        Height = 800,
                    },
                    new PhotoSize
                    {
                        FileId = "AgADAgADvKgxGxW80EtRgjrTaWNmy7UerQ4ABP6uRLtwe8Z8wZ4BAAEC",
                        FileUniqueId = "AgADcOsAAhUdZAc",
                        FileSize = 489108,
                        Width = 1280,
                        Height = 1280,
                    }
                }
            };

            var json = JsonConvert.SerializeObject(message);

            Assert.NotNull(json);
            Assert.True(json.Length > 100);
            Assert.Contains(@"""file_id"":""AgADAgADvKgxGxW80EtRgjrTaWNmy7UerQ4ABP6uRLtwe8Z8wZ4BAAEC""", json);
        }
    }
}
