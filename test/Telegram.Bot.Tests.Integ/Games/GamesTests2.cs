using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Games
{
    [Collection(Constants.TestCollections.Games2)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class GamesTests2 : IClassFixture<GamesFixture>
    {
        private readonly TestsFixture _fixture;
        private readonly GamesFixture _classFixture;

        private ITelegramBotClient BotClient => _fixture.BotClient;

        public GamesTests2(TestsFixture fixture, GamesFixture classFixture)
        {
            _fixture = fixture;
            _classFixture = classFixture;
        }

        [OrderedFact("Should send game")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendGame)]
        public async Task Should_Send_Game()
        {
            Message gameMessage = await BotClient.SendGameAsync(
                chatId: _fixture.SupergroupChat.Id,
                gameShortName: _classFixture.GameShortName
            );

            Assert.Equal(MessageType.Game, gameMessage.Type);
            Assert.NotNull(gameMessage.Game);
            Assert.NotEmpty(gameMessage.Game.Title);
            Assert.NotEmpty(gameMessage.Game.Description);
            Assert.NotNull(gameMessage.Game.Photo);
            Assert.NotEmpty(gameMessage.Game.Photo.Select(p => p.FileId));
            Assert.All(gameMessage.Game.Photo, p => Assert.True(p.FileSize > 1_000));
            Assert.All(gameMessage.Game.Photo, p => Assert.True(p.Width > 80));
            Assert.All(gameMessage.Game.Photo, p => Assert.True(p.Height > 40));

            _classFixture.GameMessage = gameMessage;
        }

        [OrderedFact("Should send game with a custom reply markup")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendGame)]
        public async Task Should_Send_Game_With_ReplyMarkup()
        {
            Message gameMessage = await BotClient.SendGameAsync(
                chatId: _fixture.SupergroupChat.Id,
                gameShortName: _classFixture.GameShortName,
                replyMarkup: new[]
                {
                    InlineKeyboardButton.WithCallBackGame(text: "Play"),
                    InlineKeyboardButton.WithCallbackData(textAndCallbackData: "Second button")
                }
            );

            Assert.Equal(MessageType.Game, gameMessage.Type);
            Assert.NotNull(gameMessage.Game);
            Assert.NotEmpty(gameMessage.Game.Title);
            Assert.NotEmpty(gameMessage.Game.Description);
            Assert.NotNull(gameMessage.Game.Photo);
            Assert.NotEmpty(gameMessage.Game.Photo.Select(p => p.FileId));
            Assert.All(gameMessage.Game.Photo, p => Assert.True(p.FileSize > 1_000));
            Assert.All(gameMessage.Game.Photo, p => Assert.True(p.Width > 80));
            Assert.All(gameMessage.Game.Photo, p => Assert.True(p.Height > 40));
        }

        [OrderedFact("Should get game high score")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetGameHighScores)]
        public async Task Should_Get_High_Scores()
        {
            GameHighScore[] highScores = await BotClient.GetGameHighScoresAsync(
                userId: _classFixture.Player.Id,
                chatId: _fixture.SupergroupChat.Id,
                messageId: _classFixture.GameMessage.MessageId
            );

            Assert.All(highScores, hs => Assert.True(hs.Position > 0));
            Assert.All(highScores, hs => Assert.True(hs.Score > 0));
            Assert.All(highScores.Select(hs => hs.User), Assert.NotNull);

            _classFixture.HighScores = highScores;
        }

        [OrderedFact("Should set game score")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetGameScore)]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetGameHighScores)]
        public async Task Should_Set_Game_Score()
        {
            int playerId = _classFixture.Player.Id;

            bool playerAlreadyHasScore = _classFixture.HighScores
                .Any(highScore => highScore.User.Id == playerId);

            int oldScore = playerAlreadyHasScore
                ? _classFixture.HighScores.Single(highScore => highScore.User.Id == playerId).Score
                : 0;

            int newScore = oldScore + 1 + new Random().Next(3);

            await _fixture.SendTestInstructionsAsync(
                $"Changing score from {oldScore} to {newScore} for " +
                $"{_classFixture.Player.Username!.Replace("_", @"\_")}."
            );

            Message gameMessage = await BotClient.SetGameScoreAsync(
                userId: playerId,
                score: newScore,
                chatId: _fixture.SupergroupChat.Id,
                messageId: _classFixture.GameMessage.MessageId
            );

            Assert.Equal(_classFixture.GameMessage.MessageId, gameMessage.MessageId);

            // update the high scores cache
            await Task.Delay(TimeSpan.FromSeconds(1));
            _classFixture.HighScores = await BotClient.GetGameHighScoresAsync(
                userId: playerId,
                chatId: _fixture.SupergroupChat.Id,
                messageId: gameMessage.MessageId
            );
        }

        [OrderedFact("Should deduct game score by setting it forcefully")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetGameScore)]
        public async Task Should_Deduct_Game_Score()
        {
            int playerId = _classFixture.Player.Id;
            int oldScore = _classFixture.HighScores.Single(highScore => highScore.User.Id == playerId).Score;
            int newScore = oldScore - 1;

            await _fixture.SendTestInstructionsAsync(
                $"Changing score from {oldScore} to {newScore} for " +
                $"{_classFixture.Player.Username!.Replace("_", @"\_")}."
            );

            Message gameMessage = await BotClient.SetGameScoreAsync(
                userId: playerId,
                score: newScore,
                chatId: _fixture.SupergroupChat.Id,
                messageId: _classFixture.GameMessage.MessageId,
                force: true
            );

            Assert.Equal(MessageType.Game, gameMessage.Type);
        }
    }
}
