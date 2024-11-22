using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Map : NetworkBehaviour
{
    [SerializeField] private string mapFileName = "map"; // Tên file map
    [SerializeField] private GameObject tilePrefab; // Prefab của tile
    [SerializeField] private float tileSpacing = 1.0f; // Khoảng cách giữa các tile
    [SerializeField] private float yPos = 2.6f; // Độ cao của tile khi spawn

    private List<Vector2Int> tilePositions = new List<Vector2Int>(); // Danh sách vị trí tile
    private Dictionary<Vector2Int, int> tileData = new Dictionary<Vector2Int, int>(); // Lưu vị trí và loại tile

    public override void OnStartServer()
    {
        LoadMapFile();
        SpawnTiles();
    }

    // Hàm load file map.txt
    private void LoadMapFile()
    {
        TextAsset mapFile = Resources.Load<TextAsset>(mapFileName);

        if (mapFile == null)
        {
            Debug.LogError($"Map file '{mapFileName}.txt' not found in Resources folder!");
            return;
        }

        string[] lines = mapFile.text.Split('\n'); // Mỗi dòng trong file map.txt

        for (int y = 0; y < lines.Length; y++)
        {
            string[] line = lines[y].Trim().Split(" "); // Tách các giá trị trên mỗi dòng
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == "*") continue; // Dấu '*' nghĩa là không spawn tile

                // Kiểm tra giá trị hợp lệ
                if (int.TryParse(line[x], out int tileType))
                {
                    Vector2Int position = new Vector2Int(x, -y);
                    tilePositions.Add(position);
                    tileData[position] = tileType; 
                }
                else
                {
                    Debug.LogWarning($"Invalid tile type '{line[x]}' at position ({x}, {y}) in map file.");
                }
            }
        }
    }

    // Hàm spawn các tile theo vị trí đã đọc từ map.txt
    private void SpawnTiles()
    {
        foreach (var position in tilePositions)
        {
            if (tileData.TryGetValue(position, out int tileType))
            {
                GameObject tileInstance = Instantiate(tilePrefab, new Vector3(position.x * tileSpacing, yPos, position.y * tileSpacing), Quaternion.identity);

                NetworkServer.Spawn(tileInstance);

                Tile tileComponent = tileInstance.GetComponent<Tile>();
                if (tileComponent != null)
                {
                    tileComponent.SetTileData(tileType); 
                }
            }
        }
    }
}
