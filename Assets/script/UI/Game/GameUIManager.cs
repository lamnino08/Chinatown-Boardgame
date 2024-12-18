using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Mirror;

public class GameUIManager : MonoBehaviour
{
    private static GameUIManager _instance;
    public static GameUIManager instance => _instance;

    [Header("Distribute card")]
    [SerializeField] private Button discardBtn;
    [SerializeField] private Button confirmCardChose;

    private void Awake() 
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        EventBus.Subscribe<JoinRoomEvent>(JoinRoom);
    }

    private void Start() 
    {
        confirmCardChose.onClick.AddListener(OnConfirmCardChose);
        // OnStartGame();
    }

    public void JoinRoom(JoinRoomEvent data) 
    {
        if (data.index == 0)
        {
            discardBtn.onClick.AddListener(OndisCard);
            discardBtn.gameObject.SetActive(GameMaster.Ishost());   
        }
    }

    private void OndisCard()
    {
        discardBtn.interactable= false;
        // GameMaster.localPlayer.NewYear();
        GameManager.instance.NewYear();
    }

    private void OnConfirmCardChose()
    {
        GameMaster.instance.deskCard.ConfirmCardChose();
    }

    public void ReceiveCardDiscard()
    {
        if (GameMaster.Ishost())
        {
            discardBtn.gameObject.SetActive(false);
        }
    }
}
