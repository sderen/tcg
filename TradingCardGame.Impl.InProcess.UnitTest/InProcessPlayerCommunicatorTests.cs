using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using TradingCardGame.Domain;
using Xunit;

namespace TradingCardGame.Impl.InProcess.UnitTest
{
    public class InProcessPlayerCommunicatorTests
    {
        [Fact]
        public void Test_InProcessPlayerCommunicator_Initialized_Correctly()
        {
            var ipc = new InProcessPlayerCommunicator("name");
            Assert.Equal("name", ipc.PlayerName);
        }

        [Fact]
        public async Task Test_InProcessPlayerCommunicator_Shows_Proper_Latest_State()
        {
            var ipc = new InProcessPlayerCommunicator("name");
            var gs = new GameState(new byte[]{2, 4}, 3, 29, 8, 4, true, 14, 13);

            await ipc.CommunicateWithPlayerAsync(new CommunicationPackage(gs));
            
            Assert.Same(gs, ipc.LatestGameState);
            
            gs = new GameState(new byte[]{2, 4}, 4, 29, 8, 4, false, 14, 13);
            await ipc.CommunicateWithPlayerAsync(new CommunicationPackage(gs));
            
            Assert.Same(gs, ipc.LatestGameState);
        }

        [Fact]
        public async Task InProcessPlayerCommunicator_Performs_Player_Action_Properly()
        {
            var ipc = new InProcessPlayerCommunicator("name");
            bool received = false;
            ipc.ActionPerformed += (sender, args) => received = true;
            
            ipc.PerformPlayerAction(new PlayedEventArgs(3, PlayerActionType.EndedTurn));
            
            Assert.True(received);
        }
    }
}