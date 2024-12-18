using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class MarkAppearance : MonoBehaviour
{
    [SerializeField] private MeshRenderer material;
    [SerializeField] private HighLight highLight;

    public void SetColor(int color)
    {
        this.material.material.color = Util.TransferColor(color);
    }

    public void Highlight(bool isHighLight)
    {
        highLight.ToggleHighlight(isHighLight);
    }
}
