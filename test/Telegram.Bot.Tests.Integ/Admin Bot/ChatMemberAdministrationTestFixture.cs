using System;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Admin_Bot
{
    public class ChatMemberAdministrationTestFixture : IAsyncLifetime
    {
        public Chat RegularMemberChat { get; private set; }

        public int RegularMemberUserId { get; private set; }

        public string RegularMemberUserName { get; private set; }

        public string GroupInviteLink { get; set; }

        private readonly TestsFixture _testsFixture;

        public ChatMemberAdministrationTestFixture(TestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
        }

        private static async Task<Chat> GetChat(TestsFixture testsFixture, string collectionName)
        {
            Chat chat;

            if (int.TryParse(ConfigurationProvider.TestConfigurations.RegularGroupMemberId, out int userId))
            {
                chat = await testsFixture.BotClient.GetChatAsync(userId);
            }
            else
            {
                await testsFixture.UpdateReceiver.DiscardNewUpdatesAsync();

                await testsFixture.SendTestCollectionNotificationAsync(collectionName,
                    $"No value is set for `{nameof(ConfigurationProvider.TestConfigurations.RegularGroupMemberId)}` " +
                    "in test settings.\n" +
                    "An admin should forward a message from non-admin member or send his/her contact."
                );

                chat = await testsFixture.GetChatFromAdminAsync();
            }

            if (chat.Username is null)
            {
                await testsFixture.SendTestCollectionNotificationAsync(collectionName,
                    $"[{chat.FirstName}](tg://user?id={chat.Id}) doesn't have a username.\n" +
                    "❎ Failing tests...");

                throw new ArgumentNullException(nameof(chat.Username), "Chat member doesn't have a username");
            }

            return chat;
        }

        public async Task InitializeAsync()
        {
            const string collectionName = Constants.TestCollections.ChatMemberAdministration;

            RegularMemberChat = await GetChat(_testsFixture, collectionName);

            await _testsFixture.SendTestCollectionNotificationAsync(
                collectionName,
                $"Chosen regular member is @{RegularMemberChat.Username!.Replace("_", @"\_")}"
            );

            RegularMemberUserId = (int) RegularMemberChat.Id;
            RegularMemberUserName = RegularMemberChat.Username;
            // Updates from regular user will be received
            _testsFixture.UpdateReceiver.AllowedUsernames.Add(RegularMemberUserName);
        }

        public Task DisposeAsync()
        {
            // Remove regular user from AllowedUserNames
            _testsFixture.UpdateReceiver.AllowedUsernames.Remove(RegularMemberUserName);

            return Task.CompletedTask;
        }
    }
}
