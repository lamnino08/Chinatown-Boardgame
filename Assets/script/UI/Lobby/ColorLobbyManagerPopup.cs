using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ColorLobbyManagerPopup : BasePopup
{
    private static ColorLobbyManagerPopup _instance;
    public static ColorLobbyManagerPopup instance { get { return _instance; } }

    [SerializeField] private Button[] buttonColors = new Button[5];
    [SerializeField] private Button OkBtn;

    private int _currentColor = -1;
    private void Awake() {
        if (_instance!=null)
        {
            Destroy(gameObject);
        } 
        _instance = this;
    }

    protected override void Start() {
        base.Start();

        if (popupImage) Debug.Log("Herer");
        for(int i = 0; i < buttonColors.Length; i++)
        {
            Image buttonImage = buttonColors[i].GetComponent<Image>();
            Color colorBtn = buttonImage.color;
            buttonColors[i].onClick.AddListener(() => OnColorPick(i, colorBtn));
        }

        OkBtn.onClick.AddListener(OnConfirmColor);
    }

    public void OnColorPick(int colorIndex, Color color) 
    {
        _currentColor = colorIndex;
        OkBtn.GetComponent<Image>().color = color;
    }

    private void OnConfirmColor()
    {
        if (_currentColor == -1)
        {
            return;
        }

        Hide();
    }
}
