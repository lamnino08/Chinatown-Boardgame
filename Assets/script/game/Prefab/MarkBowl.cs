using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MarkBowl : PieceGameObject
{
    [SerializeField] private GameObject markPref;
    [SerializeField] private AnimationCurve easeingCurve;
    [SerializeField] private List<Transform> pathMarkFlyTransform = new List<Transform>();
    public bool isClicked = false;

    private List<Vector3> pathMarkFlyVec = new List<Vector3>();

#region Command
    public void CmdSpawnMark(int tileIndex, byte color, int ownerIndex)
    {
        Tile tile = Map.instance.GetTile(tileIndex);
        SpawnMark(tile, color, connectionToClient, ownerIndex);
    }
#endregion Command

#region  ChanegBowClickedStatus
    [Server]
    public void SpawnMarks(byte[] tiles, byte color, int index)
    {
        if (pathMarkFlyVec.Count == 0)
        {
            pathMarkFlyVec.Add(transform.position);
            pathMarkFlyVec.Add(transform.position + transform.up);
        }
        StartCoroutine(SpawnMarkCoroutine(tiles, color, index));
    }
    [Server]
    private IEnumerator SpawnMarkCoroutine(byte[] tiles, byte color, int index)
    {
        List<NetworkConnection> connections = RoomServerManager.instance.playerConnections;
        for(int i =0; i < tiles.Length; i++)
        {
            // Set tile 
            Tile tile = Map.instance.GetTile(tiles[i]);
            tile.SetOwner(index);

            // Spawn mark
            SpawnMark(tile, color, connections[index], index);

            yield return new WaitForSeconds(0.2f);
        }
    }

    [Server]
    private void SpawnMark(Tile tile, byte color, NetworkConnection connection, int ownerIndex)
    {

        GameObject markObject = Instantiate(markPref, transform.position, Quaternion.identity);
        NetworkServer.Spawn(markObject, connection);

        Mark markscript = markObject.GetComponent<Mark>();
        markscript.SetData(ownerIndex, color);

        markscript.SVMoveToTile(transform.position, tile);
    }
#endregion Server

#region Client
    public override void OnMouseClick()
    {
        if (!isOwned) GamePopupManager.Toast("It's not your mark bowl"); // do nothing if has no authority

        GameMaster.gameManager.OnBowlMarkClick(this);
    }
#endregion Client
}
                                                                                                                                                                                                                                      