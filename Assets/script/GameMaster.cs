using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Start in lobby room
public class GameMaster : MonoBehaviour
{
    public static GameMaster instance { get; private set; }
    public static GameManager gameManager;
    public DeskCard deskCard;

    public static PlayerManager localPlayer { get; private set; }

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

    /// <summary>
    /// Sets the local player reference.
    /// </summary>
    /// <param name="player">The PlayerManager instance for the local player.</param>
    public void SetLocalPlayer(PlayerManager player)
    {
        localPlayer = player;
    }
}
