using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    WAITING,           // Trạng thái chờ
    TILE_CARD_DEAL,    // Phát thẻ bài (Tile Card Deal)
    NEGOTIATE,         // Đàm phán
    BUILD_STORE,       // Xây cửa hàng
    EARN               // Kiếm tiền
}


//Start in lobby room
public class GameMaster : MonoBehaviour
{
    public static GameMaster instance { get; private set; }
    public static GameManager gameManager;
    public DeskCard deskCard;
    public static GamePhase GamePhase 
    {
        get { return RoomController.state.GetGamePhase();}
    }


    public static string PlayerName = "";
    public static int color = -1;
    public static string sessionId = "";
    public static int index = -1;

    public static PlayerManager localPlayer { get; private set; } /// bo
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        // EventBus.Subscribe<StartGameEvent>(OnStartGame);
        // EventBus.Subscribe<EndDealTileCardPharse>(OnEndDealTileCardPharse);
    }

    public static bool Ishost()
    {
        return index == 0;
    }

    private void OnStartGame(StartGameEvent data)
    {
        if (localPlayer.isHost == true)
        {
            localPlayer.CmdSpawnPlayerSlot();
        }
    }
}
