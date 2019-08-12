using System;
using System.Linq;
using System.Threading.Tasks;
using TradingCardGame.Domain;
using TradingCardGame.Impl.InProcess;

namespace TradingCardGame.Impl.InProcessConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
            
        }

        private static async Task MainAsync()
        {
            var c1 = new InProcessPlayerCommunicator("player 1");
            var c2 = new InProcessPlayerCommunicator("player 2");
            
            var p1 = new Player(c1, new KnuthShuffler());
            var p2 = new Player(c2, new KnuthShuffler());

            var game = new Game(p1, p2, false);
            await game.StartGameAsync();

            while (!(p1.State.IsDead() || p2.State.IsDead()))
            {
                var activeComm = p1.State.IsActive ? c1 : c2;
                Console.WriteLine("----------");
                Console.Write($"Active: {activeComm.PlayerName} ##  Enter card index to play or -1 to end turn: ");
                int idx;
                if (!int.TryParse(Console.ReadLine(), out idx))
                {
                    continue;
                }
                if (idx < 0)
                {
                    activeComm.PerformPlayerAction(new PlayedEventArgs(activeComm.LatestGameState.Tick,
                        PlayerActionType.EndedTurn));
                }
                else if(idx < activeComm.LatestGameState.Hand.Count())
                {
                    activeComm.PerformPlayerAction(new PlayedEventArgs(activeComm.LatestGameState.Tick, PlayerActionType.PlayedCard, idx, activeComm.LatestGameState.Hand.ElementAt(idx)));
                }
            }
            
            
            
        }
    }
}