using System;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Games
{
    /// <summary>
    /// This fixture is for Games tests and it ensures test bot has a game set up before running the test methods.
    /// </summary>
    public class GamesFixture : IAsyncLifetime
    {
        public string GameShortName { get; }

        public Message GameMessage { set; get; }

        public string InlineGameMessageId { set; get; }

        public GameHighScore[] HighScores { set; get; }

        /// <summary>
        /// A chat admin to set the game scores for.
        /// </summary>
        public User Player { get; private set; }

        private readonly TestsFixture _fixture;

        public GamesFixture(TestsFixture fixture)
        {
            _fixture = fixture;
            GameShortName = "game1";
        }

        private async Task<User> GetPlayerIdFromChatAdmins(long chatId)
        {
            ChatMember[] admins = await _fixture.BotClient.GetChatAdministratorsAsync(chatId);
            ChatMember player = admins[new Random(DateTime.Now.Millisecond).Next(admins.Length)];
            return player.User;
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _fixture.BotClient.SendGameAsync(_fixture.SupergroupChat.Id, GameShortName);
            }
            catch (ApiRequestException e) when (e.ErrorCode == 400 && e.Message.Contains("game_short_name"))
            {
                throw new ArgumentException(
                    $@"Bot doesn't have game: ""{GameShortName}"". Make sure you set up a game with @BotFather.",
                    e
                );
            }

            Player = await GetPlayerIdFromChatAdmins(_fixture.SupergroupChat.Id);
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
