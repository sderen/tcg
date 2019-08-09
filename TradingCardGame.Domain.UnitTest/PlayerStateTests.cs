using System.Linq;
using Xunit;

namespace TradingCardGame.Domain.UnitTest
{
    public class PlayerStateTests
    {
        [Fact]
        public void Test_PlayerState_Is_Initialized_Properly()
        {
            var playerState = new PlayerState();
            
            Assert.False(playerState.IsActive);
            Assert.Equal(0, playerState.TotalManaSlots);
            Assert.Equal(30, playerState.Health);
            Assert.False(playerState.IsDead());
            Assert.Equal(0, playerState.AvailableManaSlots);
            var hand = playerState.Hand.ToArray();
            Assert.Equal(5, hand.Length);
            Assert.NotEqual(11, hand[0]);
            Assert.NotEqual(11, hand[1]);
            Assert.NotEqual(11, hand[2]);
            Assert.Equal(11, hand[3]);
            Assert.Equal(11, hand[4]);
        }

        [Fact]
        public void Test_PlayerState_Activate_And_Deactivate_Works_Correctly()
        {
            var playerState = new PlayerState();
            
            for (int i = 0; i < 10; i++)
            {
                playerState.Activate();
                Assert.True(playerState.IsActive);
                Assert.Equal(i + 1, playerState.TotalManaSlots);
                Assert.Equal(i + 1, playerState.AvailableManaSlots);
                playerState.Deactivate();
                Assert.False(playerState.IsActive);
            }
            
            playerState.Activate();
            Assert.True(playerState.IsActive);
            Assert.Equal(10, playerState.TotalManaSlots);
            Assert.Equal(10, playerState.AvailableManaSlots);
        }

        [Fact]
        public void Test_PlayerState_Activation_Is_Idempotent()
        {
            var playerState = new PlayerState();
            
            playerState.Activate();
            Assert.Equal(1, playerState.TotalManaSlots);
            playerState.Activate();
            Assert.Equal(1, playerState.TotalManaSlots);
        }

        [Fact]
        public void Test_Player_Is_Damaged_Correctly_And_Dead_After_Health_Is_Equal_To_Zero()
        {
            var playerState = new PlayerState();
            
            Assert.False(playerState.IsDead());
            playerState.DamagePlayer(3);
            Assert.Equal(27, playerState.Health);
            Assert.False(playerState.IsDead());
            playerState.DamagePlayer(17);
            Assert.Equal(10, playerState.Health);
            Assert.False(playerState.IsDead());
            playerState.DamagePlayer(10);
            Assert.True(playerState.IsDead());
        }

        [Fact]
        public void Test_Player_Is_Damaged_Correctly_And_Dead_After_Health_Is_Equal_Below_Zero()
        {
            var playerState = new PlayerState();
            
            playerState.DamagePlayer(35);
            
            Assert.True(playerState.IsDead());
        }
        
        
    }
}