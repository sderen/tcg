using Xunit;

namespace TradingCardGame.Domain.UnitTest
{
    public class CommunicationPackageTests
    {
        [Fact]
        public void Test_CommunicationPackage_Is_Initialized_Properly_For_GameEventType()
        {
            var cp = new CommunicationPackage(GameEventType.Activated);
            Assert.Equal(GameEventType.Activated, cp.GameEventType);
            Assert.Null(cp.PlayerActionType);
        }

        [Fact]
        public void Test_CommunicationPackage_Is_Initialized_Properly_For_PlayerActionType()
        {
            var cp = new CommunicationPackage(PlayerActionType.PlayedCard);
            Assert.Equal(PlayerActionType.PlayedCard, cp.PlayerActionType);
            Assert.Null(cp.GameEventType);
        }

        [Fact]
        public void Test_CommunicationPackage_Is_Initialized_Properly_For_GameState()
        {
            var gs = new GameState(new byte[]{2, 4}, 3, 29, 8, 4, true, 14, 13);
            var cp = new CommunicationPackage(gs);
            Assert.Equal(GameEventType.State, cp.GameEventType);
            Assert.Equal(gs, cp.GameState);
        }
    }
}