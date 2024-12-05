using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Map : NetworkBehaviour
{
    public static Map instance { get; private set; }
    [SerializeField] private string mapFileName = "map";
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float tileSpacing = 1.0f;
    [SerializeField] private float yPos = 2.6f;

    public readonly Dictionary<int, Tile> tileData = new();

    public override void OnStartServer()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        LoadAndSpawnTiles();

        // EventBus.Subscribe<SpawnMarkEvent>()
    }

    public Tile GetTile(byte tile)
    {
        if (tileData.TryGetValue(tile, out Tile tileValue))
        {
            return tileValue;
        }
        Debug.LogWarning($"Tile with ID {tile} not found!");
        return null;
    }

    [Server]
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

    [Server]
    private void ParseAndSpawnLine(string line, int row)
    {
        string[] entries = line.Trim().Split(' '); // Split the line into entries
        for (int x = 0; x < entries.Length; x++)
        {
            if (entries[x] == "*") continue; // Skip unspawnable tiles
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

    [Server]
    private void SpawnTile(Vector2Int position, byte tileID)
    {
        Vector3 worldPosition = new Vector3(position.x * tileSpacing, yPos, position.y * tileSpacing);
        GameObject tileInstance = Instantiate(tilePrefab, worldPosition, Quaternion.identity, transform);

        NetworkServer.Spawn(tileInstance);

        Tile tileComponent = tileInstance.GetComponent<Tile>();
        if (tileComponent != null)
        {
            tileComponent.SetTileData(tileID);
            tileData[tileID-1] = tileComponent; 
        }
        else
        {
            Debug.LogWarning($"Tile prefab is missing the Tile component at position {position}");
        }
    }
}    
