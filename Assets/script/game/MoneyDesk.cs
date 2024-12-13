using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MoneyType 
{
    TEN = 10,
    FIFTY = 50,
    HUNDRED = 100,
    TWO_HUNDRED = 200
}
public class MoneyDesk : MonoBehaviour
{
    public static MoneyDesk instance { get; private set; }
    public Transform moneyCardPrefab;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
