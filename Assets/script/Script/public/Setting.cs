using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public GameObject Music;
    public Toggle tg;
    public Slider slide;
    static float volume = 0.7f;
    static bool isSoundOn = true;
    private void Start()
    {
        tg.isOn = isSoundOn;
        slide.value = volume;
        AudioListener.volume = volume;
    }
    public void music()
    {
        isSoundOn = tg.isOn;
    }
    public void Sound()
    {
        AudioListener.volume = volume;
    }
}