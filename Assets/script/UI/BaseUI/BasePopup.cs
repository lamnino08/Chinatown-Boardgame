using UnityEngine;
using DG.Tweening;

public class BasePopup : MonoBehaviour
{
    [Header("Base popup")]
    [SerializeField] protected bool _isOpenOnStart; 
    [SerializeField] protected CanvasGroup canvasGroup; 
    protected float fadeDuration = 0.5f; 
    protected float zoomDuration = 0.5f; 
    protected float startScale = 0.5f; 
    [SerializeField] protected RectTransform rectTransform;

    public virtual void OnStart()
    {
        if (!canvasGroup || !rectTransform)
        {
            canvasGroup = gameObject.GetComponentInChildren<CanvasGroup>();
            rectTransform = gameObject.GetComponentInChildren<RectTransform>();
        }
        gameObject.SetActive(_isOpenOnStart);
    }

    public virtual void Show(float? duration = null)
    {
        rectTransform.localScale = Vector3.one * startScale; 
        canvasGroup.alpha = 0f; 
        canvasGroup.interactable = false; 
        canvasGroup.blocksRaycasts = false;

        gameObject.SetActive(true);

        rectTransform.DOScale(Vector3.one, zoomDuration).SetEase(Ease.OutBack);
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1f, fadeDuration).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                canvasGroup.interactable = true; 
                canvasGroup.blocksRaycasts = true;

                if (duration.HasValue)
                {
                    // Tự động ẩn popup sau thời gian duration
                    Invoke(nameof(Hide), duration.Value);
                }
            });
    }

    public virtual void Hide()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        rectTransform.DOScale(Vector3.one * startScale, zoomDuration).SetEase(Ease.InBack);
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0f, fadeDuration).SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}
