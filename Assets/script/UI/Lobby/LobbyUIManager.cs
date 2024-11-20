using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Mirror;

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
        startBtn.SetActive(isHost);
    }

    private void OnOpenColorPopup() 
    {
        GameMaster.instance.localPlayer.OpenColorPupup();
    }

    private void OpenColorPopup(List<byte> availableColors)
    {
        LobbyPopupManager.instance.ShowChoseColorPopup(availableColors);
    }

    private void OnReadyBtn()
    {
        GameMaster.instance.localPlayer.CmdReady();
    }

    public void SetSlotPlayerUI(PlayerData[] players)
    {
        foreach(PlayerData player in players)
        {
            Transform playeritem = Instantiate(playerItemPref, contentPlayers);
            playeritem.GetComponent<PlayerItemPrefab>().SetData(player.name);
        }

        startBtn.SetActive(GameMaster.instance.localPlayer.isHost);
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

    public void SetColorPlayer(byte index, Color color)
    {
        contentPlayers.GetChild(index).GetComponent<PlayerItemPrefab>().SetColor(color);
        if (index == GameMaster.instance.localPlayer.index)
        {
            readyBtn.SetActive(true);
            colorBtn.gameObject.SetActive(false);
        }
    }

    public void OnReadyResult(bool isReady)
    {
        if (isReady)
        {
           LobbyPopupManager.instance.Toast("Ready");
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
