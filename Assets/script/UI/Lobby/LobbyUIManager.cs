using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Mirror;
using DG.Tweening.Core.Easing;

public class LobbyUIManager : MonoBehaviour
{
    private static LobbyUIManager _instance;
    public static LobbyUIManager instance => _instance;


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
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start() {
        colorBtn.onClick.AddListener(OnOpenColorPopup);
        readyBtn.GetComponent<Button>().onClick.AddListener(OnReadyBtn);
    }

    public void SetupUI(bool isHost)
    {
        if (isHost)
        {
            startBtn.SetActive(true);
            startBtn.GetComponent<Button>().onClick.AddListener(OnStartGame);
        }
    }

    private void OnOpenColorPopup() 
    {
        GameMaster.localPlayer.OpenColorPupup();
    }

    private void OpenColorPopup(List<byte> availableColors)
    {
        LobbyPopupManager.instance.ShowChoseColorPopup(availableColors);
    }

    private void OnReadyBtn()
    {
        GameMaster.localPlayer.CmdReady();
    }

    private void OnStartGame()
    {
        startBtn.GetComponent<Button>().interactable = false;
        GameMaster.localPlayer.StartGame();
    }

    public void SetSlotPlayerUI(PlayerData[] players)
    {
        foreach(PlayerData player in players)
        {
            Transform playeritem = Instantiate(playerItemPref, contentPlayers);
            playeritem.GetComponent<PlayerItemPrefab>().SetData(player.name);
        }

        startBtn.SetActive(GameMaster.localPlayer.isHost);
    } 

    public void AddNewPlayerUI(PlayerData player)
    {
        Transform playeritem = Instantiate(playerItemPref, contentPlayers);
        playeritem.GetComponent<PlayerItemPrefab>().SetData(player.name);
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

    public void SetColorPlayer(string name, Color color)
    {
        if (name == GameMaster.localPlayer.playerName)
        {
            readyBtn.SetActive(true);
            colorBtn.gameObject.SetActive(false);
        }
        foreach (Transform child in contentPlayers)
        {
            var playerItem = child.GetComponent<PlayerItemPrefab>();
            if (playerItem != null && playerItem.Name == name)
            {
                playerItem.SetColor(color);
            }
        }
    }

    public void OnReadyResult(bool isReady)
    {
        if (isReady)
        {
           LobbyPopupManager.instance.Toast("Ready");
           readyBtn.SetActive(false);  
        }
        else
        {
           LobbyPopupManager.instance.Toast("Color is not available");
           readyBtn.SetActive(false);
           colorBtn.gameObject.SetActive(true);
        }
    }

    public void PlayerReady(string name, Color color)
    {
        foreach (Transform child in contentPlayers)
        {
            var playerItem = child.GetComponent<PlayerItemPrefab>();
            if (playerItem.Name == name)
                playerItem.SetReady(true, color);
        }
    }
}
