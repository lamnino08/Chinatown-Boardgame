using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerSlot : MonoBehaviour
{
    public static PlayerSlot localPlayerSlot;
    [SerializeField] private Transform _storeCardContain;
    [SerializeField] private float _spacing = 0.32f;
    [SerializeField] private MarkBowl _markBowl;
    [SerializeField] private Transform _cardHole;

    private List<int> cardDeals = new List<int>();

    private int _owner;
    private int _color;
    private string _sessionId; public string sessionId => _sessionId;

    void Start()
    {
        EventBus.Subscribe<PlayerDoneDealCardEvent>(OnPlayerDoneDealTieCard);
        EventBus.Subscribe<AllDoneDealCardEvent>(OnAllDoneDealCardEvent);
    }


    public void SetData(Player player)
    {
        _owner = player.index;
        _color = player.color;
        _sessionId = player.sessionId;
    }

    private void OnPlayerDoneDealTieCard(PlayerDoneDealCardEvent data)
    {
        if (data.sessionId == _sessionId)
        {
            this.cardDeals = data.cards;
        }
    }

    private void OnAllDoneDealCardEvent(AllDoneDealCardEvent data)
    {
        _markBowl.SpawnMarks(cardDeals, _color, _owner, _sessionId);
    }

    public List<Vector3> GetPosStoreCard(int numberCard)
    {
        List<Vector3> list = new List<Vector3>();
        Vector3 lineVec = _storeCardContain.right;

        Vector3 startPos = _storeCardContain.position - (_spacing *( (numberCard)/2 - 0.5f) + (numberCard % 2)/2) * lineVec;
        for (int i = 0; i < numberCard; i++)
        {
            Vector3 posCard = startPos + lineVec * (i * _spacing) + new Vector3(0, 0.25f, 0);
            list.Add(posCard);
        }
        return list;
    }
}
