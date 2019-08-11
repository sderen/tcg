using System;

namespace TradingCardGame.Domain
{
    public class Card
    {
        private const byte MaxManaCost = 8;

        // ManaCost would be enough and in fact I implemented with only mana cost number as card at first.
        //  However having Id for a card simplifies race conditions that may happen over multiple network commands
        //  and removes the need to keep a game tick counter for synchronization.
        public string Id { get; set; }
        public byte ManaCost { get; }

        public Card(byte manaCost)
        {
            if (manaCost > MaxManaCost)
            {
                throw new ArgumentOutOfRangeException(nameof(manaCost));
            }
            ManaCost = manaCost;
        }
    }
}