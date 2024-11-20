using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerItemPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text readyText;
    public string Name => nameText.text;
    public void SetData(string name)
    {
        nameText.text = name;
        readyText.text = "Waiting";
    }

    public void ChangeReady(bool newState)
    {
        readyText.text = newState? "Ready" : "Waiting";
    }
}
