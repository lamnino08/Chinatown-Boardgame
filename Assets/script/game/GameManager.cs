using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Camera")]
    [SerializeField] private List<Transform> cameraPositions;
    [SerializeField] private Transform cameraTransform;

    [Header("Player Slot")]
    [SerializeField] private GameObject playerSlotPrefab;
    [SerializeField] private List<Transform> playerSlotPositions;

    private readonly List<GameObject> spawnedPlayerSlots = new List<GameObject>();

    private Mark selectedMark = null;
    private StoreCard selectedStore = null;
    private MarkBowl selectedBowl = null;

    private void Awake()
    {
        instance = this;
        EventBus.Subscribe<JoinRoomEvent>(SetView);
    }

    public void SetView(JoinRoomEvent data)
    {
        cameraTransform.position = cameraPositions[data.index].position;
        cameraTransform.rotation = cameraPositions[data.index].rotation;
    }

    public void SpawnPlayerSlot(Player player)
    {
        int playerIndex = player.index;

        GameObject playerSlot = Instantiate(playerSlotPrefab, playerSlotPositions[playerIndex].position, playerSlotPositions[playerIndex].rotation);
        PlayerSlot playerSlotScript = playerSlot.GetComponent<PlayerSlot>();
        spawnedPlayerSlots.Add(playerSlot);
        playerSlotScript.SetData(player);
    }

    public void NewYear()
    {
        RoomController.room.Send(MessageClientToServerGame.new_year.ToString());
    }

    public void OnMarkClick(Mark newMark)
    {
        if (GameMaster.GamePhase != GamePhase.NEGOTIATE) return;

        DeselectMark();
        DeselectBowl();

        selectedMark = newMark;
        selectedMark.Click();
    }

    public void OnTileClick(Tile tile)
    {
        if (GameMaster.GamePhase != GamePhase.NEGOTIATE) return;

        // if (tile.owner == 6)
        // {
        //     GamePopupManager.Toast("Free tile");
        //     return;
        // }

        HandleTileClick(tile);
    }

    public void OnBowlMarkClick(MarkBowl markBowl)
    {
        if (GameMaster.GamePhase != GamePhase.NEGOTIATE) return;

        selectedBowl = markBowl;
        selectedBowl.isClicked = true;
    }

    public void OnStoreClick(StoreCard card)
    {
        if (GameMaster.GamePhase != GamePhase.NEGOTIATE) return;

        selectedStore = card;
        // card.CmdHighlgith(true, GameMaster.localPlayer.color);
    }

    public void OnTableClick()
    {
        if (GameMaster.GamePhase != GamePhase.NEGOTIATE) return;

        if (selectedMark == null && selectedStore == null) return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit))
        {
            Vector3 targetPosition = hit.point;

            MoveSelectedObjectsToTable(targetPosition);
        }
    }

    private void MoveSelectedObjectsToTable(Vector3 position)
    {
        if (selectedMark != null)
        {
            // selectedMark.CmdMoveToTable(position, GameMaster.localPlayer.color);
            selectedMark = null;
        }

        if (selectedStore != null)
        {
            // selectedStore.CmdMoveToTaget(position);
            // selectedStore.CmdHighlgith(false, 0);
            selectedStore = null;
        }
    }

    private void HandleTileClick(Tile tile)
    {
        // if (tile.isMarked && tile.owner == GameMaster.localPlayer.index)
        // {
        //     SelectNewMark(tile);
        // }
        // else if (selectedMark != null)
        // {
        //     selectedMark.CmdMoveToTile(tile.tile, GameMaster.localPlayer.color);
        //     DeselectMark();
        // }
        // else if (IsBowlSelected())
        // {
        //     selectedBowl.isClicked = false;
        //     selectedBowl.CmdSpawnMark(tile.tile, GameMaster.localPlayer.color, GameMaster.localPlayer.index);
        // }
    }

    private void SelectNewMark(Tile tile)
    {
        DeselectMark();

        var newMark = tile.GetMark();
        if (newMark != null)
        {
            selectedMark = newMark;
            newMark.Click();
        }
    }

    private void DeselectMark()
    {
        selectedMark?.UnClick();
        selectedMark = null;
    }

    private void DeselectBowl()
    {
        if (selectedBowl != null) selectedBowl.isClicked = false;
    }

    private bool IsBowlSelected()
    {
        return selectedBowl?.isClicked == true;
    }
}
