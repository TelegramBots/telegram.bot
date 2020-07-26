//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Telegram.Bot.Exceptions;
//using Telegram.Bot.Tests.Integ.Framework;
//using Telegram.Bot.Types;
//using Telegram.Bot.Types.Enums;
//using Telegram.Bot.Types.ReplyMarkups;
//using Xunit;
//
//namespace Telegram.Bot.Tests.Integ.Stickers
//{
//    [Collection(Constants.TestCollections.Stickers)]
//    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
//    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
//    public class StickersTests : IClassFixture<StickersTestsFixture>
//    {
//        private ITelegramBotClient BotClient => _fixture.BotClient;
//
//        private readonly StickersTestsFixture _classFixture;
//
//        private readonly TestsFixture _fixture;
//
//        public StickersTests(TestsFixture fixture, StickersTestsFixture classFixture)
//        {
//            _classFixture = classFixture;
//            _fixture = fixture;
//        }
//
//        #region 1. Get and send stickers from a set
//
//        [OrderedFact("Should get EvilMinds sticker set")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetStickerSet)]
//        public async Task Should_Get_Sticker_Set()
//        {
//            string setName = "EvilMinds";
//            string setTitle = "Evil Minds";
//            StickerSet stickerSet = await BotClient.GetStickerSetAsync(
//                name: setName
//            );
//
//            Assert.Equal(setName, stickerSet.Name);
//            Assert.Equal(setTitle, stickerSet.Title);
//            Assert.False(stickerSet.ContainsMasks);
//            Assert.NotEmpty(stickerSet.Stickers);
//            Assert.True(20 < stickerSet.Stickers.Length);
//
//            _classFixture.EvilMindsStickerSet = stickerSet;
//        }
//
//        [OrderedFact("Should get Masks2 sticker set")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetStickerSet)]
//        public async Task Should_Get_Sticker_Set_With_Masks()
//        {
//            string setName = "masks2";
//            string setTitle = "Masks II: Face Lift";
//            StickerSet stickerSet = await BotClient.GetStickerSetAsync(
//                name: setName
//            );
//
//            Assert.Equal(setName, stickerSet.Name);
//            Assert.Equal(setTitle, stickerSet.Title);
//            Assert.True(stickerSet.ContainsMasks);
//            Assert.True(20 < stickerSet.Stickers.Length);
//        }
//
//        [OrderedFact("Should send sticker of Vlad the Impaler 🍷")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendSticker)]
//        public async Task Should_Send_Sticker()
//        {
//            Sticker vladDraculaSticker = _classFixture.EvilMindsStickerSet.Stickers[0];
//
//            Message message = await BotClient.SendStickerAsync(
//                chatId: _fixture.SupergroupChat.Id,
//                sticker: vladDraculaSticker.FileId
//            );
//
//            Assert.Equal(MessageType.Sticker, message.Type);
//            Assert.Equal(vladDraculaSticker.FileId, message.Sticker.FileId);
//            Assert.Equal(vladDraculaSticker.Emoji, message.Sticker.Emoji);
//            Assert.Equal(vladDraculaSticker.Height, message.Sticker.Height);
//            Assert.Equal(vladDraculaSticker.MaskPosition, message.Sticker.MaskPosition);
//            Assert.Equal(vladDraculaSticker.FileSize, message.Sticker.FileSize);
//            Assert.Equal(vladDraculaSticker.Thumb.FileId, message.Sticker.Thumb.FileId);
//            Assert.Equal(vladDraculaSticker.Thumb.Height, message.Sticker.Thumb.Height);
//            Assert.Equal(vladDraculaSticker.Thumb.Width, message.Sticker.Thumb.Width);
//        }
//
//        #endregion
//
//        #region 2. Create sticker set
//
//        [OrderedFact("Should upload sticker files to get file_id")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.UploadStickerFile)]
//        public async Task Should_Upload_Stickers()
//        {
//            List<File> stickerFiles = new List<File>(2);
//            foreach (string pngFile in new [] { Constants.FileNames.Photos.Gnu, Constants.FileNames.Photos.Tux })
//            {
//                File file;
//                using (System.IO.Stream stream = System.IO.File.OpenRead(pngFile))
//                {
//                    file = await BotClient.UploadStickerFileAsync(
//                        userId: _classFixture.OwnerUserId,
//                        pngSticker: stream
//                    );
//                }
//
//                Assert.NotEmpty(file.FileId);
//                Assert.True(file.FileSize > 0);
//
//                stickerFiles.Add(file);
//            }
//
//            _classFixture.UploadedStickers = stickerFiles;
//        }
//
//        [OrderedFact("Should throw " + nameof(InvalidStickerSetNameException) +
//                     " while trying to create sticker set with name not ending in _by_<bot username>")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.CreateNewStickerSet)]
//        public async Task Should_Throw_InvalidStickerSetNameException()
//        {
//            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(async () =>
//                await BotClient.CreateNewStickerSetAsync(
//                    userId: _classFixture.OwnerUserId,
//                    name: "Invalid_Sticker_Set_Name",
//                    title: "Sticker Set Title",
//                    pngSticker: _classFixture.UploadedStickers.First().FileId,
//                    emojis: "😀"
//                )
//            );
//
//            Assert.IsType<InvalidStickerSetNameException>(exception);
//        }
//
//        [OrderedFact("Should throw " + nameof(InvalidStickerEmojisException) +
//                     " while trying to create sticker with invalid emoji")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.CreateNewStickerSet)]
//        public async Task Should_Throw_InvalidStickerEmojisException()
//        {
//            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(async () =>
//                await BotClient.CreateNewStickerSetAsync(
//                    userId: _classFixture.OwnerUserId,
//                    name: "valid_name3" + _classFixture.TestStickerSetName,
//                    title: "Sticker Set Title",
//                    pngSticker: _classFixture.UploadedStickers.First().FileId,
//                    emojis: "INVALID"
//                )
//            );
//
//            Assert.IsType<InvalidStickerEmojisException>(exception);
//        }
//
//        [OrderedFact("Should throw " + nameof(InvalidStickerDimensionsException) +
//                     " while trying to create sticker with invalid dimensions")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.CreateNewStickerSet)]
//        public async Task Should_Throw_InvalidStickerDimensionsException()
//        {
//            BadRequestException exception;
//            using (System.IO.Stream stream = System.IO.File.OpenRead(Constants.FileNames.Photos.Logo))
//            {
//                exception = await Assert.ThrowsAsync<BadRequestException>(async () =>
//                    await BotClient.CreateNewStickerSetAsync(
//                        userId: _classFixture.OwnerUserId,
//                        name: "valid_name2" + _classFixture.TestStickerSetName,
//                        title: "Sticker Set Title",
//                        pngSticker: stream,
//                        emojis: "😁"
//                    )
//                );
//            }
//
//            Assert.IsType<InvalidStickerDimensionsException>(exception);
//        }
//
//        [OrderedFact("Should remove sticker set if it exists")]
//        public async Task Should_Remove_Stickers_In_Set_If_Exists()
//        {
//            string stickerSetName = _classFixture.TestStickerSetName;
//            int setOwnerId = _classFixture.OwnerUserId;
//
//            await BotClient.SendTextMessageAsync(
//                _fixture.SupergroupChat,
//                $"Sticker set <a href=\"https://t.me/addstickers/{stickerSetName}\">{stickerSetName}</a> " +
//                "<i>might</i> already exist.\n\n" +
//                $"If exists, <a href=\"tg://user?id={setOwnerId})\">Set Owner</a> should remove it " +
//                "in the chat with @Stickers bot.\n\n" +
//                "Forward these messages to it @Stickers and then click on Continue button.",
//                ParseMode.Html,
//                replyMarkup: new InlineKeyboardMarkup(
//                    InlineKeyboardButton.WithCallbackData("Continue", "sticker_removed")
//                )
//            );
//
//            string[] commands = {"/delpack", stickerSetName, "Yes, I am totally sure."};
//            foreach (string msgText in commands)
//            {
//                await BotClient.SendTextMessageAsync(_fixture.SupergroupChat, msgText);
//            }
//
//            Update caUpdate = await _fixture.UpdateReceiver.GetCallbackQueryUpdateAsync(data: "sticker_removed");
//            await BotClient.AnswerCallbackQueryAsync(caUpdate.CallbackQuery.Id);
//        }
//
//        [OrderedFact("Should create new sticker set with the file sent")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.CreateNewStickerSet)]
//        public async Task Should_Create_New_Sticker_Set()
//        {
//            int setOwnerId = _classFixture.OwnerUserId;
//            string stickerSetName = _classFixture.TestStickerSetName;
//            string gnuStickerFileId = _classFixture.UploadedStickers.First().FileId;
//
//            try
//            {
//                await BotClient.CreateNewStickerSetAsync(
//                    /* userId: */ setOwnerId,
//                    /* name: */ stickerSetName,
//                    /* title: */ "Test Sticker Set",
//                    /* pngSticker: */ gnuStickerFileId,
//                    /* emojis: */ "😁"
//                );
//            }
//            catch (HttpRequestException e)
//                when (e.Message.Contains("500"))
//            {
//                // ToDo. Ignore because of Bot API bug. Sticker is in fact created and available shortly.
//            }
//        }
//
//        [OrderedFact("Should throw " + nameof(StickerSetNameExistsException) +
//                     " while trying to create sticker set with the same name")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.CreateNewStickerSet)]
//        public async Task Should_Throw_StickerSetNameExistsException()
//        {
//            Exception exception;
//            using (System.IO.Stream stream = System.IO.File.OpenRead(Constants.FileNames.Photos.Ruby))
//            {
//                exception = await Assert.ThrowsAsync<Exception>(async () =>
//                    await BotClient.CreateNewStickerSetAsync(
//                        userId: _classFixture.OwnerUserId,
//                        name: _classFixture.TestStickerSetName,
//                        title: "Another Test Sticker Set",
//                        pngSticker: stream,
//                        emojis: "😎"
//                    )
//                );
//            }
//
//            if (exception is HttpRequestException httpReqException && httpReqException.Message.Contains("500"))
//            {
//                // ToDo. Ignore because of Bot API bug. Sticker is in fact created and available shortly.
//                return;
//            }
//
//            Assert.IsType<StickerSetNameExistsException>(exception);
//        }
//
//        #endregion
//
//        #region 3. Edit sticker set
//
//        [OrderedFact("Should add Tux sticker to set using its uploaded file_id")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AddStickerToSet)]
//        public async Task Should_Add_Uploaded_Sticker_File_To_Set()
//        {
//            try
//            {
//                await BotClient.AddStickerToSetAsync(
//                    userId: _classFixture.OwnerUserId,
//                    name: _classFixture.TestStickerSetName,
//                    pngSticker: _classFixture.UploadedStickers.Last().FileId,
//                    emojis: "😏😃"
//                );
//            }
//            catch (HttpRequestException e)
//                when (e.Message.Contains("500"))
//            {
//                // ToDo. Ignore because of Bot API bug. Sticker is in fact created and available shortly.
//            }
//        }
//
//        [OrderedFact("Should add VLC logo sticker with mask position like hat on forehead")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AddStickerToSet)]
//        public async Task Should_Add_Sticker_With_Mask_Position_To_Set()
//        {
//            using (System.IO.Stream stream = System.IO.File.OpenRead(Constants.FileNames.Photos.Vlc))
//            {
//                try
//                {
//                    await BotClient.AddStickerToSetAsync(
//                        userId: _classFixture.OwnerUserId,
//                        name: _classFixture.TestStickerSetName,
//                        pngSticker: stream,
//                        emojis: "😇",
//                        maskPosition: new MaskPosition
//                        {
//                            Point = MaskPositionPoint.Forehead,
//                            Scale = .8f
//                        }
//                    );
//                }
//                catch (HttpRequestException e)
//                    when (e.Message.Contains("500"))
//                {
//                    // ToDo. Ignore because of Bot API bug. Sticker is in fact created and available shortly.
//                }
//            }
//        }
//
//        [OrderedFact("Should set Tux in first position of sticker set", Skip = "Bot API Bug")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetStickerPositionInSet)]
//        public async Task Should_Change_Sticker_Position_In_Set()
//        {
//            string stickerSetName = _classFixture.TestStickerSetName;
//
//            StickerSet stickerSet = await BotClient.GetStickerSetAsync(stickerSetName);
//            Sticker tuxSticker = stickerSet.Stickers[0];
//
//            await BotClient.SetStickerPositionInSetAsync(
//                /* sticker: */ tuxSticker.FileId,
//                /* position: */ 0
//            );
//        }
//
//        [OrderedFact("Should remove all stickers from test sticker set except VLC's logo", Skip = "Bot API Bug")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.DeleteStickerFromSet)]
//        public async Task Should_Remove_All_Stickers_From_Set()
//        {
//            string stickerSetName = _classFixture.TestStickerSetName;
//
//            StickerSet stickerSet = await BotClient.GetStickerSetAsync(stickerSetName);
//
//            foreach (Sticker sticker in stickerSet.Stickers)
//            {
//                await BotClient.DeleteStickerFromSetAsync(
//                    /* sticker: */ sticker.FileId
//                );
//            }
//        }
//
//        [OrderedFact(
//            "Should throw StickerSetNotModifiedException while trying to remove the last sticker in the set",
//            Skip = "Bot API Bug")]
//        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.DeleteStickerFromSet)]
//        public async Task Should_Throw_StickerSetNotModifiedException()
//        {
//            StickerSet testStickerSet = await BotClient.GetStickerSetAsync(_classFixture.TestStickerSet.Name);
//
//            BadRequestException exception = await Assert.ThrowsAsync<BadRequestException>(async () =>
//                await BotClient.DeleteStickerFromSetAsync(
//                    sticker: testStickerSet.Stickers.Single().FileId
//                )
//            );
//
//            Assert.IsType<StickerSetNotModifiedException>(exception);
//        }
//
//        #endregion
//    }
//}
