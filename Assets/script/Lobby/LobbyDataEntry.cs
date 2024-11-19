using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
// using SteamWork;
using Steamworks;

public class LobbyDataEntry : MonoBehaviour
{
    public CSteamID lobbyID;
    public string lobbyName;
    public Text lobbyNameText;

    public void SetLobbyData()
    {
        lobbyNameText.text = lobbyName == ""? "Empty" : lobbyName;
    } 

    public void JoinLobby() 
    {
        SteamLobby.Instance.JoinLobby(lobbyID);
    }
}
