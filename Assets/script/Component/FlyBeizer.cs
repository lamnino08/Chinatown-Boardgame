using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBeizer : FlyAbstact
{
    public bool isFlying = false;
    public AnimationCurve easingCurve;

    /// <summary>
    /// Starts the process of flying an object along a Bezier curve, with control points, duration, easing curve, and rotation after a specific time.
    /// </summary>
    /// <param name="controlPoints">A list of control points defining the Bezier curve. At least two points are required to compute the curve.</param>
    /// <param name="duration">The total time the object will take to travel along the curve.</param>
    /// <param name="easingCurve">An animation curve that defines the easing function to smooth the movement over time.</param>
    /// <param name="rotateAfter">The time (in seconds) after which the object will start rotating.</param>
    /// <param name="targetRotationTransform">The target transform that the object will rotate towards after the `rotateAfter` time has passed.</param>
    public void StartFlying(
        List<Vector3> controlPoints, 
        float duration ,
        AnimationCurve easingCurve,
        float rotateAfter,
        Transform targetRotationTransform
    )
    {
        if (controlPoints == null || controlPoints.Count < 2)
        {
            Debug.LogError("Control points must have at least 2 points!");
            return;
        }

        StartCoroutine(FlyAlongBezierCurve(controlPoints, duration, easingCurve, rotateAfter,  targetRotationTransform));
    }

    private IEnumerator FlyAlongBezierCurve(
        List<Vector3> controlPoints, 
        float duration ,
        AnimationCurve easingCurve,
        float rotateAfter,
        Transform targetRotationTransform
    )
    {
        isFlying = true;
        if (GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().isKinematic = true;

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

        isFlying = false;
        if (GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().isKinematic = false;
    }
}
