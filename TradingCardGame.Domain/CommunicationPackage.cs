namespace TradingCardGame.Domain
{
    public class CommunicationPackage
    {
        public GameEventType? GameEventType { get;}
        public PlayerActionType? PlayerActionType { get; }
        public string StringPayload { get; set; }
        public int? IntPayload { get; set; }
        
        public CommunicationPackage(GameEventType gameEventType)
        {
            GameEventType = gameEventType;
        }

        public CommunicationPackage(PlayerActionType playerActionType)
        {
            PlayerActionType = playerActionType;
        }

        public override string ToString()
        {
            return $"{GameEventType} {PlayerActionType} {StringPayload} {IntPayload}";
        }
    }
}