using System.Collections.Generic;

namespace TradingCardGame.Domain
{
    public interface IDeckShuffler
    {
        IEnumerable<byte> Shuffle(IEnumerable<byte> cards);
    }
}