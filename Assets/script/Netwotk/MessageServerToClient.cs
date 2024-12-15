public enum MessageServerToClient
{
    PlayerChooseColor,
    GameStart,
    UpdateScore,
}

public static class MessageTypeExtensions
{
    public static string ToMessageString(this MessageServerToClient messageType)
    {
        return messageType switch
        {
            MessageServerToClient.PlayerChooseColor => "player-choose-color",
            MessageServerToClient.GameStart => "game-start",
            MessageServerToClient.UpdateScore => "update-score",
            _ => throw new System.ArgumentOutOfRangeException(nameof(messageType), messageType, null)
        };
    }
}
