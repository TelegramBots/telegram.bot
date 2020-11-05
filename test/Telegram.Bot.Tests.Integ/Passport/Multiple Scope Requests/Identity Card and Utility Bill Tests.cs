using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IntegrationTests;
using Telegram.Bot.Passport;
using Telegram.Bot.Passport.Request;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Tests.Integ.Framework.Fixtures;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Passport;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable once CheckNamespace
namespace Telegram.Bot.Tests.Integ.Passport
{
    [Collection(Constants.TestCollections.Passport.IdentityCartAndUtilityBills)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [Trait(Constants.PassportTraitName, Constants.PassportTraitValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class IdentityCardAndUtilityBillTests : IClassFixture<EntityFixture<Update>>
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        private readonly EntityFixture<Update> _classFixture;

        private readonly ITestOutputHelper _output;

        public IdentityCardAndUtilityBillTests(
            TestsFixture fixture,
            EntityFixture<Update> classFixture,
            ITestOutputHelper output)
        {
            _fixture = fixture;
            _classFixture = classFixture;
            _output = output;
        }

        [OrderedFact("Should generate passport authorization request link")]
        public async Task Should_Generate_Auth_Link()
        {
            const string publicKey = "-----BEGIN PUBLIC KEY-----\n" +
                                     "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA0VElWoQA2SK1csG2/sY/\n" +
                                     "wlssO1bjXRx+t+JlIgS6jLPCefyCAcZBv7ElcSPJQIPEXNwN2XdnTc2wEIjZ8bTg\n" +
                                     "BlBqXppj471bJeX8Mi2uAxAqOUDuvGuqth+mq7DMqol3MNH5P9FO6li7nZxI1FX3\n" +
                                     "9u2r/4H4PXRiWx13gsVQRL6Clq2jcXFHc9CvNaCQEJX95jgQFAybal216EwlnnVV\n" +
                                     "giT/TNsfFjW41XJZsHUny9k+dAfyPzqAk54cgrvjgAHJayDWjapq90Fm/+e/DVQ6\n" +
                                     "BHGkV0POQMkkBrvvhAIQu222j+03frm9b2yZrhX/qS01lyjW4VaQytGV0wlewV6B\n" +
                                     "FwIDAQAB\n" +
                                     "-----END PUBLIC KEY-----";
            PassportScope scope = new PassportScope(new[]
            {
                new PassportScopeElementOneOfSeveral(new[]
                {
                    new PassportScopeElementOne(PassportEnums.Scope.IdentityCard),
                    new PassportScopeElementOne(PassportEnums.Scope.InternalPassport),
                })
                {
                    Selfie = true, // selfie can only be requested for documents used as proof of identity
                },
                new PassportScopeElementOneOfSeveral(new[]
                {
                    new PassportScopeElementOne(PassportEnums.Scope.UtilityBill),
                    new PassportScopeElementOne(PassportEnums.Scope.RentalAgreement),
                })
                {
                    Translation = true,
                    // Selfie = null // selfie cannot be requested for documents used as proof of address
                },
            });
            AuthorizationRequestParameters authReq = new AuthorizationRequestParameters(
                botId: _fixture.BotUser.Id,
                publicKey: publicKey,
                nonce: "Test nonce for id card & utility bill",
                scope: scope
            );

            await BotClient.SendTextMessageAsync(
                _fixture.SupergroupChat,
                "Share your *identity card with a selfie* and " +
                "a *utility bill with its translation* with bot using Passport.\n\n" +
                "1. Click inline button\n" +
                "2. Open link in browser to redirect you back to Telegram passport\n" +
                "3. Authorize bot to access the info",
                ParseMode.Markdown,
                replyMarkup: (InlineKeyboardMarkup) InlineKeyboardButton.WithUrl(
                    "Share via Passport",
                    $"https://telegrambots.github.io/Telegram.Bot.Extensions.Passport/redirect.html?{authReq.Query}"
                )
            );

            Update passportUpdate = await _fixture.UpdateReceiver.GetPassportUpdate();
            _classFixture.Entity = passportUpdate;
        }

        [OrderedFact("Should validate values in the Passport massage")]
        public void Should_Validate_Passport_Message()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message!.PassportData;

            #region identity card element validation

            EncryptedPassportElement idCardEl = Assert.Single(passportData!.Data, el => el.Type == "identity_card");
            Assert.NotNull(idCardEl);
            Assert.Equal(PassportEnums.Scope.IdentityCard, idCardEl.Type);

            Assert.NotNull(idCardEl.Data);
            Assert.NotEmpty(idCardEl.Data);
            Assert.NotEmpty(idCardEl.Hash);

            Assert.NotNull(idCardEl.FrontSide);
            Assert.NotEmpty(idCardEl.FrontSide.FileId);
            Assert.InRange(idCardEl.FrontSide.FileDate, new DateTime(2018, 6, 1), DateTime.UtcNow);

            Assert.NotNull(idCardEl.ReverseSide);
            Assert.NotEmpty(idCardEl.ReverseSide.FileId);
            Assert.InRange(idCardEl.ReverseSide.FileDate, new DateTime(2018, 6, 1), DateTime.UtcNow);

            Assert.NotNull(idCardEl.Selfie);
            Assert.NotEmpty(idCardEl.Selfie.FileId);
            Assert.InRange(idCardEl.Selfie.FileDate, new DateTime(2018, 6, 1), DateTime.UtcNow);

            #endregion

            #region utility bill element validation

            EncryptedPassportElement billElement = Assert.Single(passportData.Data, el => el.Type == "utility_bill");
            Assert.NotNull(billElement);
            Assert.Equal(PassportEnums.Scope.UtilityBill, billElement.Type);

            Assert.NotEmpty(billElement.Hash);
            Assert.Null(billElement.Data);
            Assert.NotNull(billElement.Files);

            PassportFile billScanFile = Assert.Single(billElement.Files);
            Assert.NotNull(billScanFile);
            Assert.NotEmpty(billScanFile.FileId);
            Assert.InRange(billScanFile.FileDate, new DateTime(2018, 6, 1), DateTime.UtcNow);

            PassportFile billTranslationFile = Assert.Single(billElement.Files);
            Assert.NotNull(billTranslationFile);
            Assert.NotEmpty(billTranslationFile.FileId);
            Assert.InRange(billTranslationFile.FileDate, new DateTime(2018, 6, 1), DateTime.UtcNow);

            #endregion

            Assert.NotNull(passportData.Credentials);
            Assert.NotEmpty(passportData.Credentials.Data);
            Assert.NotEmpty(passportData.Credentials.Hash);
            Assert.NotEmpty(passportData.Credentials.Secret);
        }

        [OrderedFact("Should decrypt and validate credentials")]
        public void Should_Decrypt_Credentials()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message!.PassportData;

            RSA key = EncryptionKey.ReadAsRsa();

            IDecrypter decrypter = new Decrypter();

            Credentials credentials = decrypter.DecryptCredentials(
                key: key,
                encryptedCredentials: passportData!.Credentials
            );

            Assert.NotNull(credentials);
            Assert.NotNull(credentials.SecureData);
            Assert.Equal("Test nonce for id card & utility bill", credentials.Nonce);

            // decryption of document data in 'identity_card' element requires accompanying DataCredentials
            Assert.NotNull(credentials.SecureData.IdentityCard);
            Assert.NotNull(credentials.SecureData.IdentityCard.Data);
            Assert.NotEmpty(credentials.SecureData.IdentityCard.Data.Secret);
            Assert.NotEmpty(credentials.SecureData.IdentityCard.Data.DataHash);

            // decryption of front side of 'identity_card' element requires accompanying FileCredentials
            Assert.NotNull(credentials.SecureData.IdentityCard.FrontSide);
            Assert.NotEmpty(credentials.SecureData.IdentityCard.FrontSide.Secret);
            Assert.NotEmpty(credentials.SecureData.IdentityCard.FrontSide.FileHash);

            // decryption of reverse side of 'identity_card' element requires accompanying FileCredentials
            Assert.NotNull(credentials.SecureData.IdentityCard.ReverseSide);
            Assert.NotEmpty(credentials.SecureData.IdentityCard.ReverseSide.Secret);
            Assert.NotEmpty(credentials.SecureData.IdentityCard.ReverseSide.FileHash);

            // decryption of selfie of 'identity_card' element requires accompanying FileCredentials
            Assert.NotNull(credentials.SecureData.IdentityCard.Selfie);
            Assert.NotEmpty(credentials.SecureData.IdentityCard.Selfie.Secret);
            Assert.NotEmpty(credentials.SecureData.IdentityCard.Selfie.FileHash);

            Assert.Null(credentials.SecureData.IdentityCard.Translation);
            Assert.Null(credentials.SecureData.IdentityCard.Files);

            // decryption of file scan in 'utility_bill' element requires accompanying FileCredentials
            Assert.NotNull(credentials.SecureData.UtilityBill.Files);
            FileCredentials billCredentials = Assert.Single(credentials.SecureData.UtilityBill.Files);
            Assert.NotEmpty(billCredentials.Secret);
            Assert.NotEmpty(billCredentials.FileHash);

            // decryption of translation file scan in 'utility_bill' element requires accompanying FileCredentials
            Assert.NotNull(credentials.SecureData.UtilityBill.Files);
            FileCredentials billTranslationFileCredentials =
                Assert.Single(credentials.SecureData.UtilityBill.Translation);
            Assert.NotEmpty(billTranslationFileCredentials.Secret);
            Assert.NotEmpty(billTranslationFileCredentials.FileHash);
        }

        [OrderedFact("Should decrypt document data in 'identity_card' element")]
        public void Should_Decrypt_Identity_Card_Element_Document()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message!.PassportData;

            RSA key = EncryptionKey.ReadAsRsa();

            IDecrypter decrypter = new Decrypter();
            Credentials credentials = decrypter.DecryptCredentials(passportData!.Credentials, key);
            EncryptedPassportElement idCardEl = Assert.Single(passportData.Data, el => el.Type == "identity_card");

            Assert.NotNull(idCardEl!.Data);

            IdDocumentData documentData = decrypter.DecryptData<IdDocumentData>(
                idCardEl.Data,
                credentials.SecureData.IdentityCard.Data
            );

            Assert.NotEmpty(documentData.DocumentNo);
            if (string.IsNullOrEmpty(documentData.ExpiryDate))
            {
                Assert.Null(documentData.Expiry);
            }
            else
            {
                Assert.NotNull(documentData.Expiry);
            }
        }

        [OrderedFact("Should decrypt front side photo in 'identity_card' element")]
        public async Task Should_Decrypt_Identity_Card_Element_Front_Side()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message!.PassportData;
            RSA key = EncryptionKey.ReadAsRsa();
            EncryptedPassportElement idCardEl = Assert.Single(passportData!.Data, el => el.Type == "identity_card");

            Assert.NotNull(idCardEl!.FrontSide);
            // might be null after all
            //Assert.NotNull(idCardEl!.FrontSide.FileSize);

            IDecrypter decrypter = new Decrypter();
            Credentials credentials = decrypter.DecryptCredentials(passportData.Credentials, key);

            byte[] encryptedContent;
            await using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                await BotClient.GetInfoAndDownloadFileAsync(
                    idCardEl.FrontSide.FileId,
                    stream
                );
                encryptedContent = stream.ToArray();
            }

            byte[] content = decrypter.DecryptFile(
                encryptedContent,
                credentials.SecureData.IdentityCard.FrontSide
            );

            Assert.NotEmpty(content);
        }

        [OrderedFact("Should decrypt reverse side photo in 'identity_card' element from HTTP response " +
                     "and write it to a file on disk")]
        public async Task Should_Decrypt_Identity_Card_Element_Reverse_Side()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message!.PassportData;
            RSA key = EncryptionKey.ReadAsRsa();
            EncryptedPassportElement idCardEl = Assert.Single(passportData!.Data, el => el.Type == "identity_card");
            Assert.NotNull(idCardEl!.ReverseSide);

            IDecrypter decrypter = new Decrypter();
            Credentials credentials = decrypter.DecryptCredentials(passportData.Credentials, key);

            string botToken = ConfigurationProvider.TestConfigurations.ApiToken;
            File encFileInfo = await BotClient.GetFileAsync(idCardEl.ReverseSide.FileId);

            Assert.NotNull(encFileInfo.FileSize);

            HttpClient http = new HttpClient();
            await using System.IO.Stream encFileStream = await http.GetStreamAsync(
                $"https://api.telegram.org/file/bot{botToken}/{encFileInfo.FilePath}"
            );
            string destFilePath = System.IO.Path.GetTempFileName();

            await using System.IO.Stream reverseSideFile = System.IO.File.OpenWrite(destFilePath);

            await decrypter.DecryptFileAsync(
                encFileStream,
                credentials.SecureData.IdentityCard.ReverseSide,
                reverseSideFile
            );

            Assert.InRange(
                reverseSideFile.Length,
                encFileInfo.FileSize.Value - 256,
                encFileInfo.FileSize.Value + 256
            );

            _output.WriteLine("Reverse side photo is written to file \"{0}\".", destFilePath);
        }

        [OrderedFact("Should decrypt selfie photo in 'identity_card' element")]
        public async Task Should_Decrypt_Identity_Card_Element_Selfie()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message!.PassportData;
            RSA key = EncryptionKey.ReadAsRsa();
            EncryptedPassportElement idCardEl = Assert.Single(passportData!.Data, el => el.Type == "identity_card");

            Assert.NotNull(idCardEl!.Selfie);
            //might be null
            //Assert.NotNull(idCardEl!.Selfie.FileSize);

            IDecrypter decrypter = new Decrypter();
            Credentials credentials = decrypter.DecryptCredentials(passportData.Credentials, key);

            byte[] encryptedContent;
            await using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                await BotClient.GetInfoAndDownloadFileAsync(
                    idCardEl.Selfie.FileId,
                    stream
                );
                encryptedContent = stream.ToArray();
            }

            byte[] content = decrypter.DecryptFile(
                encryptedContent,
                credentials.SecureData.IdentityCard.Selfie
            );

            Assert.NotEmpty(content);
        }

        [OrderedFact("Should decrypt the single file in 'utility_bill' element")]
        public async Task Should_Decrypt_Utility_Bill_Element_File()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message!.PassportData;
            RSA key = EncryptionKey.ReadAsRsa();

            EncryptedPassportElement billElement = Assert.Single(passportData!.Data, el => el.Type == "utility_bill");
            Assert.NotNull(billElement!.Files);

            PassportFile billScanFile = Assert.Single(billElement.Files);
            //null
            //Assert.NotNull(billScanFile!.FileSize);

            IDecrypter decrypter = new Decrypter();
            Credentials credentials = decrypter.DecryptCredentials(passportData.Credentials, key);

            FileCredentials fileCredentials = Assert.Single(credentials.SecureData.UtilityBill.Files);
            Assert.NotNull(fileCredentials);

            File encryptedFileInfo;
            await using (System.IO.Stream decryptedFile = new System.IO.MemoryStream())
            {
                encryptedFileInfo = await BotClient.DownloadAndDecryptPassportFileAsync(
                    billScanFile,
                    fileCredentials,
                    decryptedFile
                );
                Assert.NotEqual(0, decryptedFile.Length);
                /*
                Assert.InRange(
                    decryptedFile.Length,
                    billScanFile.FileSize.Value - 256,
                    billScanFile.FileSize.Value + 256
                );*/
            }

            Assert.NotNull(encryptedFileInfo.FilePath);
            Assert.NotEmpty(encryptedFileInfo.FilePath);
            Assert.NotEmpty(encryptedFileInfo.FileId);
            Assert.InRange(encryptedFileInfo.FileSize.Value, 1_000, 50_000_000);
        }

        [OrderedFact("Should decrypt the single translation file in 'utility_bill' element")]
        public async Task Should_decrypt_utility_bill_element_translation()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message!.PassportData;
            RSA key = EncryptionKey.ReadAsRsa();

            EncryptedPassportElement billElement = Assert.Single(passportData!.Data, el => el.Type == "utility_bill");
            Assert.NotNull(billElement!.Translation);

            PassportFile translationFile = Assert.Single(billElement.Translation);
            //null
            //Assert.NotNull(translationFile!.FileSize);

            IDecrypter decrypter = new Decrypter();
            Credentials credentials = decrypter.DecryptCredentials(passportData.Credentials, key);

            FileCredentials fileCredentials = Assert.Single(credentials.SecureData.UtilityBill.Translation);

            File encryptedFileInfo;
            await using (System.IO.Stream decryptedFile = new System.IO.MemoryStream())
            {
                encryptedFileInfo = await BotClient.DownloadAndDecryptPassportFileAsync(
                    translationFile!,
                    fileCredentials!,
                    decryptedFile
                );

                Assert.NotEqual(0, decryptedFile.Length);
                // does not have filesize
                /*Assert.InRange(
                    decryptedFile.Length,
                    translationFile.FileSize.Value - 256,
                    translationFile.FileSize.Value + 256
                );*/
            }

            Assert.NotNull(encryptedFileInfo.FilePath);
            Assert.NotEmpty(encryptedFileInfo.FilePath);
            Assert.NotEmpty(encryptedFileInfo.FileId);
            Assert.InRange(encryptedFileInfo.FileSize.Value, 1_000, 50_000_000);
        }
    }
}
