using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Get a list of profile pictures for a user
    /// </summary>
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class GetUserProfilePhotosRequest : RequestBase<UserProfilePhotos>, IUserTargetable
    {
        /// <summary>
        /// Unique identifier of the target user
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int UserId { get; }

        /// <summary>
        /// Sequential number of the first photo to be returned. By default, all photos are
        /// returned.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Offset { get; set; }

        /// <summary>
        /// Limits the number of photos to be retrieved. Values between 1—100 are accepted.
        /// Defaults to 100.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Limit { get; set; }

        /// <summary>
        /// Initializes a new request with user id
        /// </summary>
        /// <param name="userId">Unique identifier of the target user</param>
        public GetUserProfilePhotosRequest(int userId)
            : base("getUserProfilePhotos")
        {
            UserId = userId;
        }
    }
}
