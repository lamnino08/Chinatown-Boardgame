using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Util : MonoBehaviour
{
    // Singleton instance
    public static Util Instance { get; private set; }

    [SerializeField] private Transform contentColor;
    private List<Color> colors = new List<Color>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (contentColor)
        {
            foreach(Transform colorItem in contentColor)
            {
                Color color = colorItem.GetComponent<Image>().color;
                colors.Add(color);
            }
        }
    }

    public static Color TransferColor(byte index)
    {
        return Instance.colors[index];
    }
}
