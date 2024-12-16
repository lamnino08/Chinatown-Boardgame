using Colyseus;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static ColyseusRoom<Room> room { get; private set; }

    public static Lobby state;
    public void Init(ColyseusRoom<Room> room)
    {
        RoomController.room = room;

        // Call back 
        room.State.players.OnAdd(OnAddPlayer);
        room.State.players.OnRemove(OnRemovePlayer);
        room.State.OnChange(OnRoomStateChange);
        Debug.Log("RoomController initialized.");
    }

    private void OnAddPlayer(string key, Player player)
    {
        Debug.Log($"Player added: {player.name} (Key: {key})");
        if (key == GameMaster.sessionId)
        {
            GameMaster.index = player.index;
            Debug.Log($"your index {GameMaster.index}");
        }

        
    }

    private void OnRemovePlayer(string key, Player player)
    {
        Debug.Log($"Player removed: {player.name} (Key: {key})");
    }

    private void OnRoomStateChange()
    {
        Debug.Log("Room state has changed!");
    }
}
