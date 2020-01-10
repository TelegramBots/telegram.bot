using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Telegram.Bot.Json.Converters
{
    internal class InputFileConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) =>
            objectType.GetTypeInfo().IsSubclassOf(typeof(InputFileStream));

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var input = (IInputFile)value;
            switch (input.FileType)
            {
                case FileType.Stream:
                    writer.WriteValue(null as object);
                    break;
                case FileType.Id when value is InputTelegramFile file:
                    writer.WriteValue(file.FileId);
                    break;
                case FileType.Url when value is InputOnlineFile file:
                    writer.WriteValue(file.Url);
                    break;
                default:
                    throw new NotSupportedException("File Type is not supported");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = JToken.ReadFrom(reader).Value<string>();
            if (value == null)
            {
                return new InputFileStream(Stream.Null);
            }

            return Uri.TryCreate(value, UriKind.Absolute, out _)
                ? new InputOnlineFile(value)
                : new InputTelegramFile(value);
        }
    }
}
