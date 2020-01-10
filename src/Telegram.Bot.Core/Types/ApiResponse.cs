using System.Runtime.Serialization;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// Represents bot API response
    /// </summary>
    /// <typeparam name="TResult">Expected type of operation result</typeparam>
    [DataContract]
    public class ApiResponse<TResult>
    {
        /// <summary>
        /// Gets a value indicating whether the request was successful.
        /// </summary>
        [DataMember(IsRequired = true)]
        public bool Ok { get; set; }

        /// <summary>
        /// Gets the result object.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public TResult Result { get; set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int ErrorCode { get; set; }

        /// <summary>
        /// Contains information about why a request was unsuccessful.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public ResponseParameters Parameters { get; set; }
    }
}
