using System.Collections;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(FlySimpleCurve))]
public class StoreCardMovement : MonoBehaviour
{
    [SerializeField] private FlySimpleCurve _flyCurve;
    [SerializeField] private Rigidbody rigibody;
    private float duration = 1.5f;

    public void MoveToTarget(Vector3 target)
    {
        rigibody.isKinematic = true;
        _flyCurve.StarFlying(transform.position, target, duration,  0.7f);
        StartCoroutine(TurnRigibody()); // turn on rigibody when reach target
    }

    private IEnumerator TurnRigibody()
    {
        yield return new WaitForSeconds(duration);
        rigibody.isKinematic = false;
    }
}
