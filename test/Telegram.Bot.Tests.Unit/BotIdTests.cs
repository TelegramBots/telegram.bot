using System;
using Xunit;

namespace Telegram.Bot.Tests.Unit
{
    public class BotIdTests
    {
        [Theory]
        [InlineData("1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy", 1234567)]
        [InlineData("9:jdsaghdfilghdfiugherh", 9)]
        [InlineData("0:foo", 0)]
        [InlineData("5:", 5)]
        [InlineData("-123::::", -123)]
        public void Should_Parse_Bot_Id(string token, int expectedId)
        {
            ITelegramBotClient botClient = new TelegramBotClient(token);
            Assert.Equal(expectedId, botClient.BotId);
        }

        [Fact]
        public void Should_Throw_On_Null_Token()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                () => new TelegramBotClient(null!)
            );
            Assert.Equal("token", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(":")]
        [InlineData("1234567")]
        [InlineData("INVALID:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy")]
        public void Should_Throw_On_Invalid_Token(string invalidToken)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(
                () => new TelegramBotClient(invalidToken)
            );
            Assert.Equal("token", exception.ParamName);
        }
    }
}
