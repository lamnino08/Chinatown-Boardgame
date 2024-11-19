using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
// using System.Numerics;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public byte tile = 0;
    Outline ouline;
    public byte own = 0;
    Vector3 posHightLight;
    Vector3 posstart;
    public bool isMark = false;
    private void Start()
    {
        ouline = gameObject.GetComponent<Outline>(); 
        posstart = transform.position;
        posHightLight = posstart + new Vector3(0,0.15f,0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Cmd_HightLight(bool set)
    {
        TileParent.instance.Hightligth(tile, set);
    }
    public void outLine(bool set)
    {
        if (set)
        {
            StartCoroutine(MoveObjectSmoothly(posHightLight, 0.2f));
        }
        else
        {
            StartCoroutine(MoveObjectSmoothly(posstart,0.2f));
        }
    }
    IEnumerator MoveObjectSmoothly(Vector3 targetPosition, float duration)
    {
        float time = 0f;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            float t = time / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo vị trí cuối cùng chính xác
        transform.position = targetPosition;
    }
  
    public void SetOwn(byte index)
    {
        own = index;
    }
}
