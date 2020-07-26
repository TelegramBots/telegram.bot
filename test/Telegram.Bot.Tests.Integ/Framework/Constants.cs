using Telegram.Bot.Tests.Integ.Framework.XunitExtensions;

namespace Telegram.Bot.Tests.Integ.Framework
{
    public static class Constants
    {
        public const string AssemblyName = "Telegram.Bot.Tests.Integ";

        private const string CommonPrefix = AssemblyName + "." + nameof(Framework);

        public const string TestCaseOrderer =
            CommonPrefix + "." +
            nameof(Framework.TestCaseOrderer);

        public const string TestCaseDiscoverer =
            CommonPrefix + "." +
            nameof(XunitExtensions) + "." +
            nameof(RetryFactDiscoverer);

        public const string TestCollectionOrderer =
            CommonPrefix + "." +
            nameof(Framework.TestCollectionOrderer);

        public const string TestFramework =
            CommonPrefix + "." +
            nameof(XunitExtensions) + "." +
            nameof(XunitTestFrameworkWithAssemblyFixture);

        public const string CategoryTraitName = "Category";
        public const string InteractiveCategoryValue = "Interactive";
        public const string MethodTraitName = "Method";

        public static class TestCollections
        {
            public const string GettingUpdates = "Getting Updates";

            public const string Webhook = "Webhook";

            public const string SendTextMessage = "Sending Text Messages";

            public const string SendPhotoMessage = "Sending Photo Messages";

            public const string SendVideoMessage = "Sending Video Messages";

            public const string SendAnimationMessage = "Sending Animation Messages";

            public const string SendAudioMessage = "Sending Audio Messages";

            public const string SendVenueMessage = "Sending Venue Messages";

            public const string SendDocumentMessage = "Sending Document Messages";

            public const string SendContactMessage = "Sending Contact Messages";

            public const string NativePolls = "Native Polls";

            public const string ReplyMarkup = "Messages with Reply Markup";

            public const string PrivateChatReplyMarkup = "Messages with Reply Markup - Private Chat";

            public const string ChatInfo = "Getting Chat Info";

            public const string LeaveChat = "Leaving chats";

            public const string GetUserProfilePhotos = "Getting user profile photos";

            public const string InlineQuery = "Inline Query";

            public const string FileDownload = "File Download";

            public const string BotCommands = "Bot Commands";

            public const string Dice = "Dice";

            public const string CallbackQuery = "Callback Query";

            public const string AlbumMessage = "Sending Album Messages";

            public const string EditMessage = "Edit message content";

            public const string EditMessage2 = "Edit message content (non-interactive)";

            public const string EditMessageMedia = "Edit message media";

            public const string EditMessageMedia2 = "Edit message media (non-interactive)";

            public const string DeleteMessage = "Delete message";

            public const string DeleteMessage2 = "Delete message (non-interactive)";

            public const string LiveLocation = "Live Location";

            public const string InlineMessageLiveLocation = "Live Location for Inline Message";

            public const string Payment = "Payment";

            public const string Stickers = "Stickers";

            public const string Games = "Games";

            public const string GameException = "Game Exceptions";

            public const string Games2 = "Games (non-interactive)";

            public const string SupergroupAdminBots = "Supergroup Admin Bot";

            public const string ChannelAdminBots = "Channel Admin Bot";

            public const string ChatMemberAdministration = "Chat Member Administration";

            public const string Exceptions = "Bot API exceptions";

            public const string Exceptions2 = "Bot API exceptions (non-interactive)";
        }

        public static class PathToFile
        {
            private const string FilesDir = "Files/";

            public static class Documents
            {
                private const string DocumentDir = FilesDir + "Document/";

                public const string Hamlet = DocumentDir + "hamlet.pdf";
            }

            public static class Photos
            {
                private const string PhotoDir = FilesDir + "Photo/";

                public const string Bot = PhotoDir + "bot.gif";

                public const string Logo = PhotoDir + "logo.png";

                public const string Gnu = PhotoDir + "gnu.png";

                public const string Tux = PhotoDir + "tux.png";

                public const string Vlc = PhotoDir + "vlc.png";

                public const string Ruby = PhotoDir + "ruby.png";

                public const string Apes = PhotoDir + "apes.jpg";
            }

            public static class Videos
            {
                private const string VideoDir = FilesDir + "Video/";

                public const string GoldenRatio = VideoDir + "golden-ratio-240px.mp4";

                public const string MoonLanding = VideoDir + "moon-landing.mp4";
            }

            public static class Audio
            {
                private const string AudioDir = FilesDir + "Audio/";

                public const string AStateOfDespairMp3 = AudioDir + "Ask Again - A State of Despair.mp3";

                public const string CantinaRagMp3 = AudioDir + "Jackson F Smith - Cantina Rag.mp3";

                public const string TestOgg = AudioDir + "Test.ogg";
            }

            public static class Certificate
            {
                private const string CertificateDir = FilesDir + "Certificate/";

                public const string PublicKey = CertificateDir + "public-key.pem";
            }

            public static class Animation
            {
                private const string Dir = FilesDir + "Animation/";

                public const string Earth = Dir + "earth.gif";
            }

            public static class Thumbnail
            {
                private const string Dir = FilesDir + "Thumbnail/";

                public const string Video = Dir + "video.jpg";

                public const string TheAbilityToBreak = Dir + "The Ability to Break.jpg";
            }
        }

        public static class TelegramBotApiMethods
        {
            public const string GetMe = "getMe";

            public const string SendMessage = "sendMessage";

            public const string ForwardMessage = "forwardMessage";

            public const string AnswerCallbackQuery = "answerCallbackQuery";

            public const string AnswerInlineQuery = "answerInlineQuery";

            public const string SendInvoice = "sendInvoice";

            public const string AnswerShippingQuery = "answerShippingQuery";

            public const string AnswerPreCheckoutQuery = "answerPreCheckoutQuery";

            public const string SetChatTitle = "setChatTitle";

            public const string SetChatDescription = "setChatDescription";

            public const string SetChatPermissions = "setChatPermissions";

            public const string SetChatAdministratorCustomTitle = "setChatAdministratorCustomTitle";

            public const string ExportChatInviteLink = "exportChatInviteLink";

            public const string PinChatMessage = "pinChatMessage";

            public const string GetChat = "getChat";

            public const string LeaveChat = "leaveChat";

            public const string GetUserProfilePhotos = "getUserProfilePhotos";

            public const string GetChatMember = "getChatMember";

            public const string GetChatAdministrators = "getChatAdministrators";

            public const string GetChatMembersCount = "getChatMembersCount";

            public const string SendChatAction = "sendChatAction";

            public const string GetFile = "getFile";

            public const string UnpinChatMessage = "unpinChatMessage";

            public const string SetChatPhoto = "setChatPhoto";

            public const string DeleteChatPhoto = "deleteChatPhoto";

            public const string KickChatMember = "kickChatMember";

            public const string UnbanChatMember = "unbanChatMember";

            public const string RestrictChatMember = "restrictChatMember";

            public const string PromoteChatMember = "promoteChatMember";

            public const string GetStickerSet = "getStickerSet";

            public const string SendPhoto = "sendPhoto";

            public const string SendVideo = "sendVideo";

            public const string SendAnimation = "sendAnimation";

            public const string SendAudio = "sendAudio";

            public const string SendVenue = "sendVenue";

            public const string SendVoice = "sendVoice";

            public const string SendVideoNote = "sendVideoNote";

            public const string SendDocument = "sendDocument";

            public const string SendContact = "sendContact";

            public const string EditMessageText = "editMessageText";

            public const string EditMessageMedia = "editMessageMedia";

            public const string EditMessageReplyMarkup = "editMessageReplyMarkup";

            public const string EditMessageCaption = "editMessageCaption";

            public const string DeleteMessage = "deleteMessage";

            public const string SendLocation = "sendLocation";

            public const string EditMessageLiveLocation = "editMessageLiveLocation";

            public const string StopMessageLiveLocation = "stopMessageLiveLocation";

            public const string SetChatStickerSet = "setChatStickerSet";

            public const string SendSticker = "sendSticker";

            public const string SendMediaGroup = "sendMediaGroup";

            public const string UploadStickerFile = "uploadStickerFile";

            public const string CreateNewStickerSet = "createNewStickerSet";

            public const string AddStickerToSet = "addStickerToSet";

            public const string SetStickerPositionInSet = "setStickerPositionInSet";

            public const string DeleteStickerFromSet = "deleteStickerFromSet";

            public const string SendGame = "sendGame";

            public const string SetGameScore = "setGameScore";

            public const string GetGameHighScores = "getGameHighScores";

            public const string SetWebhook = "setWebhook";

            public const string DeleteWebhook = "deleteWebhook";

            public const string GetWebhookInfo = "getWebhookInfo";

            public const string SendPoll = "sendPoll";

            public const string StopPoll = "stopPoll";

            public const string SetMyCommands = "setMyCommands";

            public const string GetMyCommands = "getMyCommands";

            public const string SendDice = "sendDice";
        }
    }
}
