using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] List<Transform> listCameraTranform;
    [SerializeField] Transform cameraTranform;
    
    void Start()
    {
        GameMaster.gameManager = this;
        SetView();

        Debug.Log(GameMaster.localPlayer.isHost);
        if (GameMaster.localPlayer.isHost == true)
        {
            GameMaster.localPlayer.CmdSpawnPlayerSlot();
        }
    }

    public void SetView()
    {
        int localPlayerIndex = GameMaster.localPlayer.index;
        cameraTranform.position = listCameraTranform[localPlayerIndex].position;
        cameraTranform.rotation = listCameraTranform[localPlayerIndex].rotation;
    }
}
