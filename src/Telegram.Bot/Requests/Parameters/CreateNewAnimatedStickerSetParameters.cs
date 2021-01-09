using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace Telegram.Bot.Requests.Parameters
{
    /// <summary>
    ///     Parameters for <see cref="ITelegramBotClient.CreateNewAnimatedStickerSetAsync" /> method.
    /// </summary>
    public class CreateNewAnimatedStickerSetParameters : ParametersBase
    {
        /// <summary>
        ///     User identifier of created sticker set owner
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     Short name of sticker set, to be used in t.me/addstickers/ URLs (e.g., animals). Can contain only English letters,
        ///     digits and underscores. Must begin with a letter, can't contain consecutive underscores and must end in “_by_&lt;
        ///     bot_username&gt;”. &lt;bot_username&gt; is case insensitive. 1-64 characters.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Sticker set title, 1-64 characters
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Tgs animation with the sticker
        /// </summary>
        public InputFileStream TgsSticker { get; set; }

        /// <summary>
        ///     One or more emoji corresponding to the sticker
        /// </summary>
        public string Emojis { get; set; }

        /// <summary>
        ///     Pass True, if a set of mask stickers should be created
        /// </summary>
        public bool IsMasks { get; set; }

        /// <summary>
        ///     Position where the mask should be placed on faces
        /// </summary>
        public MaskPosition MaskPosition { get; set; }

        /// <summary>
        ///     Sets <see cref="UserId" /> property.
        /// </summary>
        /// <param name="userId">User identifier of created sticker set owner</param>
        public CreateNewAnimatedStickerSetParameters WithUserId(int userId)
        {
            UserId = userId;
            return this;
        }

        /// <summary>
        ///     Sets <see cref="Name" /> property.
        /// </summary>
        /// <param name="name">
        ///     Short name of sticker set, to be used in t.me/addstickers/ URLs (e.g., animals). Can contain only
        ///     English letters, digits and underscores. Must begin with a letter, can't contain consecutive underscores and must
        ///     end in “_by_&lt;bot_username&gt;”. &lt;bot_username&gt; is case insensitive. 1-64 characters.
        /// </param>
        public CreateNewAnimatedStickerSetParameters WithName(string name)
        {
            Name = name;
            return this;
        }

        /// <summary>
        ///     Sets <see cref="Title" /> property.
        /// </summary>
        /// <param name="title">Sticker set title, 1-64 characters</param>
        public CreateNewAnimatedStickerSetParameters WithTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        ///     Sets <see cref="TgsSticker" /> property.
        /// </summary>
        /// <param name="tgsSticker">Tgs animation with the sticker</param>
        public CreateNewAnimatedStickerSetParameters WithTgsSticker(InputFileStream tgsSticker)
        {
            TgsSticker = tgsSticker;
            return this;
        }

        /// <summary>
        ///     Sets <see cref="Emojis" /> property.
        /// </summary>
        /// <param name="emojis">One or more emoji corresponding to the sticker</param>
        public CreateNewAnimatedStickerSetParameters WithEmojis(string emojis)
        {
            Emojis = emojis;
            return this;
        }

        /// <summary>
        ///     Sets <see cref="IsMasks" /> property.
        /// </summary>
        /// <param name="isMasks">Pass True, if a set of mask stickers should be created</param>
        public CreateNewAnimatedStickerSetParameters WithIsMasks(bool isMasks)
        {
            IsMasks = isMasks;
            return this;
        }

        /// <summary>
        ///     Sets <see cref="MaskPosition" /> property.
        /// </summary>
        /// <param name="maskPosition">Position where the mask should be placed on faces</param>
        public CreateNewAnimatedStickerSetParameters WithMaskPosition(MaskPosition maskPosition)
        {
            MaskPosition = maskPosition;
            return this;
        }
    }
}