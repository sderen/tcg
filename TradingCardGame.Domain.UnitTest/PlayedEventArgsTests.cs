using System;
using System.Text;
using Xunit;

namespace TradingCardGame.Domain.UnitTest
{
    public class PlayedEventArgsTests
    {
        [Fact]
        public void Test_PlayedEventArgs_Is_Initialized_Correctly_For_EndedTurn()
        {
            var pea = new PlayedEventArgs(1, PlayerActionType.EndedTurn);
            Assert.Equal(PlayerActionType.EndedTurn, pea.ActionType);
            Assert.Equal(-1, pea.CardIndex);
            Assert.Equal(-1, pea.ManaCost);
            Assert.Equal(1, pea.Tick);
        }

        [Fact]
        public void
            Test_PlayedEventArg_Throws_ArgumentOutOfRangeException_Is_ActionType_Is_PlayedCard_And_Index_Is_Not_Set()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PlayedEventArgs(1, PlayerActionType.PlayedCard));
        }
        
        [Fact]
        public void
            Test_PlayedEventArg_Throws_ArgumentOutOfRangeException_Is_ActionType_Is_PlayedCard_And_ManaCost_Is_Not_Set()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PlayedEventArgs(1, PlayerActionType.PlayedCard, 1));
        }

        [Fact]
        public void Test_PlayedEventArg_Is_Initialized_Correctly_For_PlayedCard()
        {
            var pea = new PlayedEventArgs(4, PlayerActionType.PlayedCard, 2, 3);
            Assert.Equal(PlayerActionType.PlayedCard, pea.ActionType);
            Assert.Equal(2, pea.CardIndex);
            Assert.Equal(3, pea.ManaCost);
            Assert.Equal(4, pea.Tick);
        }
    }
}