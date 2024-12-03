using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyType
{
    BEIZER, // more 3 point
    LERP, // straight 2 point 
    SIMPLE_CURVE // 3 point
}
[RequireComponent(typeof(FlyBeizer),typeof(FlySimpleCurve),typeof(FlyLerp))]
public class TileCardmovement : MonoBehaviour
{
    public bool isFlying 
    {
        get 
        {
            return _flyBeizer.isFlying || _flyBeizer.isFlying || _flySimpleCurve.isFlying;
        }
    }

    [SerializeField] public FlyBeizer _flyBeizer; 
    [SerializeField] public FlyLerp _flyLerp; 
    [SerializeField] public FlySimpleCurve _flySimpleCurve; 

    private void Start() 
    {
        if (!_flyBeizer)
            _flyBeizer = GetComponent<FlyBeizer>(); 
    }

    public void StartFlyingBeizer(
        List<Vector3> controlPoints, 
        float duration ,
        float rotateAfter,
        AnimationCurve easingCurve,
        Transform targetRotationTransform
    )
    {
        _flyBeizer.StartFlying(controlPoints, duration, easingCurve, rotateAfter, targetRotationTransform);    
    }

    public void StartFlyingLerp(
        Vector3 startPosition, Vector3 endPosition, float duration, Quaternion targeRotation
    )
    {
        _flyLerp.StartFlying(startPosition, endPosition, duration, targeRotation);    
    }

    public void StartFlyingLerpSimpleCurve(
        Vector3 startPoint, Vector3 desPoint, float curveInsensity, float duration
    )
    {
        _flySimpleCurve.StarFlying(startPoint, desPoint, curveInsensity, duration);
    }
}
