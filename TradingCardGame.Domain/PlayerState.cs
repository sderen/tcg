using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;

namespace TradingCardGame.Domain
{
    public class PlayerState
    {
        private const byte MaxHealth = 30;
        private const byte MaxManaSlots = 10;
        private const int MaxHandSize = 5;
        private const int InitialDrawSize = 3;
        private const byte EmptyHandSlot = 11;
        
        private static readonly byte[] CardCosts = new byte[] {0, 0, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 5, 5, 6, 6, 7, 8};
        private static readonly int[] EmptyPositions = new int[] {4, 3, 2, 1, 0};

        public bool IsActive { get; private set; }
        public byte TotalManaSlots { get; private set; }
        public byte AvailableManaSlots { get; private set; }
        public byte Health { get; private set; }

        public IReadOnlyCollection<byte> Hand => new ReadOnlyCollection<byte>(_hand); //TODO improve it, do not return empty card slots

        private byte[] _hand; //TODO: improve whole hand management
        private Stack<byte> _deck;
        private Stack<int> _emptyPositionsInHand;


        public PlayerState()
        {
            Health = MaxHealth;

            _deck = KnuthShuffleNewDeck();
            _emptyPositionsInHand = new Stack<int>(EmptyPositions);
            _hand = new byte[MaxHandSize];
            for (int i = 0; i < _hand.Length; i++)
            {
                _hand[i] = EmptyHandSlot;
            }

            for (int i = 0; i < InitialDrawSize; i++)
            {
                DrawCard();
            }
        }

        public void DamagePlayer(byte amount)
        {
            if (amount >= Health)
            {
                Health = 0;
            }
            else
            {
                Health -= amount;    
            }
        }

        public bool IsDead()
        {
            return Health <= 0;
        }

        public void Activate()
        {
            if (IsActive)
            {
                return;
            }

            IsActive = true;
            
            //TODO: draw
            if (TotalManaSlots < MaxManaSlots)
            {
                TotalManaSlots++;
            }

            AvailableManaSlots = TotalManaSlots;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        //TODO: think about multithreading in general
        private void DrawCard()
        {
            if (_deck.Count == 0)
            {
                DamagePlayer(1);
                return;
            }

            var card = _deck.Pop();

            if (_emptyPositionsInHand.Count== 0)
            {
                //discard the card
                return;
            }

            _hand[_emptyPositionsInHand.Pop()] = card;
        }

        private Stack<byte> KnuthShuffleNewDeck()
        {
            //TODO: apply knuth
            var random = new Random();
            return new Stack<byte>(CardCosts.OrderBy(e => random.Next()));
        }
    }
}