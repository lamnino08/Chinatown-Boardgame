using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Toast : BasePopup
{
    [SerializeField] private TMP_Text contentText;
    public void Show(string content, float duration = 5000)
    {
        base.Show(duration);
        contentText.text = content;
    }
}
