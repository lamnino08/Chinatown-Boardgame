using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStack : MonoBehaviour
{
    private static Vector3 distance = new Vector3(0, 0.03f, 0);
    [SerializeField] private int money;
    [SerializeField] private int amount;
    [SerializeField] private Transform moneyCardPref;

    public void Start()
    {
        Vector3 startPos = transform.position;
        for(int i = 1; i <= amount; i++)
        {
            Transform card = Instantiate(moneyCardPref, transform);
            card.position = startPos + distance * i;
        }
    }
}
