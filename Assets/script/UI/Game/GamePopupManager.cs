using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePopupManager : BasePopupManager
{
    public new static GamePopupManager instance
    {
        get { return BasePopupManager.instance as GamePopupManager; }
    }

    public static void Toast (string content)
    {
        Toast popup = instance.GetPopup<Toast>();
        if (popup != null)
        {
            popup.Show(content);
        }
    }

}
