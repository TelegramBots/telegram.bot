using System;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Tests.Unit
{
    public class ChatIdTests
    {
        [Theory]
        [InlineData("@username")]
        [InlineData("@UserName")]
        [InlineData("@User1")]
        [InlineData("@12345")]
        [InlineData("12345")]
        [InlineData("0")]
        [InlineData("999999999999999")]
        [InlineData("@99999999999999999999999999999999")]
        public void Valid_User_Name(string userName)
        {
            var chatId = new ChatId(userName);

            Assert.Equal(chatId, userName);
        }

        [Fact]
        public void Null_User_Name()
        {
            Assert.Throws<ArgumentNullException>(() => new ChatId(null));
        }


        [Theory]
        [InlineData("username")]
        [InlineData("@u")]
        [InlineData("@User")]
        [InlineData("@1234")]
        [InlineData("999999999999999999999999")]
        [InlineData("@999999999999999999999999999999999")]
        public void Invalid_User_Name(string userName)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ChatId(userName));
        }
    }
}
