using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Mirror;

public class GameUIManager : MonoBehaviour
{
    private static GameUIManager _instance;
    public static GameUIManager instance => _instance;

    [SerializeField] private Button discardBtn;

    private void Awake() 
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start() 
    {
        discardBtn.onClick.AddListener(OndisCard);
    }

    private void OndisCard()
    {
        GameMaster.localPlayer.NewYear();
    }
}
