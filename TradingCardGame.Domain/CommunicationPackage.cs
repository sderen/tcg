namespace TradingCardGame.Domain
{
    public class CommunicationPackage
    {
        public GameEventType? GameEventType { get;}
        public PlayerActionType? PlayerActionType { get; }

        public GameState GameState { get; }

        public CommunicationPackage(GameEventType gameEventType)
        {
            GameEventType = gameEventType;
        }

        public CommunicationPackage(PlayerActionType playerActionType)
        {
            PlayerActionType = playerActionType;
        }

        public CommunicationPackage(GameState gameState)
        {
            GameEventType = Domain.GameEventType.State;
            GameState = gameState;
        }

        public override string ToString()
        {
            return $"{GameEventType} {PlayerActionType}";
        }
    }
}