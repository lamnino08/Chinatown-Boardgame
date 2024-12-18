using UnityEngine;

public enum StoreCardStatus
{
    FREE,      // Chưa được sử dụng
    ONTILE,    // Đặt trên tile
    FLYING     // Đang di chuyển
}

[RequireComponent(typeof(StoreCardMovement), typeof(StoreCardAppearance))]
public class StoreCard : PieceGameObject
{
    [SerializeField] private StoreCardAppearance appearance;
    [SerializeField] private StoreCardMovement movement;
    [SerializeField] private HighLight _highlight;

    private StoreCardStatus _status;

    public StoreCardStatus Status
    {
        get
        {
            return movement.isFlying ? StoreCardStatus.FLYING : _status;
        }
    }

    public void SetData(int storeCard, int owner, string sessionId)
    {
        this.sessionId = sessionId;
        this._owner = owner;
        SetImage(storeCard);
    }

    public void MoveToTarget(Vector3 targetPosition)
    {
        movement.MoveToTarget(targetPosition);
        _status = StoreCardStatus.FREE;
        ToggleHighlight(false, 0);
    }

    public void ToggleHighlight(bool isHighlight, byte color)
    {
        _highlight.ToggleHighlight(isHighlight, color);
    }

    private void SetImage(int storeCard)
    {
        appearance.SetImage(storeCard);
    }

    public override void OnMouseClick()
    {
        if (IsOwner())
        {
            GamePopupManager.Toast("It's not your card");
            return;
        }

        if (Status == StoreCardStatus.FLYING)
        {
            GamePopupManager.Toast("Card is flying, wait!");
            return;
        }

        GameMaster.gameManager.OnStoreClick(this);
    }
}
