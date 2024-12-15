using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance { get; private set; }
    [SerializeField] private MenuUIManager menuUIManager;
    private void Start()
    {
        GetAllRoom();
    }

    public async void GetAllRoom()
    {
        try
        {
            List<LobbyServer> rooms = await APIUtil.GetAllRoom();
            menuUIManager.SetRoomData(rooms);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to fetch rooms: {e.Message}");
        }
    }
}
