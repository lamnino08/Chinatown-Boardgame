using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardFly : FlyAbstact
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
