using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FlySimpleCurve))]
public class MarkMovement : FlyAbstact
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private Rigidbody rigibody;
    [SerializeField] private FlySimpleCurve _flySimpleCurve;

    public bool isFlying 
    {
        get { return _flySimpleCurve.isFlying;}
    }

    public void StartFlyingSimpleCurve(Vector3 startPoint, Vector3 endPoint, float insensity)
    {
        rigibody.isKinematic = true;
        _flySimpleCurve.StarFlying(startPoint, endPoint, insensity, duration);
        StartCoroutine(TurnRigibody());
    }

    private IEnumerator TurnRigibody()
    {
        yield return new WaitForSeconds(duration);
        rigibody.isKinematic = false;
    }
}
