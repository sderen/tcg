using System;
using Xunit;

namespace TradingCardGame.Domain.UnitTest
{
    public class CardTests
    {
        [Fact]
        public void Test_Card_Constructor_Throws_ArgumentOutOfRangeException_If_ManaCost_Is_Greater_Than_Ten()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Card(9));
        }

        [Fact]
        public void Test_Card_Is_Initialized_Properly_With_Valid_Mana_Cost()
        {
            for (byte i = 0; i <= 8; i++)
            {
                var card = new Card(i);
                Assert.Equal(i, card.ManaCost);
            }
        }
    }
}