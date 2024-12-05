// using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public enum CardStatus
{
    FLYING,
    LYING,
    CHOSSING,
    CHOSSEN,
}

[RequireComponent(typeof(TileCardmovement))]
public class TileCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private CardStatus _status = CardStatus.LYING;
    public CardStatus status 
    {
        get 
        { 
            if (_cardMovement.isFlying) return CardStatus.FLYING;
            return _status;
        }
        set
        {
            _status = value;
        }
    } 

    [SerializeField] private CardAppearance _appearance;

    [Header("Poperty animate when chossing")]
    [SerializeField] private float moveDistance = .03f; 
    [SerializeField] private float moveDuration = 0.5f; 

    public Vector3 originalPosition = Vector3.zero; 
    private byte _number;
    public byte number => _number;

    [SerializeField] private TileCardmovement _cardMovement;

    void Start()
    {
        EventBus.Subscribe<EndDealTileCardPharse>(OnReturnToDeskHole);
    }

    public void SetNumber(byte number)
    {
        this._number = number;
        _appearance.SetNumber(number+1);
    }

    public void FlyToPlayerView(
        List<Vector3> controlPoints, 
        float duration ,
        float rotateAfter,
        AnimationCurve easingCurve,
        Transform targetRotationTransform
    )
    {
        _cardMovement.StartFlyingBeizer(controlPoints, duration, rotateAfter, easingCurve, targetRotationTransform);
        _status = CardStatus.CHOSSING;
    }

    public void FlyReturnToDeskCard(
        List<Vector3> controlPoints, 
        float duration ,
        float rotateAfter,
        AnimationCurve easingCurve,
        Transform targetRotationTransform
    )
    {
        _cardMovement.StartFlyingBeizer(controlPoints, duration, rotateAfter, easingCurve, targetRotationTransform);
        _status = CardStatus.LYING;
    }

    private void OnReturnToDeskHole(EndDealTileCardPharse data)
    {
        // end of Deal tile card phares, all tile card will return to the hole card
        Transform target = data.deskHoleTranform;
        FlyToHoleCard(transform.position, target.position, 1.5f, target.rotation);
    }

    public void FlyToHoleCard(
        Vector3 startPosition, Vector3 endPosition, float duration, Quaternion targetRotation
    )
    {
        _cardMovement.StartFlyingLerp(startPosition, endPosition, duration, targetRotation);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (status == CardStatus.CHOSSING)
        {
            if (originalPosition == Vector3.zero) originalPosition = transform.position;
            Vector3 targetPos = originalPosition + transform.forward * -moveDistance;

            transform.DOMove(targetPos, moveDuration).SetEase(Ease.InQuad);
            // PlayerSlot.localPlayerSlot.HightLightTile(_number, true);
            EventBus.Notificate(new OnHighlightTile(_number, true));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (status == CardStatus.CHOSSING)
        {
            transform.DOMove(originalPosition, moveDuration).SetEase(Ease.OutQuad);
            EventBus.Notificate(new OnHighlightTile(_number, false));

        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (status == CardStatus.CHOSSEN)
        {
            status = CardStatus.CHOSSING;
            return; 
        } 

        if (status == CardStatus.CHOSSING)
        {
            if (DeskCard.tileCardChose.IsEnoughCardChose())
            {
                GamePopupManager.Toast("Enoguh");
                return;
            } 

            status = CardStatus.CHOSSEN;
            return;
        }
    }
}
