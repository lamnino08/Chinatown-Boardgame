using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainTile : MonoBehaviour
{
    public ushort own = 0;
    Outline ouline;
    private void Start()
    {
        ouline = gameObject.GetComponent<Outline>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void outLine(bool set)
    {
        ouline.enabled = set;
    }
}
