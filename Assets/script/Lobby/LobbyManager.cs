using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance { get; private set; }

    private void Awake() {
        instance = this;

        // Tự động đăng ký tất cả các sự kiện dựa trên enum
        foreach (MessageServerToClientLobby messageType in Enum.GetValues(typeof(MessageServerToClientLobby)))
        {
            string methodName = $"NW_{messageType}";
            Debug.Log(messageType.ToString());
            string messageString = messageType.ToString();

            MethodInfo method = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method != null)
            {
                EventHandlerNetwork.Register(messageString, (data) =>
                {
                    method.Invoke(this, new object[] { data });
                });

                Debug.Log($"Registered handler for {messageType} -> {methodName} with message type: {messageString}");
            }
            else
            {
                Debug.LogWarning($"No handler found for {messageType} -> {methodName}");
            }
        }
    }

    public void Ready()
    {
        LobbyController.lobby.Send("select_color", new { color = GameMaster.color });
    }

    public void StartGame()
    {
        LobbyController.lobby.Send("start_game");
    }

    // Hàm xử lý cho PlayerChooseColor
    private void NW_player_choose_color(object data)
    {
        var colorData = data as Dictionary<string, object>;
        string sessionId = colorData["sessionId"].ToString();
        int color = Convert.ToInt32(colorData["color"]);
        bool isAllReady = (bool)colorData["isAllReady"];

        string playerName = LobbyController.state.GetNamePlayerBySessionID(sessionId);
        Color colors = Util.TransferColor(color);
        LobbyUIManager.instance.SetColorPlayer(playerName, colors, true, isAllReady);
    }

    // Hàm xử lý cho GameStart
    private void NW_game_start(object data)
    {
        Debug.Log("Game has started!");
        var Data = data as Dictionary<string, object>;
        string roomId = Data["roomId"].ToString();
        
        RoomNetworkManager.instance.ConnectToServer(roomId);
    }
}
