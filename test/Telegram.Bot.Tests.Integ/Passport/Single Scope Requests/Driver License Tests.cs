// ReSharper disable PossibleNullReferenceException
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Telegram.Bot;
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

namespace IntegrationTests
{
    [Collection(Constants.TestCollections.Passport.DriverLicense)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [Trait(Constants.PassportTraitName, Constants.PassportTraitValue)]
    public class DriverLicenseTests : IClassFixture<EntityFixture<Update>>
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        private readonly EntityFixture<Update> _classFixture;

        private readonly ITestOutputHelper _output;

        public DriverLicenseTests(TestsFixture fixture, EntityFixture<Update> classFixture, ITestOutputHelper output)
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
                new PassportScopeElementOne(PassportEnums.Scope.DriverLicense)
                {
                    Selfie = true,
                    Translation = true,
                },
            });
            AuthorizationRequestParameters authReq = new AuthorizationRequestParameters(
                botId: _fixture.BotUser.Id,
                publicKey: publicKey,
                nonce: "Test nonce for driver license",
                scope: scope
            );

            await BotClient.SendTextMessageAsync(
                _fixture.SupergroupChat,
                "Share your *driver license* in addition to *a selfie with it* and *a translation scan* " +
                "with bot using Telegram Passport.\n\n" +
                "1. Click inline button\n" +
                "2. Open link in browser to so it redirects you to Telegram Passport\n" +
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

        [OrderedFact("Should validate driver license values in the Passport massage")]
        public void Should_Validate_Passport_Update()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message.PassportData;

            EncryptedPassportElement encryptedElement = Assert.Single(passportData.Data);
            Assert.NotNull(encryptedElement);
            Assert.Equal("driver_license", encryptedElement.Type);
            Assert.Equal(PassportEnums.Scope.DriverLicense, encryptedElement.Type);

            Assert.NotNull(encryptedElement.Data);
            Assert.NotEmpty(encryptedElement.Data);
            Assert.NotEmpty(encryptedElement.Hash);

            Assert.NotNull(encryptedElement.FrontSide);
            Assert.NotEmpty(encryptedElement.FrontSide.FileId);
            Assert.InRange(encryptedElement.FrontSide.FileSize.Value, 1_000, 50_000_000);

            Assert.NotNull(encryptedElement.ReverseSide);
            Assert.NotEmpty(encryptedElement.ReverseSide.FileId);
            if (encryptedElement.ReverseSide.FileSize.HasValue)
            {
                Assert.InRange(encryptedElement.ReverseSide.FileSize.Value, 1_000, 50_000_000);
            }


            Assert.NotNull(encryptedElement.Selfie);
            Assert.NotEmpty(encryptedElement.Selfie.FileId);
            Assert.InRange(encryptedElement.Selfie.FileSize.Value, 1_000, 50_000_000);

            Assert.NotNull(encryptedElement.Translation);
            Assert.NotEmpty(encryptedElement.Translation);
            Assert.All(encryptedElement.Translation, Assert.NotNull);
            Assert.All(
                encryptedElement.Translation,
                translation => Assert.NotEmpty(translation.FileId)
            );
            Assert.All(
                encryptedElement.Translation,
                translation => Assert.InRange(translation.FileSize.Value, 1_000, 50_000_000)
            );

            Assert.NotNull(passportData.Credentials);
            Assert.NotEmpty(passportData.Credentials.Data);
            Assert.NotEmpty(passportData.Credentials.Hash);
            Assert.NotEmpty(passportData.Credentials.Secret);
        }

        [OrderedFact("Should decrypt and validate credentials")]
        public void Should_Decrypt_Credentials()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message.PassportData;
            EncryptedPassportElement element = passportData.Data.Single();

            RSA key = EncryptionKey.ReadAsRsa();

            IDecrypter decrypter = new Decrypter();

            Credentials credentials = decrypter.DecryptCredentials(passportData.Credentials, key);

            Assert.NotNull(credentials);
            Assert.Equal("Test nonce for driver license", credentials.Nonce);
            Assert.NotNull(credentials.SecureData);

            // decryption of document data in 'driver_license' element requires accompanying DataCredentials
            Assert.NotNull(credentials.SecureData.DriverLicense);
            Assert.NotNull(credentials.SecureData.DriverLicense.Data);
            Assert.NotEmpty(credentials.SecureData.DriverLicense.Data.Secret);
            Assert.NotEmpty(credentials.SecureData.DriverLicense.Data.DataHash);

            // decryption of front side file in 'driver_license' element requires accompanying FileCredentials
            Assert.NotNull(credentials.SecureData.DriverLicense.FrontSide);
            Assert.NotEmpty(credentials.SecureData.DriverLicense.FrontSide.Secret);
            Assert.NotEmpty(credentials.SecureData.DriverLicense.FrontSide.FileHash);

            // decryption of optional selfie file in 'driver_license' element requires accompanying FileCredentials
            Assert.NotNull(credentials.SecureData.DriverLicense.Selfie);
            Assert.NotEmpty(credentials.SecureData.DriverLicense.Selfie.Secret);
            Assert.NotEmpty(credentials.SecureData.DriverLicense.Selfie.FileHash);

            // decryption of optional translation file in 'driver_license' element requires accompanying FileCredentials
            Assert.Equal(
                element.Translation.Length,
                credentials.SecureData.DriverLicense.Translation.Length
            );
        }

        [OrderedFact("Should decrypt document data of 'driver_license' element")]
        public void Should_Decrypt_Document_Data()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message.PassportData;
            EncryptedPassportElement element = passportData.Data.Single();
            Assert.NotNull(element.Data);

            RSA key = EncryptionKey.ReadAsRsa();
            IDecrypter decrypter = new Decrypter();
            Credentials credentials = decrypter.DecryptCredentials(passportData.Credentials, key);

            IdDocumentData licenseDoc = decrypter.DecryptData<IdDocumentData>(
                encryptedData: element.Data,
                dataCredentials: credentials.SecureData.DriverLicense.Data
            );

            Assert.NotEmpty(licenseDoc.DocumentNo);
            if (string.IsNullOrEmpty(licenseDoc.ExpiryDate))
            {
                Assert.Null(licenseDoc.Expiry);
            }
            else
            {
                Assert.NotNull(licenseDoc.Expiry);
            }
        }

        [OrderedFact("Should decrypt front side photo file of 'driver_license' element")]
        public async Task Should_Decrypt_Front_Side_File()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message.PassportData;
            RSA key = EncryptionKey.ReadAsRsa();
            EncryptedPassportElement element = passportData.Data.Single();

            Assert.NotNull(element.FrontSide);

            IDecrypter decrypter = new Decrypter();
            Credentials credentials = decrypter.DecryptCredentials(passportData.Credentials, key);

            File encryptedFileInfo;
            string decryptedFilePath = System.IO.Path.GetTempFileName();
            await using (System.IO.Stream decryptedFile = System.IO.File.OpenWrite(decryptedFilePath))
            {
                encryptedFileInfo = await BotClient.DownloadAndDecryptPassportFileAsync(
                    element.FrontSide,
                    credentials.SecureData.DriverLicense.FrontSide,
                    decryptedFile
                );
            }

            _output.WriteLine("Front side JPEG file is written to \"{0}\".", decryptedFilePath);

            Assert.NotNull(encryptedFileInfo.FilePath);
            Assert.NotEmpty(encryptedFileInfo.FilePath);
            Assert.NotEmpty(encryptedFileInfo.FileId);
            Assert.InRange(encryptedFileInfo.FileSize.Value, 1_000, 50_000_000);
        }

        [OrderedFact("Should decrypt reverse side photo file of 'driver_license' element")]
        public async Task Should_Decrypt_Reverse_Side_File()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message.PassportData;
            RSA key = EncryptionKey.ReadAsRsa();
            EncryptedPassportElement element = passportData.Data.Single();

            //Wrong assertion for some
            //Assert.NotNull(element.ReverseSide.FileSize);

            IDecrypter decrypter = new Decrypter();
            Credentials credentials = decrypter.DecryptCredentials(passportData.Credentials, key);

            File encryptedFileInfo;
            string decryptedFilePath = System.IO.Path.GetTempFileName();
            await using (System.IO.Stream
                encryptedContent = new System.IO.MemoryStream(),
                decryptedFile = System.IO.File.OpenWrite(decryptedFilePath)
            )
            {
                encryptedFileInfo = await BotClient.GetInfoAndDownloadFileAsync(
                    element.ReverseSide.FileId,
                    encryptedContent
                );
                encryptedContent.Position = 0;

                await decrypter.DecryptFileAsync(
                    encryptedContent,
                    credentials.SecureData.DriverLicense.ReverseSide,
                    decryptedFile
                );
            }

            _output.WriteLine("Reverse side JPEG file is written to \"{0}\".", decryptedFilePath);

            Assert.NotNull(encryptedFileInfo.FilePath);
            Assert.NotEmpty(encryptedFileInfo.FilePath);
            Assert.NotEmpty(encryptedFileInfo.FileId);
            Assert.InRange(encryptedFileInfo.FileSize.Value, 1_000, 50_000_000);
        }

        [OrderedFact("Should decrypt selfie photo file of 'driver_license' element and send it to chat")]
        public async Task Should_Decrypt_Selfie_File()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message.PassportData;
            RSA key = EncryptionKey.ReadAsRsa();
            EncryptedPassportElement element = passportData.Data.Single();
            IDecrypter decrypter = new Decrypter();
            Credentials credentials = decrypter.DecryptCredentials(passportData.Credentials, key);

            byte[] encryptedContent;
            {
                File encryptedFileInfo = await BotClient.GetFileAsync(element.Selfie.FileId);

                Assert.NotNull(encryptedFileInfo.FilePath);
                Assert.NotEmpty(encryptedFileInfo.FilePath);
                Assert.NotEmpty(encryptedFileInfo.FileId);
                Assert.NotNull(encryptedFileInfo.FileSize);
                Assert.InRange(encryptedFileInfo.FileSize.Value, 1_000, 50_000_000);

                await using System.IO.MemoryStream stream = new System.IO.MemoryStream(encryptedFileInfo.FileSize.Value);
                await BotClient.DownloadFileAsync(encryptedFileInfo.FilePath, stream);
                encryptedContent = stream.ToArray();
            }

            byte[] selfieContent = decrypter.DecryptFile(
                encryptedContent,
                credentials.SecureData.DriverLicense.Selfie
            );

            Assert.NotEmpty(selfieContent);

            await using (System.IO.Stream stream = new System.IO.MemoryStream(selfieContent))
            {
                await BotClient.SendPhotoAsync(
                    _fixture.SupergroupChat,
                    stream,
                    "selfie with driver license",
                    replyToMessageId: update.Message.MessageId
                );
            }
        }

        [OrderedFact("Should decrypt translation photo files of 'driver_license' element")]
        public async Task Should_Decrypt_Translation_File()
        {
            Update update = _classFixture.Entity;
            PassportData passportData = update.Message.PassportData;
            RSA key = EncryptionKey.ReadAsRsa();
            EncryptedPassportElement element = passportData.Data.Single();

            IDecrypter decrypter = new Decrypter();
            Credentials credentials = decrypter.DecryptCredentials(passportData.Credentials, key);

            for (int i = 0; i < element.Translation.Length; i++)
            {
                PassportFile passportFile = element.Translation[i];
                FileCredentials fileCredentials = credentials.SecureData.DriverLicense.Translation[i];

                byte[] encryptedContent;
                {
                    File encryptedFileInfo = await BotClient.GetFileAsync(passportFile.FileId);

                    Assert.NotNull(encryptedFileInfo.FilePath);
                    Assert.NotEmpty(encryptedFileInfo.FilePath);
                    Assert.NotEmpty(encryptedFileInfo.FileId);
                    Assert.NotNull(encryptedFileInfo.FileSize);
                    Assert.InRange(encryptedFileInfo.FileSize.Value, 1_000, 50_000_000);

                    await using System.IO.MemoryStream stream = new System.IO.MemoryStream(
                        encryptedFileInfo.FileSize.Value
                    );

                    await BotClient.DownloadFileAsync(encryptedFileInfo.FilePath, stream);
                    encryptedContent = stream.ToArray();
                }

                byte[] translationContent = decrypter.DecryptFile(
                    encryptedContent,
                    fileCredentials
                );

                Assert.NotEmpty(translationContent);

                string decryptedFilePath = System.IO.Path.GetTempFileName();
                await System.IO.File.WriteAllBytesAsync(decryptedFilePath, translationContent);
                _output.WriteLine("Translation JPEG file is written to \"{0}\".", decryptedFilePath);
            }
        }
    }
}
