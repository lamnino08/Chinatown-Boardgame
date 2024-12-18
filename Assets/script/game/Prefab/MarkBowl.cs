using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkBowl : PieceGameObject
{
    [SerializeField] private GameObject markPrefab;               // Prefab của Mark
    [SerializeField] private AnimationCurve easingCurve;          // Đường cong di chuyển
    [SerializeField] private List<Transform> pathMarkFlyTransform = new List<Transform>();

    private List<Vector3> pathMarkFlyVec = new List<Vector3>();   // Danh sách các điểm di chuyển
    public bool isClicked = false;                                // Trạng thái click

    public void SpawnMarks(List<int> tiles, int color, int ownerIndex, string sessionId)
    {
        // Khởi tạo đường đi mặc định nếu chưa có
        if (pathMarkFlyVec.Count == 0)
        {
            pathMarkFlyVec.Add(transform.position);
            pathMarkFlyVec.Add(transform.position + transform.up);
        }

        // Bắt đầu Coroutine để tạo Mark
        StartCoroutine(SpawnMarkCoroutine(tiles, color, ownerIndex, sessionId));
    }

    private IEnumerator SpawnMarkCoroutine(List<int> tiles, int color, int ownerIndex, string sessionId)
    {
        foreach (byte tileIndex in tiles)
        {
            SpawnMark(tileIndex, color, ownerIndex, sessionId);

            yield return new WaitForSeconds(0.2f); // Thời gian giữa mỗi lần spawn
        }
    }

    private void SpawnMark(int tile, int color, int ownerIndex, string sessionId)
    {
        GameObject markObject = Instantiate(markPrefab, transform.position, Quaternion.identity);

        Mark markScript = markObject.GetComponent<Mark>();
        markScript.SetData(ownerIndex, sessionId, color);

        // Di chuyển Mark tới vị trí của Tile
        markScript.MoveToTile(tile);
    }

    public override void OnMouseClick()
    {
        // Gửi sự kiện click cho GameManager
        GameMaster.gameManager.OnBowlMarkClick(this);
    }
}
