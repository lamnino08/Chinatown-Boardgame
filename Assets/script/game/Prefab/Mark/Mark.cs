using UnityEngine;

public enum MarkStatus
{
    FREE,      // Free
    ONTILE,    // Mark is on a tile
    FLYING     // Mark is flying
}

public class Mark : PieceGameObject
{
    private MarkStatus _status;

    public MarkStatus Status
    {
        get
        {
            if (movement.isFlying) return MarkStatus.FLYING;
            return _status;
        }
    }

    [SerializeField] private MarkAppearance appearance;
    [SerializeField] private MarkMovement movement;
    [SerializeField] private LayerMask tileLayer;

    private int _owner;

    // Set ownership and color of the mark
    public void SetData(int ownerIndex, byte color)
    {
        _owner = ownerIndex;
        SetColor(color);
    }

    // Move the mark to a tile
    public void MoveToTile(int tileIndex, byte color)
    {
        Tile tile = Map.instance.GetTile(tileIndex);

        // Unmark the previous tile
        Tile oldTile = GetTileOn();
        if (oldTile != null)
        {
            oldTile.UnMark(color);
        }

        MoveToTilePosition(transform.position, tile);
    }

    // Move the mark to a specific position on the table
    public void MoveToTable(Vector3 position, byte color)
    {
        Tile oldTile = GetTileOn();
        if (oldTile != null)
        {
            oldTile.UnMark(color);
        }

        MoveToTarget(position);
    }

    private void MoveToTilePosition(Vector3 start, Tile tile)
    {
        Vector3 targetPos = tile.transform.position + new Vector3(0, 0.2f, 0);
        movement.StartFlyingSimpleCurve(start, targetPos, 0.7f);
        tile.SetOwner(_owner);

        _status = MarkStatus.ONTILE;
    }

    private void MoveToTarget(Vector3 targetPosition)
    {
        movement.StartFlyingSimpleCurve(transform.position, targetPosition, 0.7f);
        appearance.Highlight(false);
        _status = MarkStatus.FREE;
    }

    private Tile GetTileOn()
    {
        Vector3 direction = Vector3.down;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 1f, tileLayer))
        {
            return hit.collider.GetComponent<Tile>();
        }

        return null;
    }

    // Set mark color
    private void SetColor(byte color)
    {
        appearance.SetColor(color);
    }

    // Highlighting methods
    public void Highlight(bool isHighlight)
    {
        appearance.Highlight(isHighlight);
    }

    public override void OnMouseClick()
    {
        if (Status == MarkStatus.FLYING)
        {
            GamePopupManager.Toast("Mark is flying!");
            return;
        }

        GameMaster.gameManager.OnMarkClick(this);
    }

    public void Click()
    {
        Highlight(true);
    }

    public void UnClick()
    {
        Highlight(false);
    }
}
