using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Mirror;
using Telepathy;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Button and Input")]
    [SerializeField] private TMP_InputField input_idroom;
    [SerializeField] private Button btn_color;
    [SerializeField] public  Button btn_ready;
    [SerializeField] public Button btn_start;
    [SerializeField] private TMP_Text announc;
    [Header("Spawn Player Lobby")]
    [SerializeField] public Transform Content_Player;
    [SerializeField] private Transform Player_UI; 
    [Header("Color")]
    [SerializeField] private GameObject Color_UI;

    /*
        Join room btn
    */
    public void Spawn_Player_Ui_Lobby(InformMeber member)
    {
        // add in backend
        GameManager.members.Add(member);
        Instantiate(Player_UI, Content_Player).GetComponent<PlayerLobbyUI>().StartLobby(member.name, member.isReady,member.color);
    }
    /*
        Chosse color
    */
    public void Color_tbn()
    {
        Player.localPlayer.UI_Color_btn();
    }
    public void Colors(string colors)
    {
        Color_UI.SetActive(true);
        ColorManager.instance.StartColor(colors);
    }
    /*
        Ready tbn
    */
    public void Ready()
    {
        // UI
        btn_ready.interactable = false;
        btn_color.interactable = false;
        //Call backend
        Player.localPlayer.cmdReady(ColorManager.color);
    }
    public void Fail_Ready()
    {
        // UI
        btn_ready.interactable = true;
        btn_color.interactable = true;

        Announc("Your color was chosse by other player",true);
    }
    public void Updare_ready(int index,byte color)
    {
        //update frontend
        PlayerLobbyUI playerLobyyScript = Content_Player.GetChild(index-1).GetComponent<PlayerLobbyUI>();
        playerLobyyScript.SetColor(color);
        playerLobyyScript.SetReady(true);
    }
    
    public IEnumerator Announc(string announc, bool Warn)
    {
        this.announc.gameObject.SetActive(true);
        if (Warn)
            this.announc.color = Color.red;
        else 
            this.announc.color = Color.white;
        this.announc.text = announc;
        yield return new WaitForSeconds(4);
        this.announc.gameObject.SetActive(false);
    }
    /*
        OUT Room
    */
    
    public void Outroom(int index)
    {
        if (index == ClientManager.instance.index)
        {
            ClientManager.instance.Outroom();
            input_idroom.interactable = true;
            btn_color.gameObject.SetActive(false);
            btn_ready.interactable = false;
            btn_start.gameObject.SetActive(false);
            int childCount = transform.childCount;
            for (int i = Content_Player.childCount - 1; i >= 0; i--)
            {
                Destroy(Content_Player.GetChild(i).gameObject);
            }
;       }
        else
        {
            if (index == 1 && ClientManager.instance.index == 2)
            {
                btn_start.gameObject.SetActive(true);
            }
            Destroy(Content_Player.GetChild(index-1).gameObject);
            ClientManager.instance.RemMember(index);
        }
    }
    /*
        Start
    */
    public void Start_btn()
    {
        Player.localPlayer.StartGame();
    }
}