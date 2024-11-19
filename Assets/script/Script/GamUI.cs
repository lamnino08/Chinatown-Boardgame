using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamUI : MonoBehaviour
{
    public static GamUI instance;
    [SerializeField] public Transform tiles;
    [SerializeField] public GameObject mark;
    [Header("Ground chosse phrase")]
    [SerializeField] public Transform ground_card;
    [SerializeField] private Transform content_groundCardschild;
    [SerializeField] private Transform content_groundCards;
    [SerializeField] TextMeshProUGUI announctext;
    [SerializeField] Transform content_member_UI;
    [SerializeField] Transform member_UI;
    [Header("Commit ground chosse")]
    [SerializeField] Button commit_ground_btn;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        Destroy(this);
    }
    public void Start_Member_UI()
    {
        foreach (InformMeber mem in GameManager.members)
        {
            Instantiate(member_UI, content_member_UI).GetComponent<PlayerLobbyUI>().StartLobby(mem.name, false, mem.color);
        }
    }
    /*
        Seperate card
    */
    public void SetGroundChosse(List<byte> grounds)
    {
        // Debug.Log("Yessss");
        GameManager.instance.state = "ChosseGround";
        content_groundCards.gameObject.SetActive(true);
        foreach(byte gr in grounds)
        {
            GameManager.ground_seperate.Add(gr);
            Transform newCard = Instantiate(ground_card,content_groundCardschild);
            Card_click newcard = newCard.GetComponent<Card_click>();
            newcard.SetNum(gr);
            newCard.GetComponent<Button>().AddEventListener(newcard.Click);
        }
    }
    /*
        Chosse ground pharse
    */
    public void Commit_ground_btn()
    {
        if (GameManager.ground_seperate.Count > 2)
        {
            Announc("Please choosse to enough ground",true);
        }
        else
        {
            Game.local.Commit_chosse_gound(Player.localPlayer.Index, GameManager.ground_seperate);
            commit_ground_btn.interactable = false;
        }
    }
    public void MemberCommitGround(int index)
    {
        //update UI
        // content_member_UI.GetChild(index-1).GetComponent<PlayerLobbyUI>().SetReady(true);
        //update Backend
        // if (index == ClientManager.instance.index)
        // {
        //     commit_ground_btn.gameObject.SetActive(false);
        // }
    }
    public void FailCommitGround()
    {
        Announc("Fail to commit ground", true);
        commit_ground_btn.interactable = true;
    }
    [Header("Phares")]
    [SerializeField] TMP_Text Pharse;
    // When All commit ground
    public void All_Commit_Ground()
    {
        DestroyAllChildren(content_groundCardschild);
        //commit_ground_btn.Interacle = true;
        commit_ground_btn.interactable = true;
        content_groundCards.gameObject.SetActive(false);
        //Pharse.text = "Negotiate";

        exchange_btn.gameObject.SetActive(true);
    }
    public void Announc(string text,bool warn)
    {
        StartCoroutine(AnnouncIE(text, warn));
    } 
    
    IEnumerator AnnouncIE(string text,bool warn)
    {
        announctext.text = text;
        announctext.gameObject.SetActive(true);
        announctext.color = warn ? Color.red : Color.green;
        yield return new WaitForSeconds(4);
       announctext.gameObject.SetActive(false);
    }
    public void DestroyAllChildren(Transform parentTransform)
    {
        int childCount = parentTransform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = parentTransform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }

    [Header("Exchange")]
    [SerializeField] Button doneexchange_btn;
    [SerializeField] Button exchange_btn;
    [SerializeField] GameObject wattingForFriend;
    public void Exchangebtn_click()
    {
        Game.local.Cmd_Exchangebtn_click();
        exchange_btn.interactable = false;
    }
    public void Exchangebtn_res()
    {
        exchange_btn.interactable = true;
        exchange_btn.gameObject.SetActive(false);
        doneexchange_btn.gameObject.SetActive(true);
    }

    public void DoneExchangebtn_click()
    {
        Debug.Log("click done exchange");
        Game.local.Cmd_DoneExchangebtn_click();
        doneexchange_btn.interactable = false;
    }
    public void DoneExchangebtn_res(bool result)
    {
        if (result)
        {
            // doneexchange_btn.interactable = true;
            // doneexchange_btn.gameObject.SetActive(false);
            // exchange_btn.gameObject.SetActive(true);
            wattingForFriend.SetActive(true);
        }
        else
        {
            Announc("Please mark enough your tile", true);
        }
    }

    [Header("Exchange request")]
    [SerializeField] Transform AnOffer;
    [SerializeField] Transform Content;
    
    public void RequestExchange_UI(string listchangejson)
    {
        if (listchangejson != null)
        {
            try 
            {
                List<MarkTranfer> listchange = JsonConvert.DeserializeObject<List<MarkTranfer>>(listchangejson);
                foreach(MarkTranfer m in listchange)
                {
                    Transform anoffer = Instantiate(AnOffer,Content);
                    anoffer.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = m.
                }
            }
            catch
            {

            }
        }
        else
        {
            Debug.LogWarning("You are requested exhange item but it empty");
        }

    }


    [Header("Instruction card")]
    [SerializeField] GameObject UI_cardInstruction;
    [SerializeField] Transform Card;
    [SerializeField]  Sprite frontCard;
    [SerializeField]  Sprite backCard;
    public void InstructionCard()
    {
        UI_cardInstruction.SetActive(true);
    }

    bool isFront = true;
   public void TurnCard()
{
    StartCoroutine(TurnCardAnimation());
}

IEnumerator TurnCardAnimation()
{
    float duration = 0.3f; // Thời gian lật thẻ (0.5 giây)
    float targetAngle = 90;

    Quaternion startRotation = Card.rotation;
    Quaternion targetRotation = Quaternion.Euler(targetAngle, 0.0f, 0.0f); // Góc lật 90 độ theo trục x

    // Lật thẻ
    float elapsed = 0.0f;
    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);
        Card.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
        yield return null;
    }
    Card.rotation = targetRotation;

    // Thay đổi sprite
    if (isFront)
    {
        Card.GetComponent<Image>().sprite = backCard;
        isFront = false;
    }
    else
    {
        Card.GetComponent<Image>().sprite = frontCard;
        isFront = true;
    }

    // Quay trở lại góc 0 độ
    startRotation = targetRotation;
    targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    elapsed = 0.0f;
    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);
        Card.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
        yield return null;
    }
    Card.rotation = targetRotation;
}
}
