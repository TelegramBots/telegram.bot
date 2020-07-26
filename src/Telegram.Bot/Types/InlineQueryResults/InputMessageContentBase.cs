using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Telegram.Bot.Types.InlineQueryResults
{
    /// <summary>
    /// This object represents the content of a message to be sent as a result of an
    /// <see cref="InlineQuery"/>. Telegram clients currently support the following 4 types:
    /// <see cref="InputTextMessageContent"/>, <see cref="InputLocationMessageContent"/>,
    /// <see cref="InputVenueMessageContent"/>, <see cref="InputContactMessageContent"/>
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract class InputMessageContentBase { }
}
