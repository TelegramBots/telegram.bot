using System;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Polls
{
    [Collection(Constants.TestCollections.NativePolls)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class SelfStoppingPollTests : IClassFixture<SelfStoppingPollTestsFixture>
    {
        private readonly SelfStoppingPollTestsFixture _classFixture;
        private TestsFixture Fixture => _classFixture.TestsFixture;
        private ITelegramBotClient BotClient => Fixture.BotClient;

        public SelfStoppingPollTests(SelfStoppingPollTestsFixture fixture)
        {
            _classFixture = fixture;
        }

        [OrderedFact("Should send self closing anonymous poll by period")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendPoll)]
        public async Task Should_Send_Self_Closing_Poll_Anonymous_Poll_By_Period()
        {
            Message message = await BotClient.SendPollAsync(
                chatId: Fixture.SupergroupChat,
                question: "Who shot first?",
                options: new [] { "Han Solo", "Greedo", "I don't care" },
                openPeriod: 10
            );

            Assert.Equal(MessageType.Poll, message.Type);
            Assert.NotEmpty(message.Poll!.Id);
            Assert.False(message.Poll.IsClosed);
            Assert.True(message.Poll.IsAnonymous);
            Assert.Equal("regular", message.Poll.Type);
            Assert.False(message.Poll.AllowsMultipleAnswers);
            Assert.Null(message.Poll.CorrectOptionId);
            Assert.Equal(10, message.Poll.OpenPeriod);
            Assert.NotNull(message.Poll.CloseDate);

            Assert.Equal("Who shot first?", message.Poll.Question);
            Assert.Equal(3, message.Poll.Options.Length);
            Assert.Equal("Han Solo", message.Poll.Options[0].Text);
            Assert.Equal("Greedo", message.Poll.Options[1].Text);
            Assert.Equal("I don't care", message.Poll.Options[2].Text);
            Assert.All(message.Poll.Options, option => Assert.Equal(0, option.VoterCount));

            _classFixture.PollMessage = message;
            _classFixture.OpenPeriod = 10;
        }

        [OrderedFact(
            "Should receive closed poll state update by period",
            Skip = "For some reason Telegram doesn't send poll update when a poll closes itself")]
        public async Task Should_Receive_Closed_Poll_State_Update_By_Period()
        {
            string pollId = _classFixture.PollMessage.Poll!.Id;

            await Fixture.SendTestInstructionsAsync("Wait a few seconds until the poll automatically closes");
            Update update = await Fixture.UpdateReceiver.GetUpdateAsync(updateTypes: UpdateType.Poll);

            Assert.Equal(UpdateType.Poll, update.Type);
            Assert.Equal(pollId, update.Poll!.Id);
            Assert.Equal(_classFixture.OpenPeriod, update.Poll.OpenPeriod);
            Assert.Null(update.Poll.CloseDate);
            Assert.True(update.Poll.IsClosed);
        }

        [OrderedFact("Should send self closing anonymous poll by date")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendPoll)]
        public async Task Should_Send_Self_Closing_Poll_Anonymous_Poll_By_Date()
        {
            DateTime closeDate = DateTime.UtcNow.AddSeconds(30);

            Message message = await BotClient.SendPollAsync(
                chatId: Fixture.SupergroupChat,
                question: "Who shot first?",
                options: new [] { "Han Solo", "Greedo", "I don't care" },
                closeDate: closeDate
            );

            Assert.Equal(MessageType.Poll, message.Type);
            Assert.NotEmpty(message.Poll!.Id);
            Assert.False(message.Poll.IsClosed);
            Assert.True(message.Poll.IsAnonymous);
            Assert.Equal("regular", message.Poll.Type);
            Assert.False(message.Poll.AllowsMultipleAnswers);
            Assert.Null(message.Poll.CorrectOptionId);
            Assert.NotNull(message.Poll.OpenPeriod);
            Assert.NotNull(message.Poll.CloseDate);
            Assert.InRange(
                message.Poll.CloseDate.Value,
                closeDate.AddSeconds(-2),
                closeDate.AddSeconds(2)
            );
            Assert.Equal("Who shot first?", message.Poll.Question);
            Assert.Equal(3, message.Poll.Options.Length);
            Assert.Equal("Han Solo", message.Poll.Options[0].Text);
            Assert.Equal("Greedo", message.Poll.Options[1].Text);
            Assert.Equal("I don't care", message.Poll.Options[2].Text);
            Assert.All(message.Poll.Options, option => Assert.Equal(0, option.VoterCount));

            _classFixture.PollMessage = message;
            _classFixture.CloseDate = closeDate;
        }

        [OrderedFact(
            "Should receive closed poll state update by date",
            Skip = "For some reason Telegram doesn't send poll update when a poll closes itself")]
        public async Task Should_Receive_Closed_Poll_State_Update_By_Date()
        {
            string pollId = _classFixture.PollMessage.Poll!.Id;

            await Fixture.SendTestInstructionsAsync("Wait a few seconds until the poll automatically closes");
            Update update = await Fixture.UpdateReceiver.GetUpdateAsync(updateTypes: UpdateType.Poll);

            Assert.Equal(UpdateType.Poll, update.Type);
            Assert.Equal(pollId, update.Poll!.Id);
            Assert.Null(update.Poll.OpenPeriod);
            Assert.NotNull(update.Poll.CloseDate);
            Assert.InRange(
                update.Poll.CloseDate.Value,
                _classFixture.CloseDate.AddSeconds(-2),
                _classFixture.CloseDate.AddSeconds(2)
            );
            Assert.True(update.Poll.IsClosed);
        }
    }
}
