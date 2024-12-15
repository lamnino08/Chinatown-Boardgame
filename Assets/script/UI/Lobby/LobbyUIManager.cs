using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Colyseus.Schema;

public class LobbyUIManager : BasePopup
{
    public static LobbyUIManager instance { get; private set; }

    [Header("Player slot join lobby")]
    [SerializeField]  private Transform playerItemPref;
    [SerializeField]  private Transform contentPlayers;


    [Header("Button")]
    [Tooltip("Button open color popup")]
    [SerializeField]  private Button colorBtn;
    [SerializeField]  private GameObject readyBtn;
    [SerializeField]  private GameObject startBtn;

    void Awake()
    {
        instance = this;
        colorBtn.onClick.AddListener(OnOpenColorPopup);
        readyBtn.GetComponent<Button>().onClick.AddListener(OnReadyBtn);
    }

    private void OnOpenColorPopup() 
    {
        LobbyPopupManager.instance.ShowChoseColorPopup(LobbyController.state.colors);
    }

    private void OnReadyBtn()
    {
        LobbyManager.instance.Ready();
    }

    private void OnStartGame()
    {
        startBtn.GetComponent<Button>().interactable = false;
        GameMaster.localPlayer.StartGame();
    }

    public void AddNewPlayer(PlayerLobby player)
    {
        Transform playeritem = Instantiate(playerItemPref, contentPlayers);
        playeritem.GetComponent<PlayerItemPrefab>().SetData(player);    
    } 

     public void RemovePlayerUI(string name)
    {
        foreach (Transform child in contentPlayers)
        {
            var playerItem = child.GetComponent<PlayerItemPrefab>();
            if (playerItem != null && playerItem.Name == name)
            {
                Destroy(child.gameObject);
            }
        }
    } 

    public void SetColorPlayer(string name, Color color, bool isReady = false)
    {
        if (name == GameMaster.PlayerName)
        {
            readyBtn.SetActive(true);
            colorBtn.gameObject.SetActive(false);
        }
        foreach (Transform child in contentPlayers)
        {
            var playerItem = child.GetComponent<PlayerItemPrefab>();
            if (playerItem != null && playerItem.Name == name)
            {
                playerItem.SetColor(color, isReady);
            }
        }
    }
}
