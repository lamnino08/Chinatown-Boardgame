using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum CardStatus
{
    FLYING,
    LYING,
    CHOSSING,
    CHOSSEN,
}

public class TileCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public CardStatus status = CardStatus.LYING; 
    [SerializeField] private CardAppearance _appearance;

    [Header("Poperty animate when chossing")]
    [SerializeField] private float moveDistance = .03f; 
    [SerializeField] private float moveDuration = 0.5f; 
    private bool isHovering = false; 

    public Vector3 originalPosition; // To animation when hover
    private byte _number;
    public byte number => _number;

    private CardFly _cardFly;
    public CardFly cardFly 
    {
        get
        {
            if (_cardFly == null)
            {
                _cardFly = GetComponent<CardFly>(); 
            }
            return _cardFly;
        }
        private set
        {
            _cardFly = value; // Chỉ dùng được nội bộ
        }
    }

    public void SetNumber(byte number)
    {
        this._number = number;
        _appearance.SetNumber(number+1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (status == CardStatus.CHOSSING)
        {
            // _appearance.Hover(true);
            Vector3 targetPos = originalPosition + transform.forward * -moveDistance;

            transform.DOMove(targetPos, moveDuration).SetEase(Ease.InQuad);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (status == CardStatus.CHOSSING)
        {
            // _appearance.Hover(false);
            transform.DOMove(originalPosition, moveDuration).SetEase(Ease.OutQuad);
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (status == CardStatus.CHOSSEN)
        {
            status = CardStatus.CHOSSING;
        } else
        if (status == CardStatus.CHOSSING)
        {
            if (!DeskCard.tileCardChose.IsEnoughCardChose())
            {
                status = CardStatus.CHOSSEN;
            } else
            {
                GamePopupManager.Toast("Enoguh");
            }
        }
    }
}
