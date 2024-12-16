public enum MessageServerToClient
{
    PlayerChooseColor,
    GameStart,
}

public static class MessageTypeExtensions
{
    public static string ToMessageString(this MessageServerToClient messageType)
    {
        return messageType switch
        {
            MessageServerToClient.PlayerChooseColor => "player-choose-color",
            MessageServerToClient.GameStart => "game-start",
            _ => throw new System.ArgumentOutOfRangeException(nameof(messageType), messageType, null)
        };
    }
}
