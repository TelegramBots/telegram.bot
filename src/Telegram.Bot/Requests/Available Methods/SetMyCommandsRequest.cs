using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Use this method to change the list of the bot's commands. Returns <c>true</c> on success.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SetMyCommandsRequest : RequestBase<bool>
    {
        /// <summary>
        /// A list of bot commands to be set as the list of the bot's commands. At most 100
        /// <see cref="BotCommand"/>s can be specified.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public IEnumerable<BotCommand> Commands { get; }

        /// <summary>
        /// Initializes a new request with commands
        /// </summary>
        /// <param name="commands">A list of bot commands to be set</param>
        public SetMyCommandsRequest(IEnumerable<BotCommand> commands)
            : base("setMyCommands")
        {
            Commands = commands;
        }
    }
}
