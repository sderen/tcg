using System;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Xunit;

namespace TradingCardGame.Domain.UnitTest
{
    public class GameTests
    {
        [Fact]
        public async Task Test_Game_Works()
        {
            var c1 = new ConsoleOutputProvider();
            var c2 = new ConsoleOutputProvider();
            
            var p1 = new Player(c1, new NonShuffler());
            var p2 = new Player(c2, new NonShuffler());
         
            var game = new Game(p1, p2);
            await game.StartGameAsync();

            Assert.NotEqual(p1.State.IsActive, p2.State.IsActive);

            var active = p1.State.IsActive ? p1 : p2;
            var deactive = p1.State.IsActive ? p2 : p1;

        }
        
        private class ConsoleOutputProvider : IPlayerCommunicator
        {
            public event EventHandler<PlayedEventArgs> ActionPerformed;

            public void SetPlayerState(PlayerState playerState)
            {
                RelevantPlayerState = playerState;
            }

            public PlayerState RelevantPlayerState { get; private set; }

            public async Task CommunicateWithPlayerAsync(CommunicationPackage communicationPackage)
            {
                Console.WriteLine(communicationPackage);
            }

            
        }
    }
    
}