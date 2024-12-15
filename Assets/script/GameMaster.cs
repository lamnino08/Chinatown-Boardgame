using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePharse
{
    DEAL_TILECARD,
    DRAW_STORECARD,
    TRADES,
    PLACE_SHOPTILE,
    EARN_INCOME,
    NEXTYEAR
}

//Start in lobby room
public class GameMaster : MonoBehaviour
{
    public static GameMaster instance { get; private set; }
    public static GameManager gameManager;
    public List<PlayerData> players = new List<PlayerData>();
    public DeskCard deskCard;
    public GamePharse gamePharse { get; private set; }


    public static string PlayerName = "";
    public static string color = "";

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
        EventBus.Subscribe<StartGameEvent>(OnStartGame);
        EventBus.Subscribe<EndDealTileCardPharse>(OnEndDealTileCardPharse);
    }

    /// <summary>
    /// Sets the local player reference.
    /// </summary>
    /// <param name="player">The PlayerManager instance for the local player.</param>
    public void SetLocalPlayer(PlayerManager player)
    {
        localPlayer = player;
    }

    private void OnStartGame(StartGameEvent data)
    {
        if (localPlayer.isHost == true)
        {
            localPlayer.CmdSpawnPlayerSlot();
        }
    }

    private void OnEndDealTileCardPharse(EndDealTileCardPharse data)
    {
        gamePharse = GamePharse.TRADES;
    }
}
