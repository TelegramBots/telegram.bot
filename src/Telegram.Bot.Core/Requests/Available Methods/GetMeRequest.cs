using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// A simple method for testing your bot's auth token.
    /// </summary>
    public sealed class GetMeRequest : ParameterlessRequest<User>
    {
        /// <summary>
        /// Initializes a new request
        /// </summary>
        public GetMeRequest()
            : base("getMe")
        { }
    }
}
