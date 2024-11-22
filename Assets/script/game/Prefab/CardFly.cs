using DG.Tweening;
using UnityEngine;

public class CardFly : MonoBehaviour
{
    public RectTransform uiCard; // Tham chiếu tới prefab 2D

    private Canvas canvas;

    void Start()
    {
        // Tìm canvas trong scene (cho UI)
        canvas = FindObjectOfType<Canvas>();
        FlyToUI();
    }

    public void FlyToUI()
    {
        // 1. Hiệu ứng bay lên trong không gian 3D
        transform.DOMove(transform.position + Vector3.up * 2, 0.5f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

                uiCard = Instantiate(Resources.Load<RectTransform>("CardUI"), canvas.transform);
                uiCard.position = screenPosition;
                gameObject.SetActive(false);
            });
    }
}
