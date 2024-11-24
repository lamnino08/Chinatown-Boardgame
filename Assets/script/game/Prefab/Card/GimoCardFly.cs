using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class GimoCardFly : MonoBehaviour
{
    public List<Transform> controlPoints;
    private List<Vector3> controlPointsVec = new List<Vector3>();
    private void Start() 
    {
        foreach(Transform controlPoint in controlPoints)
        {
            controlPointsVec.Add(controlPoint.position);
        }
    }

    private void Update() {
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

    private void OnDrawGizmos()
    {
        if (controlPointsVec.Count > 1)
        {
            Gizmos.color = Color.green;
            Vector3 previousPoint = controlPoints[0].position;

            for (float t = 0; t <= 1f; t += 0.01f)
            {
                Vector3 point = CalculateBezierPoint(t, controlPointsVec);
                Gizmos.DrawLine(previousPoint, point);
                previousPoint = point;
            }
        }
    }
}
