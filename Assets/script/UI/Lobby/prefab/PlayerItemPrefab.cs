using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text readyText;
    [SerializeField] private Image colorImage;
    public string Name;
    public void SetData(string name)
    {
        this.Name = name;
        nameText.text = name;
        readyText.text = "Waiting";
    }

    public void SetReady(bool newState,UnityEngine.Color? color = null)
    {
        readyText.text = newState? "Ready" : "Waiting";
        colorImage.color = color ?? UnityEngine.Color.white;
    }

    public void SetColor(UnityEngine.Color color)
    {
        colorImage.gameObject.SetActive(true);
        colorImage.color = color;
    }
}
