using System;
using System.Collections.Generic;
using System.Linq;

namespace TradingCardGame.Domain.UnitTest
{
    public class SimpleShuffler : IDeckShuffler
    {
        public IEnumerable<byte> Shuffle(IEnumerable<byte> cards)
        {
            var random = new Random();
            return cards.OrderBy(e => random.Next());
        }
    }
}