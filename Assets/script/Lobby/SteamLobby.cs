using System.Net.Mime;
using UnityEngine;
using Steamworks;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance { get; private set; }


    private Callback<LobbyCreated_t> lobbyCreatedCallback;
    private Callback<GameLobbyJoinRequested_t> joinRequestCallback;
    private Callback<LobbyEnter_t> lobbyEnterCallback;

    private NetworkManager _networkManager;
    public TMP_Text toast;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _networkManager = GetComponent<NetworkManager>();
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam is not initialized!");
            return;
        }

        lobbyCreatedCallback = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequestCallback = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        lobbyEnterCallback = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    // Tạo một lobby mới
    public void CreateLobby()
    {
        // SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, _networkManager.maxConnections);
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _networkManager.maxConnections);
        // SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePrivate, MaxLobbyMembers);
    }

    public void JoinLobby(CSteamID lobbyID)
    {
        Debug.Log($"Joining lobby {lobbyID}");
        SteamMatchmaking.JoinLobby(lobbyID);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        toast.text = "okkkeeekekeke";   
        if (callback.m_eResult == EResult.k_EResultOK)
        {
            Debug.Log($"Lobby created successfully! Lobby ID: {callback.m_ulSteamIDLobby}");
            CSteamID lobbyID = new CSteamID(callback.m_ulSteamIDLobby);

            SteamMatchmaking.SetLobbyData(lobbyID, "name", SteamFriends.GetPersonaName());
        }
        else
        {
            Debug.LogError("Failed to create lobby!");
        }
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log($"Join request from lobby {callback.m_steamIDLobby}");
        JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        Debug.Log($"Lobby entered! Lobby ID: {callback.m_ulSteamIDLobby}");
    }
}
