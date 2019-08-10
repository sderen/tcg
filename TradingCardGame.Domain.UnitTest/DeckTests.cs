using System.Collections.Generic;
using Xunit;

namespace TradingCardGame.Domain.UnitTest
{
    public class DeckTests
    {
        [Fact]
        public void Test_Deck_Is_Initialized_Properly()
        {
            var deck = new Deck();
            Assert.Equal(20, deck.CardCount);
        }

        [Fact]
        public void Test_Deck_Returns_All_Cards_With_Given_Mana_Costs_And_Null_Afterwards()
        {
            var deck = new Deck();

            var costs = new List<byte>
            {
                0, 0, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 7, 8
            };

            for (int i = 0; i < 20; i++)
            {
                var card = deck.DrawCard();
                Assert.NotNull(card);
                Assert.Equal(20 - i - 1, deck.CardCount);
                costs.Remove(card.Value);
            }
            Assert.Empty(costs);
            Assert.Null(deck.DrawCard());
            Assert.Equal(0, deck.CardCount);
        }
    }
}