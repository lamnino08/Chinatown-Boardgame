using System.Collections.Generic;
using Colyseus;
using UnityEngine;

public class LobbyNetworkManager : MonoBehaviour
{
    public static LobbyNetworkManager instance { get; private set; }

    [SerializeField] private LobbyController _lobbyController;
    public LobbyController  lobbyController => _lobbyController;
    public string serverUrl = "";

    private ColyseusClient _client;

    private void Awake()
    {
        instance = this;
    }

    private async void ConnectToServer(string playerName, string roomId = null)
    {
        if (string.IsNullOrEmpty(serverUrl))
        {
            Debug.LogError("Server URL is not set!");
            LobbyPopupManager.instance?.Toast("Server URL is not configured.");
            return;
        }

        _client = new ColyseusClient($"ws://{serverUrl}");
        var options = new Dictionary<string, object> { { "playerName", playerName } };

        ColyseusRoom<Lobby> room;
        if (string.IsNullOrEmpty(roomId))
        {
            // Create a new room
            room = await _client.Create<Lobby>("lobby", options);
            GameMaster.PlayerName = playerName;
        }
        else
        {
            // Join an existing room
            room = await _client.JoinById<Lobby>(roomId, options);
        }
        _lobbyController.Init(room);
        LobbyPopupManager.instance.OpenLobby();
    }

    public void NewRoom(string playerName)
    {
        ConnectToServer(playerName);
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

