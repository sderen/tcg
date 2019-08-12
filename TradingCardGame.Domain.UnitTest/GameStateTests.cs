using Xunit;

namespace TradingCardGame.Domain.UnitTest
{
    public class GameStateTests
    {
        [Fact]
        public void Test_GameState_Is_Initialized_Properly()
        {
            var gs = new GameState(new byte[]{2, 4}, 3, 29, 8, 4, true, 14, 13);
            Assert.Equal(new byte[]{2, 4}, gs.Hand);
            Assert.Equal(3, gs.Tick);
            Assert.Equal(29, gs.Health);
            Assert.Equal(8, gs.OpponentHealth);
            Assert.Equal(4, gs.OpponentHandCount);
            Assert.True(gs.IsYourTurn);
            Assert.Equal(14, gs.DeckSize);
            Assert.Equal(13, gs.OpponentDeckSize);
        }
    }
}