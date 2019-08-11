using System;
using System.Linq;
using System.Security.Cryptography;
using Xunit;

namespace TradingCardGame.Domain.UnitTest
{
    public class PlayerStateTests
    {
        [Fact]
        public void Test_PlayerState_Is_Initialized_Properly()
        {
            var playerState = new PlayerState(new SimpleShuffler());
            
            Assert.False(playerState.IsActive);
            Assert.Equal(0, playerState.TotalManaSlots);
            Assert.Equal(30, playerState.Health);
            Assert.False(playerState.IsDead());
            Assert.Equal(0, playerState.AvailableManaSlots);
            Assert.Equal(17, playerState.DeckCardCount);
            var hand = playerState.Hand.ToArray();
            Assert.Equal(3, hand.Length);
            Assert.NotNull(hand[0]);
            Assert.NotNull(hand[1]);
            Assert.NotNull(hand[2]);
 
        }

        [Fact]
        public void Test_PlayerState_Activate_And_Deactivate_Works_Correctly()
        {
            var playerState = new PlayerState(new SimpleShuffler());
            
            for (int i = 0; i < 10; i++)
            {
                playerState.Activate();
                Assert.True(playerState.IsActive);
                Assert.Equal(i + 1, playerState.TotalManaSlots);
                Assert.Equal(i + 1, playerState.AvailableManaSlots);
                Assert.Equal(Math.Min(i + 4, 5), playerState.Hand.Count);
                Assert.Equal(16 - i, playerState.DeckCardCount);
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
            var playerState = new PlayerState(new SimpleShuffler());
            
            playerState.Activate();
            Assert.Equal(1, playerState.TotalManaSlots);
            playerState.Activate();
            Assert.Equal(1, playerState.TotalManaSlots);
        }

        [Fact]
        public void Test_Player_Is_Damaged_Correctly_And_Dead_After_Health_Is_Equal_To_Zero()
        {
            var playerState = new PlayerState(new SimpleShuffler());
            
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
        public void Test_Player_Is_Damaged_Correctly_And_Dead_After_Health_Is_Below_Zero()
        {
            var playerState = new PlayerState(new SimpleShuffler());
            
            playerState.DamagePlayer(35);
            
            Assert.True(playerState.IsDead());
        }

        [Fact]
        public void Test_Player_Takes_Damage_For_Drawing_After_Deck_Is_Empty()
        {
            var playerState = new PlayerState(new SimpleShuffler());
            for (int i = 0; i < 17; i++)
            {
                playerState.Activate();
                playerState.Deactivate();
            }
            
            Assert.Equal(30, playerState.Health);
            for (int i = 29; i <= 0; i--)
            {
                playerState.Activate();
                Assert.Equal(i, playerState.Health);
                playerState.Deactivate();
            }

        }

        [Fact]
        public void Test_PlayCard_Removes_CardFrom_Hand()
        {
            var playerState = new PlayerState(null);
            var hand = playerState.Hand;
            for (int i = 0; i < 3; i++)
            {
                playerState.Activate();
                playerState.Deactivate();
            }
            playerState.Activate();
            playerState.PlayCard(1);
            Assert.Equal(4, playerState.Hand.Count);
            Assert.Equal(hand.ElementAt(0), playerState.Hand.ElementAt(0));
            Assert.Equal(hand.ElementAt(2), playerState.Hand.ElementAt(1));
        }
        
    }
}