using System.Collections;
using UnityEngine;

public class CardRotation : MonoBehaviour
{

    // public void StartFlying(List<Transform> controlPoints)
    [SerializeField] private float jumpHeight = 2f; // Độ cao nhảy
    [SerializeField] private float jumpDuration = 1f; // Thời gian nhảy
    [SerializeField] private Vector3 targetDirection = Vector3.forward; // Hướng mà card sẽ quay mặt về sau khi xoay

    private void Start() {
        StartCardAnimation();
    }

    /// <summary>
    /// Hàm kích hoạt animation cho card
    /// </summary>
    public void StartCardAnimation()
    {
        StartCoroutine(JumpAndRotate());
    }

    private IEnumerator JumpAndRotate()
    {
        Vector3 startPosition = transform.position;

        float elapsedTime = 0f;
       
        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;

           
            float progress = elapsedTime / jumpDuration;
            float verticalOffset = Mathf.Sin(progress * Mathf.PI) * jumpHeight; 
            transform.position = new Vector3(startPosition.x, startPosition.y + verticalOffset, startPosition.z);

            // Xoay theo thời gian
            float rotationAngle = progress * 360f; // Xoay 360 độ
            transform.rotation = Quaternion.Euler(rotationAngle, 0 , 0);

            yield return null; // Đợi 1 frame
        }

        // Phase 2: Quay về hướng xác định
        transform.position = startPosition; // Reset về vị trí ban đầu
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection); // Tính toán góc quay về hướng đích
        float rotationTime = 0.3f; // Thời gian để quay về hướng đích
        elapsedTime = 0f;

        while (elapsedTime < rotationTime)
        {
            elapsedTime += Time.deltaTime;

            // Nội suy góc quay
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / rotationTime);

            yield return null;
        }

        // Đảm bảo card hoàn toàn hướng về hướng đích
        transform.rotation = targetRotation;
    }
}
