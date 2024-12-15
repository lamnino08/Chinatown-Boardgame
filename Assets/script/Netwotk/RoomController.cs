using Colyseus;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    private ColyseusRoom<Room> _room;

    public void Init(ColyseusRoom<Room> room)
    {
        _room = room;

        Debug.Log(_room.State);

        // Call back 
        _room.State.players.OnAdd(OnAddPlayer);
        _room.State.players.OnRemove(OnRemovePlayer);
        _room.State.OnChange(OnRoomStateChange);

        Debug.Log("RoomController initialized.");
    }

    private void OnAddPlayer(string key, Player player)
    {
        Debug.Log($"Player added: {player.name} (Key: {key})");
        
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
