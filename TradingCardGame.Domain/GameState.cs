using System;
using System.Collections.Generic;

namespace TradingCardGame.Domain
{
    [Serializable]
    public class GameState
    {
        public IEnumerable<byte> Hand { get; }
        public int Tick { get; }
        public byte Health { get; }
        public byte OpponentHealth { get; }
        public int OpponentHandCount { get; }
        public bool IsYourTurn { get; }
        public int DeckSize { get; }
        public int OpponentDeckSize { get; }

        public GameState(IEnumerable<byte> hand, int tick, byte health, byte opponentHealth, int opponentHandCount, bool isYourTurn, int deckSize, int opponentDeckSize)
        {
            Hand = hand;
            Tick = tick;
            Health = health;
            OpponentHealth = opponentHealth;
            OpponentHandCount = opponentHandCount;
            IsYourTurn = isYourTurn;
            DeckSize = deckSize;
            OpponentDeckSize = opponentDeckSize;
        }

        public override string ToString()
        {
            return $"Hand: {string.Join("-", Hand)}\tOpponentHandSize: {OpponentHandCount}\nYourHealth: {Health}\tOpponentHealth: {OpponentHealth}\nYourDeckSize: {DeckSize}\tOpponentDeckSize: {OpponentDeckSize}\nTick: {Tick}\n------------\n";
        }
    }
}