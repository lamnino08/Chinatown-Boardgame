using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Mirror;
using Colyseus.Schema;

public class ChoseColorManagerPopup : BasePopup
{
    private static ChoseColorManagerPopup _instance;
    public static ChoseColorManagerPopup instance { get { return _instance; } }

    [SerializeField] private Button[] buttonColors = new Button[5];
    [SerializeField] private Button OkBtn;

    private void Awake() {
        if (_instance!=null)
        {
            Destroy(gameObject);
        } 
        _instance = this;
    }

    public void OnShow(MapSchema<string> availableColors)
    {
        base.Show();
        for(int i = 0; i < buttonColors.Length; i++)
        {
            buttonColors[i].gameObject.SetActive(availableColors.ContainsKey(i.ToString()));
        }
    }

    public override void OnStart() {
        base.OnStart();
        for(int i = 0; i < buttonColors.Length; i++)
        {
            int colorIndex = i;
            Image buttonImage = buttonColors[i].GetComponent<Image>();
            Color colorBtn = buttonImage.color;
            buttonColors[i].onClick.AddListener(() => OnColorPick(colorIndex, colorBtn));
        }
    }

    public void OnColorPick(int colorIndex, Color color) 
    {
        GameMaster.color = colorIndex.ToString();
        OkBtn.onClick.AddListener(() => OnConfirmColor(color));
        OkBtn.GetComponent<Image>().color = color;
    }

    private void OnConfirmColor(Color color)
    {
        if (GameMaster.color == "")
        {
            LobbyPopupManager.instance.Toast("Please chose your color");
            return;
        }
        LobbyUIManager.instance.SetColorPlayer(GameMaster.PlayerName, color);

        Hide();
    }
}
