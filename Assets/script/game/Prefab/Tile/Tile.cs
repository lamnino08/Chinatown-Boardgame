using TMPro;
using UnityEngine;

public class Tile : PieceGameObject
{
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private LayerMask markLayer;
    [SerializeField] private HighLight _highlight;

    private int _tile;                // Giá trị của tile
    private int _owner;               // Chủ sở hữu của tile
    private bool _isMarked = false;   // Trạng thái tile có được đánh dấu hay không

    private bool isHighlighting = false;

    public int TileValue => _tile;    // Getter cho tile
    public int Owner => _owner;       // Getter cho owner
    public bool IsMarked => _isMarked; // Getter cho trạng thái đánh dấu

    public void SetOwner(int owner)
    {
        _owner = owner;
        _isMarked = true;
        ChangeMarkedStatus(false, 0);
    }

    public void UnMark(byte color)
    {
        _isMarked = false;
        ChangeMarkedStatus(true, color);
    }

    public void SetTileData(int type)
    {
        _tile = type;
        _isMarked = false;
        UpdateTileNumber();
    }

    private void UpdateTileNumber()
    {
        numberText.text = (_tile + 1).ToString();
    }

    private void ChangeMarkedStatus(bool isMarked, byte color)
    {
        if (isMarked)
        {
            ToggleHighlight(true, color);
        }
        else
        {
            ToggleHighlight(false);
        }
    }

    public override void OnMouseClick()
    {
        GameMaster.gameManager.OnTileClick(this);
    }

    public void ToggleHighlight(bool isHighlight, int color = 6)
    {
        _highlight.ToggleHighlight(isHighlight, color);
    }

    public Mark GetMark()
    {
        Vector3 direction = Vector3.up;

        // Perform the raycast
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 1f, markLayer))
        {
            Mark mark = hit.collider.GetComponent<Mark>();
            return mark;
        }

        Debug.Log("No Mark object found in the upward direction.");
        return null;
    }
}
