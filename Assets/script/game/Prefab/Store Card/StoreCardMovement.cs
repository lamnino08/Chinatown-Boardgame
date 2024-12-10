using System.Collections;
using Mirror;
using UnityEngine;


[RequireComponent(typeof(FlySimpleCurve))]
public class StoreCardMovement : MonoBehaviour
{
    [SerializeField] private FlySimpleCurve _flyCurve;
    private float duration = 1.5f;
    public bool isFlying 
    {
        get { return _flyCurve.isFlying;}
    }

    public void MoveToTarget(Vector3 target)
    {
        _flyCurve.StarFlying(transform.position, target, duration,  0.7f);
    }
}