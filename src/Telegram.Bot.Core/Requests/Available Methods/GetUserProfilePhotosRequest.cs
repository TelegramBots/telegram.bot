using Telegram.Bot.Types;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Requests
{
    /// <summary>
    /// Get a list of profile pictures for a user
    /// </summary>
    public class GetUserProfilePhotosRequest : RequestBase<UserProfilePhotos>
    {
        /// <summary>
        /// Unique identifier of the target user
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Sequential number of the first photo to be returned. By default, all photos are returned.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Limits the number of photos to be retrieved. Values between 1—100 are accepted. Defaults to 100.
        /// </summary>
        public int Limit { get; set; }

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
