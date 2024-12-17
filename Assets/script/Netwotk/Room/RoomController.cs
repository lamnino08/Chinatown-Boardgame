using Colyseus;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static ColyseusRoom<Room> room { get; private set; }

    public static Room state;
    public void Init(ColyseusRoom<Room> room)
    {
        RoomController.room = room;

        // Call back 
        room.State.players.OnAdd(OnAddPlayer);
        room.State.players.OnRemove(OnRemovePlayer);
        room.State.OnChange(OnRoomStateChange);

        RegisterAllMessages();
    }

    private void RegisterAllMessages()
    {
        foreach (MessageServerToClientGame messageType in System.Enum.GetValues(typeof(MessageServerToClientGame)))
        {
            room.OnMessage<Dictionary<string, object>>(messageType.ToString(), (data) =>
            {
                Debug.Log($"On message {messageType.ToString()}");
                EventHandlerNetwork.Trigger(messageType.ToString(), data);
            });
        }
    }

    private void OnAddPlayer(string key, Player player)
    {
        Debug.Log($"Player added: {player.name} (Key: {key})");
        if (key == GameMaster.sessionId)
        {
            GameMaster.index = player.index;
            GameManager.instance.SetView(player.index);
            Debug.Log($"your index {GameMaster.index}");
        }

        GameManager.instance.SpawnPlayerSlot(player);
        GameUIManager.instance.SetStart();
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
