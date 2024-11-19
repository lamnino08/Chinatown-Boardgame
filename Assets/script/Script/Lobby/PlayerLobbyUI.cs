using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text ready;
    [SerializeField] private Image color_img;
    public void StartLobby(string name, bool isReady, byte color)
    {
        Name.text = name;
        SetReady(isReady);
        SetColor(color);
    }
    public void SetReady(bool ready)
    {
        if (ready)
        {
            this.ready.color = Color.green;
        }
        else
        {
            this.ready.color = Color.red;
        }
    }
    public void SetColor(int color)
    {
        if (color != 0)
        {
            color_img.gameObject.SetActive(true);
            color_img.color = ColorManager.Turn_Color(color);
        }
    }
}
