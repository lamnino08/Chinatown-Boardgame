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

    public void SpawnMarks(byte[] tiles, byte color, int ownerIndex)
    {
        // Khởi tạo đường đi mặc định nếu chưa có
        if (pathMarkFlyVec.Count == 0)
        {
            pathMarkFlyVec.Add(transform.position);
            pathMarkFlyVec.Add(transform.position + transform.up);
        }

        // Bắt đầu Coroutine để tạo Mark
        StartCoroutine(SpawnMarkCoroutine(tiles, color, ownerIndex));
    }

    private IEnumerator SpawnMarkCoroutine(byte[] tiles, byte color, int ownerIndex)
    {
        foreach (byte tileIndex in tiles)
        {
            // Lấy tile tương ứng từ Map
            Tile tile = Map.instance.GetTile(tileIndex);
            if (tile != null)
            {
                // Set owner của tile
                tile.SetOwner(ownerIndex);

                // Tạo Mark và di chuyển tới Tile
                SpawnMark(tile, color, ownerIndex);
            }

            yield return new WaitForSeconds(0.2f); // Thời gian giữa mỗi lần spawn
        }
    }

    private void SpawnMark(Tile tile, byte color, int ownerIndex)
    {
        // Tạo một Mark mới
        GameObject markObject = Instantiate(markPrefab, transform.position, Quaternion.identity);

        // Gán dữ liệu cho Mark
        Mark markScript = markObject.GetComponent<Mark>();
        markScript.SetData(ownerIndex, color);

        // Di chuyển Mark tới vị trí của Tile
        // markScript.MoveToTile(transform.position, tile);
    }

    public override void OnMouseClick()
    {
        // Gửi sự kiện click cho GameManager
        GameMaster.gameManager.OnBowlMarkClick(this);
    }
}
