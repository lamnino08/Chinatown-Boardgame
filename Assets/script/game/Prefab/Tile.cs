using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] TMP_Text numberText;
    public int tile { get; private set; } 

    public void SetTileData(int type)
    {
        tile = type;
        numberText.text = type.ToString();
    }
}
