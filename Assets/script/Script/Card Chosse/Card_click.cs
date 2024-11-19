using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Unity.VisualScripting;

public static class ButtonExtension
{
    public static void AddEventListener ( this Button button, Action Onclick)
    {
        button.onClick.AddListener(delegate ()
        {
            Onclick();
        });
    }
    public static void AddEventListener<t> ( this Button button, t pama, Action<t> Onclick)
    {
        button.onClick.AddListener(delegate ()
        {
            Onclick(pama);
        });
    }
    public static void AddEventListener<t,q>(this Button button, t pama1,q pama2, Action<t> Onclick1, Action<q> Onclick2)
    {
        button.onClick.AddListener(delegate ()
        {
            Onclick1(pama1);
            Onclick2(pama2);
        });
    }
}
public class Card_click : MonoBehaviour
{
    public byte num;
    [SerializeField] bool is_chosse = false;
    [SerializeField] Image color;
    void Start()
    {
        color = GetComponent<Image>();
    }
    public void SetNum(byte num)
    {
        this.num = num;
        transform.GetChild(0).GetComponent<TMP_Text>().text = num.ToString();
    }
    public void Click()
    {
        if (is_chosse)
        {
            if (GameManager.ground_chosse.Contains(num) && !GameManager.ground_seperate.Contains(num))
            {
                GameManager.ground_seperate.Add(num);
                GameManager.ground_chosse.Remove(num);
                is_chosse = false;
                color.color = new Color(0,0,0);
                //Debug.Log($"Heeer{GameManager.ground_seperate.Count} {GameManager.ground_chosse.Count}");
            }
        }
        else
        {
            if (GameManager.ground_seperate.Count > 2)
            {
                if (!GameManager.ground_chosse.Contains(num) && GameManager.ground_seperate.Contains(num))
                {
                    GameManager.ground_seperate.Remove(num);
                    GameManager.ground_chosse.Add(num);
                    is_chosse = true;
                    //Debug.Log($"eree{GameManager.ground_seperate.Count} {GameManager.ground_chosse.Count}");
                    color.color = new Color(180,0,0);
                }
            }
            else
                GamUI.instance.Announc("You chosse enough",true);
            
        }
    }
}
