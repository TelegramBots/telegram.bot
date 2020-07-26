using System;
using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Polls
{
    [Collection(Constants.TestCollections.NativePolls)]
    [Trait(Constants.CategoryTraitName, Constants.InteractiveCategoryValue)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class PublicPollTests : IClassFixture<PublicPollTestsFixture>
    {
        private readonly PublicPollTestsFixture _classFixture;
        private TestsFixture Fixture => _classFixture.TestsFixture;
        private ITelegramBotClient BotClient => Fixture.BotClient;

        public PublicPollTests(PublicPollTestsFixture classFixture)
        {
            _classFixture = classFixture;
        }

        [OrderedFact("Should send public poll with multiple answers")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendPoll)]
        public async Task Should_Send_Non_Anonymous_Poll_With_Multiple_Answers()
        {
            Message message = await Fixture.BotClient.SendPollAsync(
                chatId: Fixture.SupergroupChat,
                question: "Pick your team",
                options: new [] { "Aragorn", "Galadriel", "Frodo" },
                isAnonymous: false,
                type: PollType.Regular,
                allowsMultipleAnswers: true
            );

            Assert.Equal(MessageType.Poll, message.Type);
            Assert.NotEmpty(message.Poll!.Id);
            Assert.False(message.Poll.IsClosed);
            Assert.False(message.Poll.IsAnonymous);
            Assert.Equal("regular", message.Poll.Type);
            Assert.True(message.Poll.AllowsMultipleAnswers);
            Assert.Null(message.Poll.CorrectOptionId);
            Assert.Null(message.Poll.OpenPeriod);
            Assert.Null(message.Poll.CloseDate);

            Assert.Equal("Pick your team", message.Poll.Question);
            Assert.Equal(3, message.Poll.Options.Length);
            Assert.Equal("Aragorn", message.Poll.Options[0].Text);
            Assert.Equal("Galadriel", message.Poll.Options[1].Text);
            Assert.Equal("Frodo", message.Poll.Options[2].Text);
            Assert.All(message.Poll.Options, option => Assert.Equal(0, option.VoterCount));

            _classFixture.OriginalPollMessage = message;
        }

        [OrderedFact("Should receive a poll answer update")]
        public async Task Should_Receive_Poll_Answer_Update()
        {
            await Fixture.SendTestInstructionsAsync(
                "🗳 Vote for more than one option on the poll above 👆"
            );

            Update pollAnswerUpdate = await Fixture.UpdateReceiver.GetUpdateAsync(
                update => update.PollAnswer!.OptionIds.Length > 1,
                updateTypes: UpdateType.PollAnswer
            );

            Poll poll = _classFixture.OriginalPollMessage.Poll;
            PollAnswer pollAnswer = pollAnswerUpdate.PollAnswer;

            Assert.Equal(poll!.Id, pollAnswer!.PollId);
            Assert.NotNull(pollAnswer.User);
            Assert.All(
                pollAnswer.OptionIds,
                optionId => Assert.True(optionId < poll.Options.Length)
            );

            _classFixture.PollAnswer = pollAnswer;
        }

        [OrderedFact("Should stop non-anonymous the poll")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.StopPoll)]
        public async Task Should_Stop_Non_Anonymous_Poll()
        {
            // don't close poll immediately, without a delay the resulting poll object
            // doesn't match up with the previously received poll answer
            await Task.Delay(TimeSpan.FromSeconds(5));

            Poll closedPoll = await BotClient.StopPollAsync(
                chatId: _classFixture.OriginalPollMessage.Chat!,
                messageId: _classFixture.OriginalPollMessage.MessageId
            );

            Assert.Equal(_classFixture.OriginalPollMessage.Poll!.Id, closedPoll.Id);
            Assert.True(closedPoll.IsClosed);

            PollAnswer pollAnswer = _classFixture.PollAnswer;

            Assert.All(
                pollAnswer.OptionIds,
                optionId => Assert.True(closedPoll.Options[optionId].VoterCount > 0)
            );
        }
    }
}
