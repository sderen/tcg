using System;
using System.Threading.Tasks;
using TradingCardGame.Domain;

namespace TradingCardGame.Impl.InProcess
{
    public class InProcessPlayerCommunicator : IPlayerCommunicator
    {        
        public event EventHandler<PlayedEventArgs> ActionPerformed;
        
        public PlayerState RelevantPlayerState { get; private set; }

        public GameState LatestGameState { get; private set; }

        public string PlayerName { get; private set; }

        public InProcessPlayerCommunicator(string playerName)
        {
            PlayerName = playerName;
        }
        
        public async Task CommunicateWithPlayerAsync(CommunicationPackage communicationPackage)
        {
            Console.WriteLine($"{PlayerName}: {communicationPackage}");
            if (communicationPackage.GameEventType == GameEventType.State)
            {
                LatestGameState = communicationPackage.GameState;
            }
        }

        public void SetPlayerState(PlayerState playerState)
        {
            RelevantPlayerState = playerState;
        }

        public void PerformPlayerAction(PlayedEventArgs args)
        {
            ActionPerformed?.Invoke(this, args);
        }
    }
}