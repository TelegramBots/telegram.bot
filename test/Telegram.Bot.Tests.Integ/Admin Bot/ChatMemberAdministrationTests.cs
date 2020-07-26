using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Admin_Bot
{
    [Collection(Constants.TestCollections.ChatMemberAdministration)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class ChatMemberAdministrationTests : IClassFixture<ChatMemberAdministrationTestFixture>
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        private readonly ChatMemberAdministrationTestFixture _classFixture;

        public ChatMemberAdministrationTests(TestsFixture fixture, ChatMemberAdministrationTestFixture classFixture)
        {
            _fixture = fixture;
            _classFixture = classFixture;
        }

        #region Kick, Unban, and Invite chat member back

        [OrderedFact("Should kick user from chat and ban him/her for ever")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.KickChatMember)]
        public async Task Should_Kick_Chat_Member_For_Ever()
        {
            await BotClient.KickChatMemberAsync(
                chatId: _fixture.SupergroupChat.Id,
                userId: _classFixture.RegularMemberUserId
            );
        }

        [OrderedFact("Should unban a chat member")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.UnbanChatMember)]
        public async Task Should_Unban_Chat_Member()
        {
            await BotClient.UnbanChatMemberAsync(
                chatId: _fixture.SupergroupChat.Id,
                userId: _classFixture.RegularMemberUserId
            );
        }

        [OrderedFact("Should export an invite link to the group")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.ExportChatInviteLink)]
        public async Task Should_Export_Chat_Invite_Link()
        {
            string result = await BotClient.ExportChatInviteLinkAsync(chatId: _fixture.SupergroupChat.Id);

            Assert.StartsWith("https://t.me/joinchat/", result);

            _classFixture.GroupInviteLink = result;
        }

        [OrderedFact("Should receive a notification of new member (same kicked member) joining the chat")]
        public async Task Should_Receive_New_Chat_Member_Notification()
        {
            await _fixture.SendTestInstructionsAsync(
                $"@{_classFixture.RegularMemberUserName.Replace("_", @"\_")} should join the " +
                "group using invite link sent to him/her in private chat"
            );

            await _fixture.UpdateReceiver.DiscardNewUpdatesAsync();

            await BotClient.SendTextMessageAsync(
                chatId: _classFixture.RegularMemberChat,
                text: _classFixture.GroupInviteLink
            );

            Update update = await _fixture.UpdateReceiver
                    .GetUpdateAsync(u =>
                            u.Message?.Chat?.Type == ChatType.Supergroup &&
                            u.Message.Chat.Id == _fixture.SupergroupChat.Id &&
                            u.Message.Type == MessageType.ChatMembersAdded,
                        updateTypes: UpdateType.Message
                    );

            await _fixture.UpdateReceiver.DiscardNewUpdatesAsync();

            Message serviceMsg = update.Message;

            Assert.Equal(
                _classFixture.RegularMemberUserId.ToString(),
                serviceMsg!.NewChatMembers!.Single().Id.ToString()
            );
        }

        #endregion

        #region Promote and Restrict Chat Member

        [OrderedFact("Should promote chat member to change chat information")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.PromoteChatMember)]
        public async Task Should_Promote_User_To_Change_Chat_Info()
        {
            await BotClient.PromoteChatMemberAsync(
                chatId: _fixture.SupergroupChat.Id,
                userId: _classFixture.RegularMemberUserId,
                canChangeInfo: true
            );
        }

        [OrderedFact("Should set a custom title for the previously promoted admin")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetChatAdministratorCustomTitle)]
        public async Task Should_Set_Custom_Title_For_Admin()
        {
            ChatMember promotedRegularUser = await BotClient.GetChatMemberAsync(
                chatId: _fixture.SupergroupChat,
                userId: _classFixture.RegularMemberUserId
            );

            await BotClient.SetChatAdministratorCustomTitleAsync(
                chatId: _fixture.SupergroupChat,
                userId: promotedRegularUser.User.Id,
                customTitle: "CHANGED TITLE"
            );

            ChatMember newChatMember = await BotClient.GetChatMemberAsync(
                chatId: _fixture.SupergroupChat,
                userId: promotedRegularUser.User.Id
            );

            Assert.Equal("CHANGED TITLE", newChatMember.CustomTitle);

            // Restore default title by sending empty string
            await BotClient.SetChatAdministratorCustomTitleAsync(
                chatId: _fixture.SupergroupChat,
                userId: promotedRegularUser.User.Id,
                customTitle: string.Empty
            );
        }

        [OrderedFact("Should demote chat member by taking his/her only admin right: change_info")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.PromoteChatMember)]
        public async Task Should_Demote_User()
        {
            await BotClient.PromoteChatMemberAsync(
                chatId: _fixture.SupergroupChat.Id,
                userId: _classFixture.RegularMemberUserId,
                canChangeInfo: false
            );
        }

        [OrderedFact("Should restrict chat member from sending stickers temporarily")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.RestrictChatMember)]
        public async Task Should_Restrict_Sending_Stickers_Temporarily()
        {
            const int banSeconds = 50;

            await BotClient.RestrictChatMemberAsync(
                chatId: _fixture.SupergroupChat.Id,
                userId: _classFixture.RegularMemberUserId,
                untilDate: DateTime.UtcNow.AddSeconds(banSeconds),
                permissions: new ChatPermissions
                {
                    CanSendMessages = true,
                    CanSendMediaMessages = true,
                    CanSendOtherMessages = false
                }
            );
        }

        #endregion

        #region Kick chat member temporarily

        [OrderedFact("Should kick user from chat and ban him/her temporarily")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.KickChatMember)]
        public async Task Should_Kick_Chat_Member_Temporarily()
        {
            const int banSeconds = 50;
            await _fixture.SendTestInstructionsAsync(
                $"@{_classFixture.RegularMemberUserName.Replace("_", @"\_")} should be able to join again in" +
                $" *{banSeconds} seconds* via the link shared in private chat with him/her"
            );

            await BotClient.KickChatMemberAsync(
                chatId: _fixture.SupergroupChat.Id,
                userId: _classFixture.RegularMemberUserId,
                untilDate: DateTime.UtcNow.AddSeconds(banSeconds)
            );
        }

        #endregion
    }
}
