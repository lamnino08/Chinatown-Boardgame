using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map instance { get; private set; }
    [SerializeField] private string mapFileName = "map";
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float tileSpacing = 1.0f;
    [SerializeField] private float yPos = 2.6f;

    public readonly Dictionary<int, Tile> tileData = new();

    public  void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        LoadAndSpawnTiles();
    }

    public Tile GetTile(int tile)
    {
        if (tileData.TryGetValue(tile, out Tile tileValue))
        {
            return tileValue;
        }
        Debug.LogWarning($"Tile with ID {tile} not found!");
        return null;
    }

    public void ToggleHightLightTile(int tile, bool isHighlight, int color)
    {
        Tile t = GetTile(tile);
        t.ToggleHighlight(isHighlight, color);
    }


    private void LoadAndSpawnTiles()
    {
        TextAsset mapFile = Resources.Load<TextAsset>(mapFileName);

        if (mapFile == null)
        {
            Debug.LogError($"Map file '{mapFileName}.txt' not found in Resources folder!");
            return;
        }

        string[] lines = mapFile.text.Split('\n'); // Split file into lines
        for (int y = 0; y < lines.Length; y++)
        {
            ParseAndSpawnLine(lines[y], -y);
        }
    }

    private void ParseAndSpawnLine(string line, int row)
    {
        string[] entries = line.Trim().Split(' ');
        for (int x = 0; x < entries.Length; x++)
        {
            if (entries[x] == "*") continue; 
            if (byte.TryParse(entries[x], out byte tileID))
            {
                Vector2Int position = new Vector2Int(x, row);
                SpawnTile(position, tileID);
            }
            else
            {
                Debug.LogWarning($"Invalid tile data '{entries[x]}' at ({x}, {row})");
            }
        }
    }

    private void SpawnTile(Vector2Int position, byte tileID)
    {
        Vector3 worldPosition = new Vector3(position.x * tileSpacing, yPos, position.y * tileSpacing);
        GameObject tileInstance = Instantiate(tilePrefab, worldPosition, Quaternion.identity, transform);

        Tile tileComponent = tileInstance.GetComponent<Tile>();

        tileComponent.SetTileData(tileID-1);
        tileData.Add(tileID-1, tileComponent);
    }

}    
