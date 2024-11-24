using UnityEngine;
using TMPro;

public class CardAppearance : MonoBehaviour
{
    [SerializeField] private TMP_Text _cardNumberText;
    [SerializeField] private Outline _outline;
    public void SetNumber(int number)
    {
        _cardNumberText.text = number.ToString();
    }

    public void Hover(bool isHover)
    {
        _outline.enabled = isHover;
    }
}
