using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Telegram.Bot.Types.Enums
{
    /// <summary>
    /// The part of the face relative to which the mask should be placed.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum MaskPositionPoint
    {
        /// <summary>
        /// The forehead
        /// </summary>
        Forehead,
        /// <summary>
        /// The eyes
        /// </summary>
        Eyes,
        /// <summary>
        /// The mouth
        /// </summary>
        Mouth,
        /// <summary>
        /// The chin
        /// </summary>
        Chin
    }
}
