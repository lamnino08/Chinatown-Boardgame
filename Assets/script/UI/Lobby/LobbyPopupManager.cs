using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPopupManager : BasePopupManager
{
    public new static LobbyPopupManager instance
    {
        get { return BasePopupManager.instance as LobbyPopupManager; }
    }

    public void ShowChoseColorPopup()
    {
        BasePopup popup = GetPopup<ColorManagerPopup>();
        if (popup != null)
        {
            popup.Show();
        }
    }
}
