using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Mirror;

public class ChoseColorManagerPopup : BasePopup
{
    private static ChoseColorManagerPopup _instance;
    public static ChoseColorManagerPopup instance { get { return _instance; } }

    [SerializeField] private Button[] buttonColors = new Button[5];
    [SerializeField] private Button OkBtn;

    private byte _currentColorIndex = 6;
    private Color _currentColor;
    private void Awake() {
        if (_instance!=null)
        {
            Destroy(gameObject);
        } 
        _instance = this;
    }

    public void OnShow(List<byte> availableColors)
    {
        base.Show();
        for(byte i = 0; i < buttonColors.Length; i++)
        {
            buttonColors[i].gameObject.SetActive(availableColors.Contains(i));
        }
    }

    public override void OnStart() {
        base.OnStart();
        for(byte i = 0; i < buttonColors.Length; i++)
        {
            byte colorIndex = i;
            Image buttonImage = buttonColors[i].GetComponent<Image>();
            Color colorBtn = buttonImage.color;
            buttonColors[i].onClick.AddListener(() => OnColorPick(colorIndex, colorBtn));
        }
        OkBtn.onClick.AddListener(OnConfirmColor);
    }

    public void OnColorPick(byte colorIndex, Color color) 
    {
        Debug.Log(colorIndex);
        _currentColorIndex = colorIndex;
        _currentColor = color;
        OkBtn.GetComponent<Image>().color = color;
    }

    private void OnConfirmColor()
    {
        if (_currentColorIndex == 6)
        {
            LobbyPopupManager.instance.Toast("Please chose your color");
            return;
        }
        GameMaster.instance.localPlayer.SetColor(_currentColorIndex);
        LobbyUIManager.instance.SetColorPlayer(GameMaster.instance.localPlayer.playerName, _currentColor);

        Hide();
    }
}
