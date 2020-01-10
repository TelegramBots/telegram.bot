using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Telegram.Bot
{
    /// <summary>
    /// Converter used for (de)serializing the json data.
    /// </summary>
    public interface ITelegramBotJsonConverter
    {
        /// <summary>
        /// Deserializing json stream into Update class.
        /// </summary>
        /// <param name="jsonStream">UTF-8 stream which contains JSON data.</param>
        /// <param name="cancellationToken">Cancellation token used for cancellation of the deserialization task.</param>
        /// <returns>Update model.</returns>
        ValueTask<TOutput> DeserializeAsync<TOutput>(Stream jsonStream, CancellationToken cancellationToken);

        /// <summary>
        /// Serializing model of type <see cref="inputType"/> to output stream.
        /// </summary>
        /// <param name="outputStream">UTF-8 stream to serialize to.</param>
        /// <param name="inputModel">Value to serialize.</param>
        /// <param name="inputType">Type of <see cref="inputModel"/>.</param>
        /// <param name="cancellationToken">Cancellation token used for cancellation of the deserialization task.</param>
        ValueTask SerializeAsync(Stream outputStream, object inputModel, Type inputType, CancellationToken cancellationToken);

        /// <summary>
        /// Reading enumeration of <see cref="KeyValuePair{TKey,TValue}"/> type with <see cref="string"/> key
        /// which represents name and with <see cref="HttpContent"/> value which represents response body content.
        /// </summary>
        /// <param name="value">Value to serialize.</param>
        /// <param name="valueType">Type of <see cref="value"/></param>
        /// <param name="propertyNamesToExcept">Names of properties which must be ignored.</param>
        /// <param name="cancellationToken">Cancellation token used for cancellation of this conversion.</param>
        /// <returns>Enumeration of name-content pairs for <see cref="value"/> object.</returns>
        ValueTask<IEnumerable<KeyValuePair<string, HttpContent>>> ToNodesAsync(object value, Type valueType,
            string[] propertyNamesToExcept, CancellationToken cancellationToken);
    }
}
