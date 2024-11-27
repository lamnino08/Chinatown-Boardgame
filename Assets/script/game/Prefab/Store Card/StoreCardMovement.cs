using System.Collections;
using Mirror;
using UnityEngine;

public class StoreCardMovement : NetworkBehaviour
{
    [SerializeField] private AnimationCurve movementCurve; 
    private float moveDuration = 1.5f;

    void Start()
    {
    }

    public void MoveToTarget(Vector3 target)
    {
        RpcMoveToTarget(target);
    }

    [ClientRpc]
    private void RpcMoveToTarget(Vector3 target)
    {
        StartCoroutine(MoveToTargetCoroutine(target));
    }

    private IEnumerator MoveToTargetCoroutine(Vector3 target)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            // Tính toán vị trí mới dựa trên AnimationCurve
            float t = elapsedTime / moveDuration;
            float curveValue = movementCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPosition, target, curveValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo vị trí cuối cùng là target
        transform.position = target;
        Debug.Log("Card has reached its target!");
    }
}
