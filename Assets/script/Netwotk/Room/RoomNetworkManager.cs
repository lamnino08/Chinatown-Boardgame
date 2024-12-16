using System.Collections.Generic;
using Colyseus;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomNetworkManager : MonoBehaviour
{
    public static RoomNetworkManager instance { get; private set; }

    [SerializeField] private RoomController _roomController;
    public RoomController  roomController => _roomController;
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

    public async void ConnectToServer(string roomId)
    {
        SceneManager.LoadScene("GameScene");
        _client = new ColyseusClient($"ws://{serverUrl}");

        string playerName = GameMaster.PlayerName;

        
        var options = new Dictionary<string, object> { { "playerName", playerName } };

        ColyseusRoom<Room> room = await _client.JoinById<Room>(roomId, options);

        GameMaster.sessionId = room.SessionId;
        _roomController.Init(room);
    }
}
