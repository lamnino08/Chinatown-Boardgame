using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Transform> listCameraTranform;
    [SerializeField] Transform cameraTranform;
    public Mark markClicked = null;
    
    void Start()
    {
        GameMaster.gameManager = this;
        SetView();
        EventBus.Notificate<StartGameEvent>(new StartGameEvent()); // Game Manager, GameUIManager
    }

    public void SetView()
    {
        int localPlayerIndex = GameMaster.localPlayer.index;
        cameraTranform.position = listCameraTranform[localPlayerIndex].position;
        cameraTranform.rotation = listCameraTranform[localPlayerIndex].rotation;
    }

    public void OnMarkClick(Mark newMarkClicked)
    {
        if (GameMaster.instance.gamePharse != GamePharse.TRADES) return;

        if (markClicked != null)
        {
            markClicked.UnClick();
        }

        markClicked = newMarkClicked;
    }

    public void OnTileClick(Tile tile)
    {
        if (GameMaster.instance.gamePharse != GamePharse.TRADES) return;

        if (markClicked != null)
        {
            if (tile.owner == 6) { GamePopupManager.Toast("This is free tile"); return; }
            if (tile.isMarked) 
            { 
                if (tile.isMarked && tile.owner == GameMaster.localPlayer.index)
                {
                    Mark newMarkClicked = tile.GetMark();
                }
                GamePopupManager.Toast("this tile is marked"); return; 
            }

            markClicked.MoveToTile(markClicked.transform.position, tile.transform.position);
            // tile.SetOwner();
        }

        markClicked = null;
    }

    public void OnClickTable()
    {
        if (GameMaster.instance.gamePharse != GamePharse.TRADES) return;

        if (markClicked != null)
        {
            markClicked.UnClick();
        } 

        markClicked = null;
    }
}
