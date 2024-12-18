using System.Collections.Generic;

public class PlayerDoneDealCardEvent
{
    public string sessionId;
    public List<int> cards;

    public PlayerDoneDealCardEvent(string sessionId, List<int> cards)
    {
        this.sessionId = sessionId;
        this.cards = cards;
    }
}