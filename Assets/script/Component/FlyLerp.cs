using UnityEngine;
using DG.Tweening; // Make sure DOTween is installed

public class FlyLerp : FlyAbstact
{
    public bool isFlying = false;
    public AnimationCurve easingCurve;

    /// <summary>
    /// Starts the flight from point A to point B with an animation curve to control the movement.
    /// Optionally, rotates the object to face a target rotation during the flight.
    /// </summary>
    /// <param name="startPosition">Starting position (point A)</param>
    /// <param name="endPosition">Ending position (point B)</param>
    /// <param name="duration">Total duration of the flight</param>
    /// <param name="targetRotation">Optional target rotation for the object (can be null for no rotation)</param>
    public void StartFlying(Vector3 startPosition, Vector3 endPosition, float duration, Quaternion? targetRotation = null)
    {
        if (isFlying) return;

        if (rigidbody) rigidbody.isKinematic = true;

        isFlying = true;

        // Set the initial position
        transform.position = startPosition;

        // Set up the movement animation using DOTween
        Tween moveTween = transform.DOMove(endPosition, duration)
            .SetEase(easingCurve)
            .OnKill(() => isFlying = false);

        if (targetRotation.HasValue)
        {
            // Use DORotate to smoothly rotate the object to the target rotation
            Tween rotateTween = transform.DORotateQuaternion(targetRotation.Value, duration)
                .SetEase(Ease.Linear);
        }

        isFlying = false;
        if (rigidbody) rigidbody.isKinematic = false;
    }
}
