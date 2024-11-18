using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Toast : BasePopup
{
    [SerializeField] private TMP_Text contentText;
    public void Show(string content)
    {
        base.Show();
        contentText.text = content;
    }
}
