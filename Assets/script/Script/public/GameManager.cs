using System.Collections.Generic;
using System.Numerics;
using Mirror;
using Telepathy;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static GameManager instance;
    public static List<InformMeber> members = new List<InformMeber>();
    public string state ="";
    public static List<byte> ground_seperate = new List<byte>();
    public static List<byte> ground_chosse = new List<byte>();
   void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        Player.localPlayer.StartGames();
        //GamUI.instance.Start_Member_UI();
    }
    public static float Angle_Spawn(byte index)
    {
        switch (index)
        {
            case 1:
            case 2:
                return 0;
            case 3:
            case 4:
                return 180;
            default:
            return 90;
        }
    }
   

}
