using System;
using System.Threading.Tasks;

namespace TradingCardGame.Domain
{
    public interface IPlayerCommunicator
    {
        event EventHandler<PlayedEventArgs> ActionPerformed;

        Task CommunicateWithPlayerAsync(CommunicationPackage communicationPackage);

        void SetPlayerState(PlayerState playerState);
        
        PlayerState RelevantPlayerState { get; }
    }
}