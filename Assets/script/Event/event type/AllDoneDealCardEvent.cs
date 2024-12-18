using System.Collections.Generic;
using UnityEngine;

public class AllDoneDealCardEvent
{
    public List<int[]> storeCards;

    public AllDoneDealCardEvent(List<int[]> storecards)
    {
        this.storeCards = storecards;
    }
}