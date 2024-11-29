using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MarkBowl : NetworkBehaviour
{
    [SerializeField] private GameObject markPref;
    public void SpawnMark(byte[] tiles, Color color)
    {
        StartCoroutine(SpawnMarkCoroutine(tiles, color));
    }

    private IEnumerator SpawnMarkCoroutine(byte[] tiles, Color color)
    {
        foreach(byte tile in tiles)
        {
            Instantiate(markPref, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
                                                                                                                                                                                                                                      