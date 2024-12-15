using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TMP_Text roomIDText;
    [SerializeField] private TMP_Text numberPlayerText;
    [SerializeField] private Button joinButton;

    public void Init(LobbyServer lobby, string playerName)
    {
        this.roomIDText.text = lobby.roomId.ToString();
        this.numberPlayerText.text = lobby.clients.ToString();

        joinButton.onClick.AddListener(() => OnJoinRoom(lobby.roomId, playerName));
    }

    private void OnJoinRoom(string idRoom, string playerName)
    {
        LobbyNetworkManager.instance.JoinRoom(idRoom, playerName);
    }
}
