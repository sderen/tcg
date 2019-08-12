using System;
using System.Linq;
using System.Threading.Tasks;

namespace TradingCardGame.Domain
{
    // Game could also become the service Layer since it just drives the whole process with the actions.
    public class Game
    {
        private readonly bool _throwExceptions;
        private Player _activePlayer;
        private Player _passivePlayer;

        public bool IsStarted { get; private set; }
        public int Tick { get; private set; }
        
        public Game(Player playerA, Player playerB, bool throwExceptions = true)
        {
            _throwExceptions = throwExceptions;
            if (new Random().Next(0, 2) == 0)
            {
                _activePlayer = playerA;
                _passivePlayer = playerB;
            }
            else
            {
                _activePlayer = playerB;
                _passivePlayer = playerA;
            }

            _activePlayer.PlayerCommunicator.ActionPerformed += PlayerCommunicatorOnActionPerformed;

            _passivePlayer.PlayerCommunicator.ActionPerformed += PlayerCommunicatorOnActionPerformed;
        }

        public async Task StartGameAsync()
        {
            await Task.WhenAll(_activePlayer.PlayerCommunicator.CommunicateWithPlayerAsync(
                    new CommunicationPackage(GameEventType.GameStarted)),
                _activePlayer.PlayerCommunicator.CommunicateWithPlayerAsync(
                    new CommunicationPackage(GameEventType.GameStarted)));

            await ActivatePlayerAsync(_activePlayer);
            IsStarted = true;
            await SendSummaryAsync(false);
        }

        //TODO: race conditions will affect this
        private async void PlayerCommunicatorOnActionPerformed(object sender, PlayedEventArgs e)
        {
            try
            {
                if (!IsStarted)
                {
                    throw new InvalidOperationException("Can not consume events, since game has not been started yet");
                }

                if (!(sender is IPlayerCommunicator communicator) || !communicator.RelevantPlayerState.IsActive)
                {
                    throw new InvalidOperationException();
                }

                if (e.Tick != Tick)
                {
                    throw new InvalidOperationException(
                        "Message does not belong to current tick, only one action per tick can be performed");
                }

                if (e.ActionType == PlayerActionType.PlayedCard)
                {
                    var card = _activePlayer.State.Hand.ElementAt(e.CardIndex);
                    _activePlayer.State.PlayCard(e.CardIndex);
                    //TODO: get card 
                    _passivePlayer.State.DamagePlayer(card);

                    if (_passivePlayer.State.IsDead())
                    {
                        await FinishTheGameAsync();
                    }
                }
                else if (e.ActionType == PlayerActionType.EndedTurn)
                {
                    await SwitchActivePlayerAsync();
                }

                await SendSummaryAsync();
            }
            catch
            {
                if (_throwExceptions)
                {
                    throw;
                }
            }
        }

        private async Task SendSummaryAsync(bool incrementTick = true)
        {
            if (incrementTick)
            {
                Tick++;
            }

            await Task.WhenAll(
                SendSummaryToPlayerAsync(_activePlayer, _passivePlayer),
                SendSummaryToPlayerAsync(_passivePlayer, _activePlayer)
            );

        }

        private async Task SendSummaryToPlayerAsync(Player target, Player opponent)
        {
            var gs = new GameState(target.State.Hand, Tick, target.State.Health, opponent.State.Health, opponent.State.Hand.Count, target.State.IsActive, target.State.DeckCardCount, opponent.State.DeckCardCount);
            await target.PlayerCommunicator.CommunicateWithPlayerAsync(new CommunicationPackage(gs));
        }

        private async Task ActivatePlayerAsync(Player player)
        {
            player.State.Activate();
            await player.PlayerCommunicator.CommunicateWithPlayerAsync(
                new CommunicationPackage(GameEventType.Activated));
        }

        private async Task DeactivatePlayerAsync(Player player)
        {
            player.State.Deactivate();
            await player.PlayerCommunicator.CommunicateWithPlayerAsync(
                new CommunicationPackage(GameEventType.Deactivated));
        }

        private async Task SwitchActivePlayerAsync()
        {
            await Task.WhenAll(
                ActivatePlayerAsync(_passivePlayer),
                DeactivatePlayerAsync(_activePlayer));

            var player = _activePlayer;
            _activePlayer = _passivePlayer;
            _passivePlayer = player;

            if (_activePlayer.State.IsDead())
            {
                await FinishTheGameAsync(false);
            }
        }

        private async Task FinishTheGameAsync(bool activePlayerWins = true)
        {
            if (activePlayerWins)
            {
                await Task.WhenAll(
                    _activePlayer.PlayerCommunicator.CommunicateWithPlayerAsync(
                        new CommunicationPackage(GameEventType.GameWon)),
                    _passivePlayer.PlayerCommunicator.CommunicateWithPlayerAsync(
                        new CommunicationPackage(GameEventType.GameLost)));
            }
            else
            {
                await Task.WhenAll(
                    _activePlayer.PlayerCommunicator.CommunicateWithPlayerAsync(
                        new CommunicationPackage(GameEventType.GameLost)),
                    _passivePlayer.PlayerCommunicator.CommunicateWithPlayerAsync(
                        new CommunicationPackage(GameEventType.GameWon)));
            }
        }
    }
}