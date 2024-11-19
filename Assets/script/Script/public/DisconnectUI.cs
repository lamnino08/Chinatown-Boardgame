using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectUI : MonoBehaviour
{
    public static DisconnectUI singleton;
    public static DisconnectUI Singleton
    {
        get => singleton;
        private set
        {
            if (singleton == null) singleton = value;
            else if (singleton != value) Destroy(value);
        }
    }
    private void Awake()
    {
        Singleton = this;
    }
    [SerializeField] private GameObject disconectUI;
    [SerializeField] private GameObject connecting;
    [SerializeField] private GameObject failConnect;

    public void Disconect()
    {
        disconectUI.SetActive(true);
        connecting.SetActive(true);
        failConnect.SetActive(false);
    }
    public void Connect()
    {
        disconectUI.SetActive(false);
    }
    public void FailConnect()
    {
        disconectUI.SetActive(true);
        connecting.SetActive(false);
        failConnect.SetActive(true);
    }
}
