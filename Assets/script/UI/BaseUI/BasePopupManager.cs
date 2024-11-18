using System;
using System.Collections.Generic;
using UnityEngine;

public class BasePopupManager : MonoBehaviour
{
    private static BasePopupManager _instance;
    public static BasePopupManager instance { get { return _instance; } }

    [SerializeField] private List<BasePopup> popupList;

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    protected virtual void Start() 
    {
        Debug.Log("heere");
        foreach(BasePopup popup in popupList)
        {
            popup.OnStart();
        }
    }

    public T GetPopup<T>() where T : BasePopup
    {
        foreach (BasePopup popup in popupList)
        {
            if (popup is T)
            {
                return (T)popup;
            }
        }

        Debug.LogError($"Popup of type {typeof(T).Name} not found!");
        return null;
    }
}
