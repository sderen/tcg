using System.Collections.Generic;

namespace TradingCardGame.Domain.UnitTest
{
    public class NonShuffler : IDeckShuffler
    {
        public IEnumerable<byte> Shuffle(IEnumerable<byte> cards)
        {
            return cards;
        }
    }
}