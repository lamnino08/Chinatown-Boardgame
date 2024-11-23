using UnityEngine;
using Mirror;

public class Card : MonoBehaviour
{
    [SerializeField] private CardAppearance _appearance;
    private byte _number;
    public void SetNumber(byte number)
    {
        this._number = number;
        _appearance.SetNumber(number);
    }
}
