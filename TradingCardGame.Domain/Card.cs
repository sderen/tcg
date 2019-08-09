using System;

namespace TradingCardGame.Domain
{
    public class Card
    {
        private const byte MaxManaCost = 8;
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