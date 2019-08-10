using System;
using System.Collections.Generic;
using System.Linq;

namespace TradingCardGame.Domain
{
    public class Deck
    {
        private static readonly byte[] CardCosts = new byte[] {0, 0, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 7, 8};
        
        private readonly object _lock = new object();
        
        private readonly Stack<byte> _cards;

        public int CardCount => _cards.Count;

        public Deck()
        {
            _cards = KnuthShuffleNewDeck();
        }

        public byte? DrawCard()
        {
            if (_cards.Count > 0)
            {
                lock (_lock)
                {
                    if (_cards.Count > 0)
                    {
                        return _cards.Pop();
                    }
                }
            }

            return null;
        }
        
        private Stack<byte> KnuthShuffleNewDeck()
        {
            //TODO: apply knuth
            var random = new Random();
            return new Stack<byte>(CardCosts.OrderBy(e => random.Next()));
        }
    }
}