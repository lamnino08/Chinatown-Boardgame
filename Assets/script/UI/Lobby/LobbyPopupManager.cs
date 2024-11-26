using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPopupManager : BasePopupManager
{
    public new static LobbyPopupManager instance
    {
        get { return BasePopupManager.instance as LobbyPopupManager; }
    }

    public void ShowChoseColorPopup(List<byte> availableColors)
    {
        ChoseColorManagerPopup popup = GetPopup<ChoseColorManagerPopup>();
        if (popup != null)
        {
            popup.OnShow(availableColors);
        }
    }

    public void Toast (string content)
    {
        Toast popup = GetPopup<Toast>();
        if (popup != null)
        {
            popup.Show(content);
        }
    }
}
