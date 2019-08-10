using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;

namespace TradingCardGame.Domain
{
    public class Hand
    {
        private const int MaxHandSize = 5;
        
        private readonly object _lock = new object();

        private LinkedList<byte> _hand;

        public IReadOnlyCollection<byte> CardsInHand => new ReadOnlyCollection<byte>(_hand.ToList()); //TODO improve it

        
        public Hand()
        {
            _hand = new LinkedList<byte>();
        }

        public bool AddCardToHand(byte card)
        {
            if (_hand.Count < MaxHandSize)
            {
                lock (_lock)
                {
                    if (_hand.Count < MaxHandSize)
                    {
                        _hand.AddLast(card);
                        return true;
                    }
                }
            }

            return false;
        }

        public void RemoveCardAtIndex(int handIndex)
        {
            if (handIndex > _hand.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(handIndex));
            }
            
            LinkedListNode<byte> node = _hand.First; //could get last if idx > 2, too little gain for it though
            lock (_lock)
            {
                for (int i = 0; i < handIndex; i++)
                {
                    node = node.Next;
                }

                _hand.Remove(node);
            }
        }

    }
}