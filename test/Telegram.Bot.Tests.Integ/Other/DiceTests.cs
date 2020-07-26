using System.Threading.Tasks;
using Telegram.Bot.Tests.Integ.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Tests.Integ.Other
{
    [Collection(Constants.TestCollections.Dice)]
    [TestCaseOrderer(Constants.TestCaseOrderer, Constants.AssemblyName)]
    public class DiceTests
    {
        private readonly TestsFixture _testsFixture;

        public DiceTests(TestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [OrderedFact("Should send a die")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendDice)]
        public async Task Should_Send_A_Die()
        {
            Message message = await _testsFixture.BotClient.SendDiceAsync(chatId: _testsFixture.SupergroupChat);

            Assert.Equal(MessageType.Dice, message.Type);
            Assert.NotNull(message.Dice);
            Assert.Equal("🎲", message.Dice.Emoji);
            Assert.InRange(message.Dice.Value, 1, 6);
        }

        [OrderedFact("Should send a dart")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendDice)]
        public async Task Should_Send_A_Dart()
        {
            Message message = await _testsFixture.BotClient.SendDiceAsync(
                chatId: _testsFixture.SupergroupChat,
                emoji: Emoji.Darts
            );

            Assert.Equal(MessageType.Dice, message.Type);
            Assert.NotNull(message.Dice);
            Assert.Equal("🎯", message.Dice.Emoji);
            Assert.InRange(message.Dice.Value, 1, 6);
        }

        [OrderedFact("Should send a basketball")]
        [Trait(Constants.MethodTraitName, Constants.TelegramBotApiMethods.SendDice)]
        public async Task Should_Send_A_Basketball()
        {
            Message message = await _testsFixture.BotClient.SendDiceAsync(
                chatId: _testsFixture.SupergroupChat,
                emoji: Emoji.Basketball
            );

            Assert.Equal(MessageType.Dice, message.Type);
            Assert.NotNull(message.Dice);
            Assert.Equal("🏀", message.Dice.Emoji);
            Assert.InRange(message.Dice.Value, 1, 5);
        }
    }
}
