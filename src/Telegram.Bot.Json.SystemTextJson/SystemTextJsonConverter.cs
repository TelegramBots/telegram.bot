using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Dahomey.Json;
using Dahomey.Json.NamingPolicies;

#if NETSTANDARD2_0
using Telegram.Bot.Helpers;
#endif

namespace Telegram.Bot.Json
{
    public sealed class SystemTextJsonConverter : ITelegramBotJsonConverter
    {
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                Converters =
                {
                    new JsonStringEnumConverter(new SnakeCaseNamingPolicy(), false)
                }
            }
            .SetupExtensions();

        public ValueTask<TOutput> DeserializeAsync<TOutput>(
            [DisallowNull] Stream jsonStream,
            CancellationToken cancellationToken)
        {
            return JsonSerializer.DeserializeAsync<TOutput>(jsonStream, _serializerOptions, cancellationToken);
        }

        public ValueTask SerializeAsync(
            [DisallowNull] Stream outputStream,
            object? inputModel,
            [DisallowNull] Type inputType,
            CancellationToken cancellationToken)
        {
            return new ValueTask(JsonSerializer.SerializeAsync(outputStream, inputModel, inputType, _serializerOptions,
                cancellationToken));
        }

        public ValueTask<IEnumerable<KeyValuePair<string, HttpContent>>> ToNodesAsync(
            [DisallowNull] object value,
            [DisallowNull] Type valueType,
            [DisallowNull] string[] propertyNamesToExcept,
            CancellationToken cancellationToken)
        {
            var jsonObject = JsonObject.FromObject(value, valueType, _serializerOptions);
            var result = new Dictionary<string, HttpContent>();

            foreach (var (name, node) in jsonObject)
            {
                if (propertyNamesToExcept.Contains(name))
                    continue;

                result.Add(name, new StringContent(node.ToString()));
            }

            return new ValueTask<IEnumerable<KeyValuePair<string, HttpContent>>>(
                new ReadOnlyDictionary<string, HttpContent>(result));
        }
    }
}
