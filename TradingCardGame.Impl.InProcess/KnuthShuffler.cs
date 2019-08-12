using System;
using System.Collections.Generic;
using System.Linq;
using TradingCardGame.Domain;

namespace TradingCardGame.Impl.InProcess
{
    public class KnuthShuffler : IDeckShuffler
    {
        public IEnumerable<byte> Shuffle(IEnumerable<byte> cards)
        {
            var cardsArr = cards.ToArray();
            var random = new Random();
            for (int i = 0; i < cardsArr.Length - 1; i++)
            {
                int j = random.Next(i, cardsArr.Length);
                byte val = cardsArr[i];
                cardsArr[i] = cardsArr[j];
                cardsArr[j] = val;
            }

            return cardsArr;
        }
    }
}