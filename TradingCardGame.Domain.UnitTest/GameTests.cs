using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace TradingCardGame.Domain.UnitTest
{
    public class GameTests
    {
        [Fact]
        public async Task Test_Game_Sends_Relevant_Notifications_And_Performs_Correct_Actions()
        {
            var m1 = GetMockCommunicator();
            var m2 = GetMockCommunicator();

            var p1 = new Player(m1.Object, null);
            var p2 = new Player(m2.Object, null);

            var game = new Game(p1, p2);
            await game.StartGameAsync();

            Assert.NotEqual(p1.State.IsActive, p2.State.IsActive);

            var active = p1.State.IsActive ? p1 : p2;
            var deactive = p1.State.IsActive ? p2 : p1;

            var activeMock = p1.State.IsActive ? m1 : m2;
            var deactiveMock = p1.State.IsActive ? m2 : m1;

            Assert.Equal(4, active.State.Hand.Count);
            Assert.Equal(16, active.State.DeckCardCount);
            Assert.Equal(1, active.State.TotalManaSlots);
            Assert.Equal(1, active.State.AvailableManaSlots);

            active.PlayerCommunicator.PerformPlayerAction(new PlayedEventArgs(game.Tick, PlayerActionType.PlayedCard, 0,
                0));
            Assert.Equal(1, game.Tick);

            activeMock.Verify(m => m.CommunicateWithPlayerAsync(It.Is<CommunicationPackage>(p =>
                p.GameEventType == GameEventType.State && p.GameState.IsYourTurn && p.GameState.Hand.Count() == 3 &&
                p.GameState.Tick == 1 && p.GameState.Health == 30 && p.GameState.DeckSize == 16 &&
                p.GameState.OpponentHealth == 30 && p.GameState.OpponentHandCount == 3)), Times.Once);

            deactiveMock.Verify(m => m.CommunicateWithPlayerAsync(It.Is<CommunicationPackage>(p =>
                p.GameEventType == GameEventType.State && !p.GameState.IsYourTurn && p.GameState.Hand.Count() == 3 &&
                p.GameState.Tick == 1 && p.GameState.Health == 30 && p.GameState.DeckSize == 17 &&
                p.GameState.OpponentHealth == 30 && p.GameState.OpponentHandCount == 3)), Times.Once);

            active.PlayerCommunicator.PerformPlayerAction(new PlayedEventArgs(game.Tick, PlayerActionType.PlayedCard, 0,
                0));
            Assert.Equal(2, game.Tick);
            active.PlayerCommunicator.PerformPlayerAction(new PlayedEventArgs(game.Tick, PlayerActionType.PlayedCard, 0,
                1));
            Assert.Equal(3, game.Tick);

            Assert.Equal(0, active.State.AvailableManaSlots);
            Assert.Equal(29, deactive.State.Health);

            activeMock.Verify(m => m.CommunicateWithPlayerAsync(It.Is<CommunicationPackage>(p =>
                p.GameEventType == GameEventType.State && p.GameState.IsYourTurn && p.GameState.Hand.Count() == 1 &&
                p.GameState.Tick == 3 && p.GameState.Health == 30 && p.GameState.DeckSize == 16 &&
                p.GameState.OpponentDeckSize == 17 && p.GameState.OpponentHealth == 29 &&
                p.GameState.OpponentHandCount == 3)), Times.Once);

            deactiveMock.Verify(m => m.CommunicateWithPlayerAsync(It.Is<CommunicationPackage>(p =>
                p.GameEventType == GameEventType.State && !p.GameState.IsYourTurn && p.GameState.Hand.Count() == 3 &&
                p.GameState.Tick == 3 && p.GameState.Health == 29 && p.GameState.DeckSize == 17 &&
                p.GameState.OpponentDeckSize == 16 && p.GameState.OpponentHealth == 30 &&
                p.GameState.OpponentHandCount == 1)), Times.Once);

            active.PlayerCommunicator.PerformPlayerAction(new PlayedEventArgs(game.Tick, PlayerActionType.EndedTurn));
            Assert.Equal(4, game.Tick);

            Assert.False(active.State.IsActive);
            Assert.True(deactive.State.IsActive);

            activeMock.Verify(
                m => m.CommunicateWithPlayerAsync(It.Is<CommunicationPackage>(p =>
                    p.GameEventType == GameEventType.State && p.GameState.IsYourTurn == false)), Times.Once);
            deactiveMock.Verify(
                m => m.CommunicateWithPlayerAsync(It.Is<CommunicationPackage>(p =>
                    p.GameEventType == GameEventType.State && p.GameState.IsYourTurn)), Times.Once);

            activeMock.Verify(
                m => m.CommunicateWithPlayerAsync(It.Is<CommunicationPackage>(c =>
                    c.GameEventType == GameEventType.Deactivated)), Times.Once);
            deactiveMock.Verify(
                m => m.CommunicateWithPlayerAsync(It.Is<CommunicationPackage>(c =>
                    c.GameEventType == GameEventType.Activated)), Times.Once);

            // deactive player should die due to overdrawing in 45 turns
            for (int i = 0; i < 90; i++)
            {
                if (i % 2 == 0)
                {
                    deactive.PlayerCommunicator.PerformPlayerAction(new PlayedEventArgs(game.Tick,
                        PlayerActionType.EndedTurn));
                }
                else
                {
                    active.PlayerCommunicator.PerformPlayerAction(new PlayedEventArgs(game.Tick,
                        PlayerActionType.EndedTurn));
                }
            }

            Assert.True(deactive.State.IsDead());
            Assert.False(active.State.IsDead());

            activeMock.Verify(
                m => m.CommunicateWithPlayerAsync(It.Is<CommunicationPackage>(c =>
                    c.GameEventType == GameEventType.GameWon)), Times.Once);
            deactiveMock.Verify(
                m => m.CommunicateWithPlayerAsync(It.Is<CommunicationPackage>(c =>
                    c.GameEventType == GameEventType.GameLost)), Times.Once);
        }

        private Mock<ConsoleOutputProvider> GetMockCommunicator()
        {
            var mock = new Mock<ConsoleOutputProvider>();
            mock.Setup(m => m.RelevantPlayerState).CallBase();
            mock.Setup(m => m.SetPlayerState(It.IsAny<PlayerState>())).CallBase();
            mock.Setup(m => m.PerformPlayerAction(It.IsAny<PlayedEventArgs>())).CallBase().Verifiable();
            mock.Setup(m => m.CommunicateWithPlayerAsync(It.IsAny<CommunicationPackage>())).CallBase().Verifiable();
            return mock;
        }

        public class ConsoleOutputProvider : IPlayerCommunicator
        {
            public event EventHandler<PlayedEventArgs> ActionPerformed;

            public virtual void SetPlayerState(PlayerState playerState)
            {
                RelevantPlayerState = playerState;
            }

            public virtual PlayerState RelevantPlayerState { get; private set; }

            public virtual void PerformPlayerAction(PlayedEventArgs args)
            {
                ActionPerformed?.Invoke(this, args);
            }

            public virtual async Task CommunicateWithPlayerAsync(CommunicationPackage communicationPackage)
            {
                Console.WriteLine(communicationPackage);
            }
        }
    }
}