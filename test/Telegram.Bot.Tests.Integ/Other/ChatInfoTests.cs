using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Other
{
    [Collection(Constants.TestCollections.ChatInfo)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class ChatInfoTests
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        public ChatInfoTests(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [OrderedFact("Should get supergroup chat info")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetChat)]
        public async Task Should_Get_Supergroup_Chat()
        {
            Chat supergroupChat = _fixture.SupergroupChat;

            Chat chat = await BotClient.GetChatAsync(
                chatId: supergroupChat.Id
            );

            Assert.Equal(ChatType.Supergroup, chat.Type);
            Assert.Equal(supergroupChat.Id, chat.Id);
            Assert.Equal(supergroupChat.Title, chat.Title);
            Assert.Equal(supergroupChat.Username, chat.Username);
            Assert.Equal(supergroupChat.Description, chat.Description);
            // Don't compare invite links, it's easy to invalidate them accidentally so the test
            // fails for no good reason
            // Assert.Equal(supergroupChat.InviteLink, chat.InviteLink);
            Assert.NotNull(chat.InviteLink);
            Assert.NotEmpty(chat.InviteLink);
            Assert.StartsWith("https://t.me", chat.InviteLink);
            Assert.Equal(supergroupChat.PinnedMessage, chat.PinnedMessage);
            Assert.Equal(supergroupChat.StickerSetName, chat.StickerSetName);
            Assert.Equal(supergroupChat.CanSetStickerSet, chat.CanSetStickerSet);
            Assert.Null(chat.FirstName);
            Assert.Null(chat.LastName);
            Assert.NotNull(chat.Permissions);
        }

        [OrderedFact("Should get chat member: bot(admin)")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetChatMember)]
        public async Task Should_Get_Bot_Chat_Member()
        {
            ChatMember memberBot = await BotClient.GetChatMemberAsync(
                chatId: _fixture.SupergroupChat.Id,
                userId: _fixture.BotUser.Id
            );

            Assert.Equal(ChatMemberStatus.Administrator, memberBot.Status);
            Assert.True(memberBot.CanChangeInfo);
            Assert.True(memberBot.CanDeleteMessages);
            Assert.True(memberBot.CanInviteUsers);
            Assert.True(memberBot.CanPromoteMembers);
            Assert.True(memberBot.CanRestrictMembers);
            Assert.True(memberBot.CanPinMessages);
            Assert.False(memberBot.CanBeEdited);
            Assert.Null(memberBot.UntilDate);
            Assert.Null(memberBot.CanPostMessages);
            Assert.Null(memberBot.CanEditMessages);
            Assert.Null(memberBot.CanSendMessages);
            Assert.Null(memberBot.CanSendMediaMessages);
            Assert.Null(memberBot.CanSendOtherMessages);
            Assert.Null(memberBot.CanAddWebPagePreviews);
            Asserts.UsersEqual(_fixture.BotUser, memberBot.User);
        }

        [OrderedFact("Should get supergroup chat administrators")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetChatAdministrators)]
        public async Task Should_Get_Chat_Admins()
        {
            ChatMember[] chatAdmins = await BotClient.GetChatAdministratorsAsync(
                chatId: _fixture.SupergroupChat.Id
            );

            ChatMember memberCreator = Assert.Single(chatAdmins, _ => _.Status == ChatMemberStatus.Creator);
            Debug.Assert(memberCreator != null);

            ChatMember memberBot = Assert.Single(chatAdmins, _ => _.User.IsBot);
            Debug.Assert(memberBot != null);

            Assert.True(2 <= chatAdmins.Length); // at least, Bot and the Creator
            Assert.Null(memberCreator.UntilDate);
            Assert.Null(memberCreator.CanBeEdited);
            Assert.Null(memberCreator.CanChangeInfo);
            Assert.Null(memberCreator.CanPostMessages);
            Assert.Null(memberCreator.CanEditMessages);
            Assert.Null(memberCreator.CanDeleteMessages);
            Assert.Null(memberCreator.CanInviteUsers);
            Assert.Null(memberCreator.CanRestrictMembers);
            Assert.Null(memberCreator.CanPinMessages);
            Assert.Null(memberCreator.CanPromoteMembers);
            Assert.Null(memberCreator.CanSendMessages);
            Assert.Null(memberCreator.CanSendMediaMessages);
            Assert.Null(memberCreator.CanSendOtherMessages);
            Assert.Null(memberCreator.CanAddWebPagePreviews);
            Asserts.UsersEqual(_fixture.BotUser, memberBot.User);
        }

        [OrderedFact("Should get private chat info")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetChatAdministrators)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetChat)]
        public async Task Should_Get_Private_Chat()
        {
            /* In order to have a private chat id, take the Creator of supergroup and use his User ID because
             * for a regular user, "User ID" is the same number as "Private Chat ID".
             */
            ChatMember[] chatAdmins = await BotClient.GetChatAdministratorsAsync(chatId: _fixture.SupergroupChat);

            long privateChatId = chatAdmins
                .Single(member => member.Status == ChatMemberStatus.Creator)
                .User.Id;

            Chat chat = await BotClient.GetChatAsync(
                chatId: privateChatId
            );

            Assert.Equal(ChatType.Private, chat.Type);
            Assert.Equal(privateChatId, chat.Id);

            // Mandatory fields:
            Assert.NotEmpty(chat.Username!);
            Assert.NotEmpty(chat.FirstName!);

            // Following fields of a chat do not apply to a private chat:
            Assert.Null(chat.Title);
            Assert.Null(chat.Permissions);
            Assert.Null(chat.Description);
            Assert.Null(chat.InviteLink);
            Assert.Null(chat.PinnedMessage);
            Assert.Null(chat.StickerSetName);
            Assert.Null(chat.CanSetStickerSet);
        }

        [OrderedFact("Should get chat members count")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetChatMembersCount)]
        public async Task Should_Get_Chat_Members_Count()
        {
            int membersCount = await BotClient.GetChatMembersCountAsync(
                chatId: _fixture.SupergroupChat.Id
            );

            Assert.True(2 <= membersCount); // at least, Bot and the Creator
        }

        /// <remarks>
        /// The status is set for 5 seconds or less (when a message arrives from your bot, Telegram clients clear
        /// its typing status)
        /// </remarks>
        [OrderedFact("Should send action to chat: recording audio")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendChatAction)]
        public async Task Should_Send_Chat_Action()
        {
            await BotClient.SendChatActionAsync(
                chatId: _fixture.SupergroupChat.Id,
                chatAction: ChatAction.RecordAudio
            );

            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}
