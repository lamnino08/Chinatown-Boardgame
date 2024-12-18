public class JoinRoomEvent
{
    public int index;
    public string sessionId;

    public JoinRoomEvent(int index, string sessionId)
    {
        this.index = index;
        this.sessionId = sessionId;
    }
}
