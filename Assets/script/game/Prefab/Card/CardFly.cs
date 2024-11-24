using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardFly : MonoBehaviour
{
    private TileCard _card;

    [SerializeField] 
    public TileCard card
    {
        get
        {
            if (_card == null)
            {
                _card = GetComponent<TileCard>(); 
            }
            return _card;
        }
        private set
        {
            _card = value; // Chỉ dùng được nội bộ
        }
    }

    private void Start() 
    {
        if (!_card)
        _card = GetComponent<TileCard>();
    }
    public void StartFlying(
        List<Vector3> controlPoints, 
        float duration ,
        float rotateAfter,
        AnimationCurve easingCurve,
        Transform targetRotationTransform,
        CardStatus nextStatus
    )
    {
        if (_card.status == CardStatus.FLYING) return;

        if (controlPoints == null || controlPoints.Count < 2)
        {
            Debug.LogError("Control points must have at least 2 points!");
            return;
        }

        StartCoroutine(FlyAlongBezierCurve(controlPoints, duration, rotateAfter, easingCurve, targetRotationTransform, nextStatus));
    }

    private IEnumerator FlyAlongBezierCurve(
        List<Vector3> controlPoints, 
        float duration ,
        float rotateAfter,
        AnimationCurve easingCurve,
        Transform targetRotationTransform,
        CardStatus nextStatus
    )
    {
        _card.status = CardStatus.LYING;
        float elapsedTime = 0f;

        bool isRotating = false;
        float rotationElapsedTime = 0f; 
        Quaternion initialRotation = Quaternion.identity;
        Quaternion targetRotation = targetRotationTransform.rotation;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration); 
            float easedT = easingCurve.Evaluate(t);

            Vector3 positionOnCurve = CalculateBezierPoint(easedT, controlPoints);
            transform.position = positionOnCurve;

            // Rotate
            if (elapsedTime < rotateAfter)
            {
                Vector3 nextPosition = CalculateBezierPoint(easingCurve.Evaluate(Mathf.Clamp01(t + 0.01f)), controlPoints);
                transform.LookAt(nextPosition);
            }
            else
            {
                if (!isRotating)
                {
                    initialRotation = transform.rotation;
                    rotationElapsedTime = 0f;
                    isRotating = true;
                }

                rotationElapsedTime += Time.deltaTime;
                float rotationT = Mathf.Clamp01(rotationElapsedTime / (duration - rotateAfter)); 
                float easedRotationT = easingCurve.Evaluate(rotationT);
                transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, easedRotationT);
            }

            yield return null; 
        }

        _card.originalPosition = transform.position;
        _card.status = nextStatus;
    }

    /// <summary>
    /// Tính toán vị trí trên đường cong tại thời điểm t với n điểm kiểm soát
    /// </summary>
    /// <param name="t">Thời gian nội suy (0 <= t <= 1)</param>
    /// <param name="points">Danh sách các điểm kiểm soát</param>
    /// <returns>Vị trí trên đường cong</returns>
    private Vector3 CalculateBezierPoint(float t, List<Vector3> points)
    {
        int n = points.Count - 1; 
        Vector3 result = Vector3.zero;

        for (int i = 0; i <= n; i++)
        {
            // Công thức Bezier tổng quát: B(t) = Σ [ C(n, i) * (1 - t)^(n-i) * t^i * P(i) ]
            float coefficient = BinomialCoefficient(n, i) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i);
            result += coefficient * points[i];
        }

        return result;
    }

    /// <summary>
    /// Tính hệ số tổ hợp: C(n, k) = n! / [k! * (n-k)!]
    /// </summary>
    /// <param name="n">Tổng số phần tử</param>
    /// <param name="k">Số phần tử được chọn</param>
    /// <returns>Giá trị tổ hợp</returns>
    private int BinomialCoefficient(int n, int k)
    {
        int result = 1;
        for (int i = 1; i <= k; i++)
        {
            result *= n--;
            result /= i;
        }
        return result;
    }

    // private void OnDrawGizmos()
    // {
    //     if (controlPoints.Count > 1)
    //     {
    //         Gizmos.color = Color.green;
    //         Vector3 previousPoint = controlPoints[0].position;

    //         for (float t = 0; t <= 1f; t += 0.01f)
    //         {
    //             Vector3 point = CalculateBezierPoint(t, controlPoints);
    //             Gizmos.DrawLine(previousPoint, point);
    //             previousPoint = point;
    //         }
    //     }
    // }
}


    // private IEnumerator FlyAlongBezierCurve(List<Transform> controlPoints, Vector3 finalDirection)
    // {
    //     isFlying = true;
    //     float elapsedTime = 0f;

    //     while (elapsedTime < duration)
    //     {
    //         elapsedTime += Time.deltaTime;

    //         float t = Mathf.Clamp01(elapsedTime / duration); 
    //         float easedT = easingCurve.Evaluate(t);

    //         // Tính toán vị trí hiện tại theo đường cong
    //         Vector3 positionOnCurve = CalculateBezierPoint(easedT, controlPoints);
    //         transform.position = positionOnCurve;

    //         // Đối tượng quay về hướng đường cong
    //         if (t < 1f)
    //         {
    //             Vector3 nextPosition = CalculateBezierPoint(easingCurve.Evaluate(Mathf.Clamp01(t + 0.01f)), controlPoints);
    //                 transform.LookAt(nextPosition);
    //         }
    //         // yield return StartCoroutine(RotateTowardsDirection(finalDirection, 1f));
    //         yield return null; 
    //     }

    //     isFlying = false;
    // }
