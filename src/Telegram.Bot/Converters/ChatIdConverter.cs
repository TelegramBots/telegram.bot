using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Telegram.Bot.Types;

namespace Telegram.Bot.Converters
{
    internal class ChatIdConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var chatId = (ChatId)value;

            if (chatId.Username != null)
            {
                writer.WriteValue(chatId.Username);
            }
            else
            {
                writer.WriteValue(chatId.Identifier);
            }
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            var value = JToken.ReadFrom(reader).Value<string>();

            return new ChatId(value);
        }

        public override bool CanConvert(Type objectType)
            => typeof(ChatId) == objectType;
    }
}
