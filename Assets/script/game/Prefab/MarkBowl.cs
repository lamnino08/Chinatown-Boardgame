using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MarkBowl : NetworkBehaviour
{
    [SerializeField] private GameObject markPref;
    [SerializeField] private AnimationCurve easeingCurve;
    [SerializeField] private List<Transform> pathMarkFlyTransform = new List<Transform>();
    private List<Vector3> pathMarkFlyVec = new List<Vector3>();

    [Server]
    public void SpawnMark(byte[] tiles, byte color, int index)
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
            Vector3 targetTile = Map.instance.GetTile(tiles[i]).transform.position + new Vector3(0,.2f, 0);

            GameObject markObject = Instantiate(markPref, transform.position, Quaternion.identity);
            NetworkServer.Spawn(markObject, connections[index]);

            Mark markscript = markObject.GetComponent<Mark>();
            markscript.SetData(i, color);

            markscript.MoveToTile(transform.position, targetTile);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
                                                                                                                                                                                                                                      