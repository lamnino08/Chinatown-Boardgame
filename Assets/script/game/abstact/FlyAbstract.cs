using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class FlyAbstact : MonoBehaviour
{
    /// <summary>
    /// Tính toán vị trí trên đường cong tại thời điểm t với n điểm kiểm soát
    /// </summary>
    /// <param name="t">Thời gian nội suy (0 <= t <= 1)</param>
    /// <param name="points">Danh sách các điểm kiểm soát</param>
    /// <returns>Vị trí trên đường cong</returns>
    protected Vector3 CalculateBezierPoint(float t, List<Vector3> points)
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
    protected int BinomialCoefficient(int n, int k)
    {
        int result = 1;
        for (int i = 1; i <= k; i++)
        {
            result *= n--;
            result /= i;
        }
        return result;
    }
}
