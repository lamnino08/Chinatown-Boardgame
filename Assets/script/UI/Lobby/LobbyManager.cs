using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance { get; private set; }

    private void Awake() {
        instance = this;
        EventHandlerNetwork.Register(MessageServerToClient.PlayerChooseColor.ToMessageString(), NW_PlayerChosseColor);
    }

    public void Ready()
    {
        LobbyController.lobby.Send("select_color", new { color = GameMaster.color });
    }

    private void NW_PlayerChosseColor(object data)
    {
        var colorData = data as Dictionary<string, object>;
        string sessionId = colorData["sessionId"].ToString();
        string color = colorData["color"].ToString();

        string playerName = LobbyController.state.GetNamePlayerBySessionID(sessionId);
        Color colors = Util.StringToColor(color);
        LobbyUIManager.instance.SetColorPlayer(playerName, colors, true);
    }
}
