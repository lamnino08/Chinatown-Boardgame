using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BasePopup : MonoBehaviour
{
    [SerializeField] protected Image popupImage;
    [SerializeField] protected float fadeDuration = 0.5f; 

    protected virtual void Start() {
        if (popupImage == null)
        {
            popupImage = gameObject.GetComponent<Image>();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);

        popupImage.DOFade(1f, fadeDuration).SetEase(Ease.OutQuad);
    }

    public void Hide()
    {
        popupImage.DOFade(0f, fadeDuration).SetEase(Ease.InQuad).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
