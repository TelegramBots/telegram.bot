using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.InlineQueryResults;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send answers to an inline query. On success, <c>true</c> is returned. No more than 50
    /// results per query are allowed.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class AnswerInlineQueryRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the answered query
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string InlineQueryId { get; }

        /// <summary>
        /// A JSON-serialized array of results for the inline query
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public IEnumerable<InlineQueryResultBase> Results { get; }

        /// <summary>
        /// The maximum amount of time in seconds that the result of the inline query may be
        /// cached on the server. Defaults to 300.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? CacheTime { get; set; }

        /// <summary>
        /// Optional. Pass <c>true</c>, if results may be cached on the server side only for the
        /// user that sent the query. By default, results may be returned to any user who sends
        /// the same query
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? IsPersonal { get; set; }

        /// <summary>
        /// Optional. Pass the offset that a client should send in the next query with the same
        /// text to receive more results. Pass an empty string if there are no more results or if
        /// you don‘t support pagination. Offset length can’t exceed 64 bytes.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? NextOffset { get; set; }

        /// <summary>
        /// Optional. If passed, clients will display a button with specified text that switches
        /// the user to a private chat with the bot and sends the bot a start message with the
        /// parameter <see cref="SwitchPmParameter"/>
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? SwitchPmText { get; set; }

        /// <summary>
        /// Optional. Deep-linking parameter for the <c>/start</c> message sent to the bot when
        /// user presses the switch button. 1-64 characters, only A-Z, a-z, 0-9, _ and -
        /// are allowed.
        /// </summary>
        /// <example>
        /// An inline bot that sends YouTube videos can ask the user to connect the bot to their
        /// YouTube account to adapt search results accordingly.To do this, it displays a
        /// ‘Connect your YouTube account’ button above the results, or even before showing any.
        /// The user presses the button, switches to a private chat with the bot and, in doing so,
        /// passes a start parameter that instructs the bot to return an oauth link. Once done,
        /// the bot can offer a switch_inline button so that the user can easily return to the
        /// chat where they wanted to use the bot's inline capabilities.
        /// </example>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? SwitchPmParameter { get; set; }

        /// <summary>
        /// Initializes a new request with inlineQueryId and an array of
        /// <see cref="InlineQueryResultBase"/>
        /// </summary>
        /// <param name="inlineQueryId">Unique identifier for the answered query</param>
        /// <param name="results">A JSON-serialized array of results for the inline query</param>
        public AnswerInlineQueryRequest(
            string inlineQueryId,
            IEnumerable<InlineQueryResultBase> results)
            : base("answerInlineQuery")
        {
            InlineQueryId = inlineQueryId;
            Results = results;
        }
    }
}
