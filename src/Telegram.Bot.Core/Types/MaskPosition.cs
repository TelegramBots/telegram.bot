using System.Runtime.Serialization;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types
{
    /// <summary>
    /// This object describes the position on faces where a mask should be placed by default.
    /// </summary>
    [DataContract]
    public class MaskPosition
    {
        /// <summary>
        /// The part of the face relative to which the mask should be placed. One of “forehead”, “eyes”, “mouth”, or “chin”.
        /// </summary>
        [DataMember(IsRequired = true)]
        public MaskPositionPoint Point { get; set; }

        /// <summary>
        /// Shift by X-axis measured in widths of the mask scaled to the face size, from left to right. For example, choosing -1.0 will place mask just to the left of the default mask position.
        /// </summary>
        [DataMember(IsRequired = true)]
        public float XShift { get; set; }

        /// <summary>
        /// Shift by Y-axis measured in heights of the mask scaled to the face size, from top to bottom. For example, 1.0 will place the mask just below the default mask position.
        /// </summary>
        [DataMember(IsRequired = true)]
        public float YShift { get; set; }

        /// <summary>
        /// Mask scaling coefficient. For example, 2.0 means double size.
        /// </summary>
        [DataMember(IsRequired = true)]
        public float Scale { get; set; }
    }
}
