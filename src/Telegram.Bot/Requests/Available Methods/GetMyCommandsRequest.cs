using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Use this method to get the current list of the bot's commands. Requires no parameters.
    /// Returns array of <see cref="BotCommand"/> on success.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class GetMyCommandsRequest : RequestBase<BotCommand[]>
    {
        /// <summary>
        /// Initializes a new request
        /// </summary>
        public GetMyCommandsRequest()
            : base("getMyCommands")
        { }
    }
}
