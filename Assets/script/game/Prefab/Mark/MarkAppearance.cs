using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class MarkAppearance : MonoBehaviour
{
    [SerializeField] private MeshRenderer material;

    public void SetColor(byte color)
    {
        this.material.material.color = Util.TransferColor(color);
    }
}
