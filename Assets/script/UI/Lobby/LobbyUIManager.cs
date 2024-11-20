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
        colorBtn.onClick.AddListener(OpenColorPopup);
    }

    private void OpenColorPopup() 
    {
        LobbyPopupManager.instance.ShowChoseColorPopup();
    }

    public void SetSlotPlayerUI(PlayerData[] players)
    {
        foreach(PlayerData player in players)
        {
            Transform playeritem = Instantiate(playerItemPref, contentPlayers);
            playeritem.GetComponent<PlayerItemPrefab>().SetData(player.name);
        }
    } 

    public void AddNewPlayerUI(PlayerData player)
    {
        Transform playeritem = Instantiate(playerItemPref, contentPlayers);
        playeritem.GetComponent<PlayerItemPrefab>().SetData(player.name);
    } 

     public void RemovePlayer(string name)
    {
         foreach (Transform child in contentPlayers)
        {
            var playerItem = child.GetComponent<PlayerItemPrefab>();
            if (playerItem != null && playerItem.Name == name)
            {
                // Xóa GameObject của player được tìm thấy
                Destroy(child.gameObject);
                Debug.Log($"Removed player: {name}");
                return;
            }
        }
    } 
}
