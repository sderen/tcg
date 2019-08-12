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
        private const int InitialDrawSize = 3;
        

        public bool IsActive { get; private set; }
        public byte TotalManaSlots { get; private set; }
        public byte AvailableManaSlots { get; private set; }
        public byte Health { get; private set; }

        public int DeckCardCount => _deck.CardCount;

        public IReadOnlyCollection<byte> Hand => _hand.CardsInHand;

        private readonly Deck _deck;
        private readonly Hand _hand;


        public PlayerState(IDeckShuffler deckShuffler)
        {
            Health = MaxHealth;

            _deck = new Deck(deckShuffler);
            _hand = new Hand();
            

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
            
            if (TotalManaSlots < MaxManaSlots)
            {
                TotalManaSlots++;
            }

            AvailableManaSlots = TotalManaSlots;
            
            DrawCard();
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void PlayCard(int cardIndex)
        {
            if (Hand.Count < cardIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(cardIndex));
            }

            var card = Hand.ElementAt(cardIndex);
            if (card > AvailableManaSlots)
            {
                throw new InvalidOperationException("Cannot play this card with current available mana slots.");
            }
            _hand.RemoveCardAtIndex(cardIndex);
            AvailableManaSlots -= card;
        }
        
        //TODO: think about multithreading in general
        private void DrawCard()
        {
            var card = _deck.DrawCard();
            if (card == null)
            {
                DamagePlayer(1);
                return;
            }

            _hand.AddCardToHand(card.Value); //TODO: do i need return value anywhere 
        }

    }
}