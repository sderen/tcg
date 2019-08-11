using System;
using System.Linq;
using System.Threading.Tasks;

namespace TradingCardGame.Domain
{
    public class Game
    {
        private Player _activePlayer;
        private Player _passivePlayer;

        private bool isStarted;

        public Game(Player playerA, Player playerB)
        {
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
            isStarted = true;
        }

        //TODO: race conditions will affect this
        private async void PlayerCommunicatorOnActionPerformed(object sender, PlayedEventArgs e)
        {
            if (!isStarted)
            {
                throw new InvalidOperationException("Can not consume events, since game has not been started yet");
            }
            
            if (!(sender is IPlayerCommunicator communicator) || !communicator.RelevantPlayerState.IsActive)
            {
                throw new InvalidOperationException();
            }

            //TODO: assume it is active for now, change it later
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