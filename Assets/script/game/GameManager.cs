using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Transform> listCameraTranform;
    [SerializeField] Transform cameraTranform;
    public Mark markClicked = null;
    public StoreCard storeClicked = null;
    public MarkBowl bowClicked = null;
    
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

        // bowClicked.isClicked = false;
        ChanegBowClickedStatus(false);

        markClicked = newMarkClicked;
        newMarkClicked.Click();
    }

    public void OnTileClick(Tile tile)
    {
        if (GameMaster.instance.gamePharse != GamePharse.TRADES) return;

        if (tile.owner == 6) { GamePopupManager.Toast("Free tile"); return; }
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
                markClicked = newMarkClicked;
                newMarkClicked.Click(); // new mark clicked
            }
            return;
        }

        // click to other tile and it is not mark => it to you
        if (markClicked)
        {
            markClicked.CmdMoveToTile(tile.tile);
            markClicked.UnClick();
            markClicked = null;
            return;
        }

        if (IsBowClicked())
        {
            bowClicked.isClicked = false;
            bowClicked.CmdSpawnMark(tile.tile, GameMaster.localPlayer.color, GameMaster.localPlayer.index);
        }
    }

    public void OnBowlMarkClick(MarkBowl markBowlClicked)
    {
        if (GameMaster.instance.gamePharse != GamePharse.TRADES) return;
        if (this.bowClicked == null) this.bowClicked = markBowlClicked;

        this.bowClicked.isClicked = true;
    }

    public void OnStoreClick(StoreCard card)
    {
        if (GameMaster.instance.gamePharse != GamePharse.TRADES) return;

        storeClicked = card;
    }

    public void OnTableClick()
    {
        if (GameMaster.instance.gamePharse != GamePharse.TRADES) return;

        if (markClicked != null || storeClicked != null)
        {
            Camera mainCamera = Camera.main;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPosition = hit.point;

                if (markClicked != null)
                {
                    markClicked.CmdMoveToTable(hitPosition);
                    markClicked = null;
                }

                if (storeClicked != null)
                {
                    storeClicked.CmdMoveToTaget(hitPosition);
                    storeClicked = null;
                }
            }

        } 
    }

    private void ChanegBowClickedStatus(bool isClicked)
    {
        if (bowClicked) bowClicked.isClicked = isClicked;
    }

    private bool IsBowClicked()
    {
        if (bowClicked) return bowClicked.isClicked;
        return false;
    }
}
