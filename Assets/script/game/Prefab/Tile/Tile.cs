using TMPro;
using UnityEngine;
using Mirror;

public class Tile : NetworkBehaviour
{
    [SerializeField] TMP_Text numberText;
    [SyncVar(hook = nameof(OnTileChanged))]
    public int tile;

    [Server]
    public void SetTileData(int type)
    {
        tile = type;
    }

    private void OnTileChanged(int oldValue, int newValue)
    {
        numberText.text = newValue.ToString();
    }

    [Server]
    public void HightLight(NetworkConnectionToClient conn, bool isHighlight)
    {
        RpcHighLightTile(conn, isHighlight);
    }
    
    [TargetRpc]
    private void RpcHighLightTile(NetworkConnectionToClient conn, bool isHighlight)
    {
        GetComponent<HighLight>().ToggleHighlight(isHighlight);
    }
}
