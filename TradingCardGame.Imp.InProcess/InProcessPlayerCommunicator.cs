using System;
using System.Threading.Tasks;
using TradingCardGame.Domain;

namespace TradingCardGame.Impl.InProcess
{
    public class InProcessPlayerCommunicator : IPlayerCommunicator
    {
        public event EventHandler<PlayedEventArgs> ActionPerformed;
        public async Task CommunicateWithPlayerAsync(CommunicationPackage communicationPackage)
        {
            Console.WriteLine(communicationPackage);
        }

        public void SetPlayerState(PlayerState playerState)
        {
            RelevantPlayerState = playerState;
        }

        public PlayerState RelevantPlayerState { get; private set; }
        public void PerformPlayerAction(PlayedEventArgs args)
        {
            ActionPerformed?.Invoke(this, args);
        }
    }
}