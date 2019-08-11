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
            var pea = new PlayedEventArgs(PlayerActionType.EndedTurn);
            Assert.Equal(PlayerActionType.EndedTurn, pea.ActionType);
            Assert.Equal(-1, pea.CardIndex);
            Assert.Equal(-1, pea.ManaCost);
        }

        [Fact]
        public void
            Test_PlayedEventArg_Throws_ArgumentOutOfRangeException_Is_ActionType_Is_PlayedCard_And_Index_Is_Not_Set()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PlayedEventArgs(PlayerActionType.PlayedCard));
        }
        
        [Fact]
        public void
            Test_PlayedEventArg_Throws_ArgumentOutOfRangeException_Is_ActionType_Is_PlayedCard_And_ManaCost_Is_Not_Set()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PlayedEventArgs(PlayerActionType.PlayedCard, 1));
        }

        [Fact]
        public void Test_PlayedEventArg_Is_Initialized_Correctly_For_PlayedCard()
        {
            var pea = new PlayedEventArgs(PlayerActionType.PlayedCard, 2, 3);
            Assert.Equal(PlayerActionType.PlayedCard, pea.ActionType);
            Assert.Equal(2, pea.CardIndex);
            Assert.Equal(3, pea.ManaCost);
        }
    }
}