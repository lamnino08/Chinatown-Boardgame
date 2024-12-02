using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MarkFly : FlyAbstact
{
    [SerializeField] private AnimationCurve easingCurve;
    [SerializeField] private float duration = 2f;
    [SerializeField] private Rigidbody rigibody;

    public void StartFlying(
        List<Vector3> controlPoints
    )
    {
        if (controlPoints == null || controlPoints.Count < 2)
        {
            Debug.LogError("Control points must have at least 2 points!");
            return;
        }

        rigibody.isKinematic = true;
        StartCoroutine(FlyAlongBezierCurve(controlPoints));
    }

    private IEnumerator FlyAlongBezierCurve(
        List<Vector3> controlPoints
    )
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration); 
            float easedT = easingCurve.Evaluate(t);

            Vector3 positionOnCurve = CalculateBezierPoint(easedT, controlPoints);
            transform.position = positionOnCurve;

            yield return null; 
        }
        rigibody.isKinematic = false;
    }
}
