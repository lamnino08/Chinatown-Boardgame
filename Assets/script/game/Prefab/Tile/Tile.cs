using TMPro;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

public class Tile : PieceGameObject
{
    [SerializeField] TMP_Text numberText;
    [SerializeField] private LayerMask markLayer;

    [SyncVar(hook = nameof(OnTileChanged))]
    public int tile;
    [SyncVar] private int _owner;  public int owner => _owner;
    [SyncVar] private bool _isMarked = false; public bool isMarked => _isMarked;
 
    private bool isHighlighting = false;

    public override void OnStartClient()
    {
        EventBus.Subscribe<OnHighlightTile>(Highlight);
        EventBus.Subscribe<EndDealTileCardPharse>(OffHighlight);

        // gameObject.layer = LayerMask.NameToLayer("Tile");
    }

    public override void OnStartServer()
    {
        _owner = 6; // all tile has none owner
    }

#region Server
    [Server]
    public void SetOwner(int owner)
    {
        this._owner = owner;
        _isMarked = true;
    }

    [Server]
    public void SetTileData(int type)
    {
        tile = type;
        _isMarked = false;
    }

    private void OnTileChanged(int oldValue, int newValue)
    {
        numberText.text = newValue.ToString();
    }
#endregion

#region  Client
    public override void OnMouseClick()
    {
        GamePopupManager.Toast($"Tile {tile} click");
        if (GameMaster.instance.gamePharse != GamePharse.TRADES) return; 

        GameMaster.gameManager.OnTileClick(this);
    }

    public Mark GetMark()
    {
        Vector3 direction = Vector3.up;

        // Perform the raycast
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 1f, markLayer))
        {
            Mark mark = hit.collider.GetComponent<Mark>();
            return mark; // Return the Mark object if found
        }

        Debug.Log("No Mark object found in the upward direction.");
        return null;
    }

    [Client]
    private void Highlight(OnHighlightTile data)
    {
        if (tile == data.tile)
        {
            isHighlighting = data.isHighlight;
            GetComponent<HighLight>().ToggleHighlight(data.isHighlight);
        }
    }

    [Client]
    private void OffHighlight(EndDealTileCardPharse data)
    {
        if (isHighlighting)
        {
            GetComponent<HighLight>().ToggleHighlight(false);
        }
    }   
#endregion Client
}
