using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Games
{
    [Collection(Constants.TestCollections.Games)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class GamesTests : IClassFixture<GamesFixture>
    {
        private ITelegramBotClient BotClient => _fixture.BotClient;

        private readonly TestsFixture _fixture;

        private readonly GamesFixture _classFixture;

        public GamesTests(TestsFixture fixture, GamesFixture classFixture)
        {
            _fixture = fixture;
            _classFixture = classFixture;
        }

        [OrderedFact("Should answer inline query with a game")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerInlineQuery)]
        public async Task Should_Answer_InlineQuery_With_Game()
        {
            await _fixture.SendTestInstructionsAsync(
                "Staring the inline query with this message...",
                startInlineQuery: true
            );

            Update queryUpdate = await _fixture.UpdateReceiver.GetInlineQueryUpdateAsync();

            string resultId = "game";
            await BotClient.AnswerInlineQueryAsync(
                inlineQueryId: queryUpdate.InlineQuery!.Id,
                results: new InlineQueryResultBase[]
                {
                    new InlineQueryResultGame(
                        id: resultId,
                        gameShortName: _classFixture.GameShortName
                    )
                },
                cacheTime: 0
            );

            (Update messageUpdate, Update chosenResultUpdate) =
                await _fixture.UpdateReceiver.GetInlineQueryResultUpdates(MessageType.Game);

            Assert.Equal(MessageType.Game, messageUpdate.Message!.Type);
            Assert.Equal(resultId, chosenResultUpdate.ChosenInlineResult!.ResultId);
            Assert.Empty(chosenResultUpdate.ChosenInlineResult.Query);

            _classFixture.InlineGameMessageId = chosenResultUpdate.ChosenInlineResult.InlineMessageId;
        }

        [OrderedFact("Should get game high score for inline message")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.GetGameHighScores)]
        public async Task Should_Get_High_Scores_Inline_Message()
        {
            GameHighScore[] highScores = await BotClient.GetGameHighScoresAsync(
                userId: _classFixture.Player.Id,
                inlineMessageId: _classFixture.InlineGameMessageId
            );

            Assert.All(highScores, _ => Assert.True(_.Position > 0));
            Assert.All(highScores, _ => Assert.True(_.Score > 0));
            Assert.All(highScores.Select(_ => _.User), Assert.NotNull);

            _classFixture.HighScores = highScores;
        }

        [OrderedFact("Should set game score for inline message")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SetGameScore)]
        public async Task Should_Set_Game_Score_Inline_Message()
        {
            int playerId = _classFixture.Player.Id;
            int oldScore = _classFixture.HighScores.Single(highScore => highScore.User.Id == playerId).Score;
            int newScore = oldScore + 1 + new Random().Next(3);

            await _fixture.SendTestInstructionsAsync(
                $"Changing score from {oldScore} to {newScore} for " +
                $"{_classFixture.Player.Username!.Replace("_", @"\_")}."
            );

            await BotClient.SetGameScoreAsync(
                userId: playerId,
                score: newScore,
                inlineMessageId: _classFixture.InlineGameMessageId
            );
        }

        [OrderedFact("Should answer game callback query")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.AnswerCallbackQuery)]
        public async Task Should_Answer_CallbackQuery_With_Game_Url()
        {
            await _fixture.SendTestInstructionsAsync(
                "Click on any Play button on any of the game messages above 👆"
            );

            Update cqUpdate = await _fixture.UpdateReceiver.GetCallbackQueryUpdateAsync();

            Assert.True(cqUpdate.CallbackQuery!.IsGameQuery);

            await BotClient.AnswerCallbackQueryAsync(
                callbackQueryId: cqUpdate.CallbackQuery.Id,
                url: "https://tbot.xyz/lumber/"
            );
        }
    }
}
