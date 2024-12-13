using System.Collections.Generic;
using UnityEngine;

public class MoneyStack : MonoBehaviour
{
    private static Vector3 distance = new Vector3(0, 0.03f, 0);
    [SerializeField] private MoneyType type;
    [SerializeField] private int amount;
    [SerializeField] private List<MoneyCard> _moneyCards = new List<MoneyCard>();

    public void Start()
    {
        Vector3 startPos = transform.position;
        for(int i = 1; i <= amount; i++)
        {
            Transform card = Instantiate(MoneyDesk.instance.moneyCardPrefab, transform.position + distance * i, transform.rotation);
            card.position = startPos + distance * i;

        }
    }
}
