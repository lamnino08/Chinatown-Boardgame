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
        newMarkClicked.Click();
    }

    public void OnTileClick(Tile tile)
    {
        if (GameMaster.instance.gamePharse != GamePharse.TRADES) return;

        if (tile.owner == 6) { GamePopupManager.Toast("free tile"); return; }
        if (tile.isMarked) 
        { 
            if (tile.owner != GameMaster.localPlayer.index) return;

            // Click to tile of your have mark
            if (markClicked != null)
            {
                markClicked.UnClick(); // Unclick old mark clicked
                markClicked = null;
            }

            Mark newMarkClicked = tile.GetMark();
            // GamePopupManager.Toast($"{newMarkClicked}");
            if (newMarkClicked != null)
            {
                GamePopupManager.Toast($"{newMarkClicked.name}");
                markClicked = newMarkClicked;
                newMarkClicked.Click(); // new mark clicked
            }
            return;
        }

        // click to other tile and it is not mark => it to you
        markClicked.CmdMoveToTile(tile.tile);
    }

    public void OnTableClick()
    {
        if (GameMaster.instance.gamePharse != GamePharse.TRADES) return;

        if (markClicked != null)
        {
            Camera mainCamera = Camera.main;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPosition = hit.point;

                // Xử lý khi click vào bề mặt
                Debug.Log("Hit Position: " + hitPosition);

                markClicked.CmdMoveToTable(hitPosition);
            }

            markClicked = null;
        } 
    }
}
