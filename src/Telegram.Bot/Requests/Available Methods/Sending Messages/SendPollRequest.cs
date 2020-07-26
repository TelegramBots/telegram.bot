using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
// ReSharper disable CheckNamespace

namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Send a native poll. A native poll can't be sent to a private chat.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SendPollRequest : RequestBase<Message>, IChatTargetable
    {
        /// <summary>
        /// Unique identifier for the target chat or username of the target channel
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Poll question, 1-255 characters
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Question { get; }

        /// <summary>
        /// List of answer options, 2-10 strings 1-100 characters each
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public IEnumerable<string> Options { get; }

        /// <summary>
        /// Optional. True, if the poll needs to be anonymous, defaults to True
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? IsAnonymous { get; set; }

        /// <summary>
        /// Optional. Poll type, <see cref="PollType.Quiz"/> or <see cref="PollType.Regular"/>,
        /// defaults to <see cref="PollType.Regular"/>
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PollType? Type { get; set; }

        /// <summary>
        /// Optional. <c>true</c>, if the poll allows multiple answers, ignored for polls in
        /// quiz mode, defaults to <c>false</c>
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? AllowsMultipleAnswers { get; set; }

        /// <summary>
        /// Optional. 0-based identifier of the correct answer option, required for polls in
        /// quiz mode
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? CorrectOptionId { get; set; }

        /// <summary>
        /// Optional. Text that is shown when a user chooses an incorrect answer or taps on the
        /// lamp icon in a quiz-style poll, 0-200 characters with at most 2 line feeds after
        /// entities parsing
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Explanation { get; set; }

        /// <summary>
        /// Optional. Mode for parsing entities in the explanation
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ParseMode? ExplanationParseMode { get; set; }

        /// <summary>
        /// Optional. Amount of time in seconds the poll will be active after creation, 5-600.
        /// Can't be used together with <see cref="CloseDate"/>.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? OpenPeriod { get; set; }

        /// <summary>
        /// Optional. Point in time (Unix timestamp) when the poll will be automatically closed.
        /// Must be at least 5 and no more than 600 seconds in the future. Can't be used
        /// together with <see cref="OpenPeriod"/>.
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// Optional. Pass True, if the poll needs to be immediately closed
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? IsClosed { get; set; }

        /// <summary>
        /// Sends the message silently. Users will receive a notification with no sound.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? DisableNotification { get; set; }

        /// <summary>
        /// If the message is a reply, ID of the original message.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ReplyToMessageId { get; set; }

        /// <summary>
        /// A JSON-serialized object for a custom reply keyboard,
        /// instructions to hide keyboard or to force a reply from the user.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReplyMarkup? ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId, question and <see cref="PollOption"/>
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target channel
        /// </param>
        /// <param name="question">Poll question, 1-255 characters</param>
        /// <param name="options">
        /// List of answer options, 2-10 strings 1-100 characters each
        /// </param>
        public SendPollRequest(ChatId chatId, string question, IEnumerable<string> options)
            : base("sendPoll")
        {
            ChatId = chatId;
            Question = question;
            Options = options;
        }
    }
}
