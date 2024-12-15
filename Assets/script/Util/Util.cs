using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Util : MonoBehaviour
{
    // Singleton instance
    public static Util Instance { get; private set; }

    // Color Util
    [SerializeField] private Transform contentColor;
    private List<Color> colors = new List<Color>();

    //Store Card util
    [SerializeField] private Material[] storecardmaterials = new Material[12];

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
    public static Color StringToColor(string colorIndex)
    {
        return Instance.colors[int.Parse(colorIndex)];
    }

    public static Material TransferStoreCardSprite(byte cardIndex)
    {
        return Instance.storecardmaterials[cardIndex];
    }

    public static int NumberTileCard(byte year, int numberPlayer)
    {
        numberPlayer = numberPlayer < 3? 3 : numberPlayer;
        switch (numberPlayer)
        {
            case 3:
                if (year == 1) return 7;
                return 6;
            case 4:
                if (year == 1) return 6;
                return 5;
            case 5:
                if (year < 4) return 5;
                return 4;
        }
        return 0;
    }

    public static int NumberStoreCard(byte year, int numberPlayer)
    {
        switch (numberPlayer)
        {
            case 3:
                if (year == 1) return 7;
                return 4;
            case 4:
                if (year == 1) return 6;
                return 3;
            case 5:
                if (year == 1) return 5;
                if (year == 2 || year == 3) return 3;
                return 2;
        }
        
        return 0;
    }
}
