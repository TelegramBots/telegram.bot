#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Json.Converters;

namespace Telegram.Bot.Json
{
    public sealed class NewtonsoftTelegramBotJsonConverter : ITelegramBotJsonConverter
    {
        private readonly JsonSerializer _jsonSerializer = new JsonSerializer
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            Converters =
            {
                new ChatIdConverter(),
                new InputFileConverter(),
                new InputMediaConverter(),
                /*new MessageEntityTypeConverter(),*/
                new StringEnumConverter(new SnakeCaseNamingStrategy())
            }
        };

        public NewtonsoftTelegramBotJsonConverter(ITraceWriter? traceWriter = default)
        {
            if (traceWriter == default)
                return;

            _jsonSerializer.TraceWriter = traceWriter;
        }

        public ValueTask SerializeAsync(
            [DisallowNull] Stream outputStream,
            object? inputModel,
            [DisallowNull] Type inputType,
            CancellationToken cancellationToken)
        {
            using (var sw = new StreamWriter(outputStream))
            using (var jsonTextWriter = new JsonTextWriter(sw))
            {
                _jsonSerializer.Serialize(jsonTextWriter, inputModel, inputType);
            }

            return default;
        }

        public ValueTask<IEnumerable<KeyValuePair<string, HttpContent>>> ToNodesAsync(
            [DisallowNull] object value,
            [DisallowNull] Type valueType,
            [DisallowNull] string[] propertyNamesToExcept,
            CancellationToken cancellationToken)
        {
            var stringContents = JObject.FromObject(value)
                .Properties()
                .Where(prop => propertyNamesToExcept?.Contains(prop.Name) == false)
                .Select(prop => new KeyValuePair<string, HttpContent>(prop.Name, new StringContent(prop.Value.ToString())));

            return new ValueTask<IEnumerable<KeyValuePair<string, HttpContent>>>(stringContents);
        }

        public ValueTask<TOutput> DeserializeAsync<TOutput>(
            [DisallowNull] Stream jsonStream,
            CancellationToken cancellationToken)
        {
            using (var sr = new StreamReader(jsonStream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return new ValueTask<TOutput>(_jsonSerializer.Deserialize<TOutput>(jsonTextReader));
            }
        }
    }
}
