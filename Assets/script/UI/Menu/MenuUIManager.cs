using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MenuUIManager : BasePopup
{
    public static MenuUIManager instance { get; private set; }
    [SerializeField] private MenuManager menuManager;
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Transform roomSlotPref;
    [SerializeField] private Transform content;
    [SerializeField] private Button newRoomButton;
    [SerializeField] private Button reloadButton;

    void Awake()
    {
        instance = this;
    }

    private void Start() 
    {
        newRoomButton.onClick.AddListener(OnNewRoom);
        reloadButton.onClick.AddListener(menuManager.GetAllRoom);
    }

    private void OnNewRoom()
    {
        if (nameInputField.text == "")
        {
            LobbyPopupManager.instance.Toast("Name player is empty");
            return;
        }

        LobbyNetworkManager.instance.NewRoom(nameInputField.text);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void SetRoomData(List<LobbyServer> lobbys)
    {
        foreach (LobbyServer lobby in lobbys)
        {
            Transform roomItem = Instantiate(roomSlotPref, content);
            RoomItem itemScript = roomItem.GetComponent<RoomItem>();
            itemScript.Init(lobby, nameInputField.text);
        }
    }
}
