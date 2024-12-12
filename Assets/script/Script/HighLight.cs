using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLight : MonoBehaviour
{
    [SerializeField] private List<Renderer> renderers;
    [SerializeField] private float transitionDuration = 0.5f; 
    [SerializeField] private float insensity = 10f; 

    private List<Material> materials;

    private void Awake()
    {
        materials = new List<Material>();
        foreach (var renderer in renderers)
        {
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    public void ToggleHighlight(bool val, byte? color = 6)
    {
        if (val)
        {
            Color hightlightColor = color == 6? Color.white : Util.TransferColor((byte)color);

            foreach (var material in materials)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", hightlightColor*insensity);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}
