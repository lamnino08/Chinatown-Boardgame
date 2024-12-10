using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlySimpleCurve : FlyAbstact
{
    public bool isFlying = false; 
    public AnimationCurve easingCurve; 


    public void StarFlying (
        Vector3 startPoint, Vector3 desPoint, float curveInsensity, float duration
    )
    {
        List<Vector3> curves = new List<Vector3>();
        curves.Add(startPoint);
        curves.Add(startPoint + new Vector3(0, curveInsensity, 0));
        curves.Add(desPoint + new Vector3(0, 0.2f, 0));

        StartCoroutine(FlySimpleBeizer(curves, duration));
    }

    /// <summary>
    /// Starts the flight from point A to point B with an animation curve to control the movement.
    /// </summary>
    /// <param name="startPosition">Starting position (point A)</param>
    /// <param name="endPosition">Ending position (point B)</param>
    /// <param name="duration">Total duration of the flight</param>
     private IEnumerator FlySimpleBeizer(
        List<Vector3> controlPoints,
        float duration
    )
    {
        if (rigidbody) rigidbody.isKinematic = true;
        
        isFlying = true;
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
        
        isFlying = false;
        if (rigidbody) GetComponent<Rigidbody>().isKinematic = false;
    }
}
