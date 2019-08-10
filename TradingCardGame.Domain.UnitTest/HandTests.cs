using System;
using System.Linq;
using Xunit;

namespace TradingCardGame.Domain.UnitTest
{
    public class HandTests
    {
        [Fact]
        public void Test_Hand_Is_Initialized_Properly()
        {
            var hand = new Hand();
            Assert.Empty(hand.CardsInHand);
        }

        [Fact]
        public void Test_Hand_Can_Grow_Up_To_Five_Cards_And_Returns_Right_Card_List_Back()
        {
            var hand = new Hand();
            for (byte i = 0; i < 5; i++)
            {
                Assert.True(hand.AddCardToHand(i));
                var cards = hand.CardsInHand.ToArray();
                Assert.Equal(i + 1, cards.Length);
                for (byte j = 0; j <= i; j++)
                {
                    Assert.Equal(j, j);
                }
            }
            
            Assert.False(hand.AddCardToHand(99));
            Assert.Equal(5, hand.CardsInHand.Count);
            Assert.False(hand.CardsInHand.Contains((byte) 99));
        }

        [Fact]
        public void Test_RemoveCardAtIndex_Throws_ArgumentOutOfRangeException_For_Invalid_Index()
        {
            var hand = new Hand();
            Assert.Throws<ArgumentOutOfRangeException>(() => hand.RemoveCardAtIndex(1));
        }

        [Fact]
        public void Test_RemoveCardAtIndex_Removes_Right_Card_From_Hand()
        {
            var hand = new Hand();
            hand.AddCardToHand(1);
            hand.AddCardToHand(3);
            hand.RemoveCardAtIndex(0);
            Assert.Single(hand.CardsInHand);
            Assert.Equal(3, hand.CardsInHand.First());
        }
    }
}