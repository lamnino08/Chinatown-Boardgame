using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    // btn 
    [SerializeField]  
    private Button colorBtn;

    private void Start() {
        colorBtn.onClick.AddListener(OpenColorPopup);
    }

    private void OpenColorPopup() 
    {
        LobbyPopupManager.instance.ShowChoseColorPopup();
    }
}
