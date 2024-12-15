using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text readyText;
    [SerializeField] private Image colorImage;
    public string Name;
    public void SetData(PlayerLobby player)
    {
        this.Name = player.name;
        nameText.text = player.name;
        readyText.text = "Waiting";
    }

    public void SetReady(bool newState, Color? color = null)
    {
        readyText.text = newState? "Ready" : "Waiting";
        // colorImage.color = color ?? Color.white;
    }

    public void SetColor(UnityEngine.Color color, bool isReady)
    {
        colorImage.gameObject.SetActive(true);
        colorImage.color = color;
        readyText.text = isReady? "Ready" : "Waiting";
    }
}
