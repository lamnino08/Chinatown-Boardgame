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
    }

    private void Start() 
    {
        discardBtn.onClick.AddListener(OndisCard);
        confirmCardChose.onClick.AddListener(OnConfirmCardChose);
        OnStartGame();
    }

    private void OnStartGame() 
    {
        discardBtn.gameObject.SetActive(GameMaster.localPlayer.isHost);
    }

    private void OndisCard()
    {
        discardBtn.interactable= false;
        GameMaster.localPlayer.NewYear();
    }

    private void OnConfirmCardChose()
    {
        GameMaster.instance.deskCard.ConfirmCardChose();
    }

    public void ReceiveCardDiscard()
    {
        if (GameMaster.localPlayer.isHost)
        {
            discardBtn.gameObject.SetActive(false);
        }
    }
}
