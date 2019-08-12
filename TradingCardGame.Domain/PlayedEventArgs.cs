using System;

namespace TradingCardGame.Domain
{
    public class PlayedEventArgs : EventArgs
    {
        public int Tick { get; }
        public PlayerActionType ActionType { get; }
        public int CardIndex { get; }
        public int ManaCost { get; }

        //having mana cost here has only one purpose, removing the race condition on multiple actions received 
        public PlayedEventArgs(int tick, PlayerActionType playerActionType, int cardIndex = -1, int manaCost = -1)
        {
            if (playerActionType == PlayerActionType.PlayedCard)
            {
                if (cardIndex < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(cardIndex));    
                }

                if (manaCost < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(manaCost));
                }
                
            }

            Tick = tick;
            ActionType = playerActionType;
            CardIndex = cardIndex;
            ManaCost = manaCost;
        }
    }
}