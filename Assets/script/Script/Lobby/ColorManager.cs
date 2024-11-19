using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;
    [SerializeField] private GameObject[] colors;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] private Image commit_btn;
    [SerializeField] private GameObject CHosseColor_UI;
    public static byte color = 0;
    public void Color_Chosse(int color)
    {
        ColorManager.color = (byte)color;
        commit_btn.color = Turn_Color(color);
    }
    /// <summary>
    /// Set color are available to UI chosse color
    /// </summary>
    /// <param name="json_colors"></param>
    public void StartColor(string json_colors)
    {
        List<int> color = JsonConvert.DeserializeObject<List<int>>(json_colors);
        for(int i = 0; i < colors.Length; i++)
        {
            if (color.Contains(i+1))
            {
                colors[i].SetActive(true);
            }
            else
                colors[i].SetActive(false);
        }   
    }
    public void Commit_btn()
    { 
        if (color == 0)
        {
            UIManager.instance.StartCoroutine(UIManager.instance.Announc("Please chosse a color", true));
        }
        else
        {
            CHosseColor_UI.SetActive(false);
            UIManager.instance.btn_ready.interactable = true;
        }
    }
    public static Color Turn_Color(int color)
    {
        switch (color)
        {
            case 1:
                return Color.red;
            case 2:
                return Color.magenta;
            case 3:
                return Color.green;
            case 4:
                return Color.blue;
            default:
                return Color.yellow;
        }
    }
}
