using System;

namespace TradingCardGame.Domain
{
    public class Player
    {   
        public PlayerState State { get; }
        public IPlayerCommunicator PlayerCommunicator { get; }
        
        public Player(IPlayerCommunicator playerCommunicator, IDeckShuffler deckShuffler)
        {
            PlayerCommunicator = playerCommunicator ?? throw new ArgumentNullException(nameof(playerCommunicator));
            
            State = new PlayerState(deckShuffler);
            
            PlayerCommunicator.SetPlayerState(State);
        }
    }
}