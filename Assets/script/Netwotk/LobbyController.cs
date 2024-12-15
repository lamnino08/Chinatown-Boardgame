using System.Collections.Generic;
using Colyseus;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public static ColyseusRoom<Lobby> lobby { get; private set; }

    public static Lobby state;

    public void Init(ColyseusRoom<Lobby> lobby)
    {
        LobbyController.lobby = lobby;
        state = lobby.State;
        lobby.State.players.OnAdd(OnAddPlayer);
        lobby.State.players.OnRemove(OnRemovePlayer);

        RegisterAllMessages(); // Registe trigger all message from server
    }

    private void RegisterAllMessages()
    {
        foreach (MessageServerToClient messageType in System.Enum.GetValues(typeof(MessageServerToClient)))
        {
            RegisterMessage(messageType);
        }
    }

    private void RegisterMessage(MessageServerToClient messageType)
    {
        lobby.OnMessage<Dictionary<string, object>>(messageType.ToMessageString(), (data) =>
        {
            Debug.Log($"On message {messageType.ToMessageString()}");
            EventHandlerNetwork.Trigger(messageType.ToMessageString(), data);
        });
    }

    private void OnAddPlayer(string key, PlayerLobby player)
    {
        LobbyUIManager.instance.AddNewPlayer(player);
    }

    private void OnRemovePlayer(string key, PlayerLobby player)
    {
        Debug.Log($"Player removed: {player.name} (Key: {key})");
    }
}
