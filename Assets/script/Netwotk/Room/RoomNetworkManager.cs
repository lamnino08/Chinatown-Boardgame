using System.Collections.Generic;
using Colyseus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomNetworkManager : MonoBehaviour
{
    public static RoomNetworkManager instance { get; private set; }

    [SerializeField] private LobbyController _lobbyController;
    public LobbyController  lobbyController => _lobbyController;
    public string serverUrl = "";

    private ColyseusClient _client;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public async void ConnectToServer(string roomId = null)
    {
        SceneManager.LoadScene("GameScene");
        _client = new ColyseusClient($"ws://{serverUrl}");

        string playerName = GameMaster.PlayerName;

        ColyseusRoom<Room> room;
        var options = new Dictionary<string, object> { { "playerName", playerName } };

        room = await _client.JoinById<Room>(roomId, options);

        Debug.Log("ok");

        // _lobbyController.Init(room);
        // LobbyPopupManager.instance.OpenLobby();
    }

    public void JoinRoom(string roomId, string playerName)
    {
        ConnectToServer(playerName, roomId);
    }

    private void OnApplicationQuit()
    {
        _lobbyController?.gameObject?.SetActive(false); // Optional cleanup
    }
}
