using UnityEngine;
using TMPro;

public class CardAppearance : MonoBehaviour
{
    [SerializeField] private TMP_Text _cardNumberText;
    public void SetNumber(byte number)
    {
        _cardNumberText.text = number.ToString();
    }
}
