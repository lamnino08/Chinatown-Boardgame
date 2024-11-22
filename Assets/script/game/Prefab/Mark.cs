using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class MarkAppearance : MonoBehaviour
{
    [SerializeField] private Material material;
    public byte index { get; private set; }

    public void SetIndexBinder(byte index)
    {
        this.index = index;
        this.material.color = Util.TransferColor(index);
    }
}
