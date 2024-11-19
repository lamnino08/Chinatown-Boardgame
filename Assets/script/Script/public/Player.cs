using System.Collections;
using System.Collections.Generic;
using Mirror;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using Unity.VisualScripting;
[SerializeField]
public class Player : NetworkBehaviour
{
    public static Player localPlayer = null;
    bool created = false;
    [SyncVar]
    private byte index;
    private NetworkConnectionToClient connection;
    private uint id_room = 0;
    public Member inform;
    [SyncVar]
    public string Name = "";
    public byte Index
    {
        get{ return index;}
        set
        {   
            if (isServer) index = value;
        }
    }
    public NetworkConnectionToClient Connection
    {
        get { return isServer ? connection : null; }
        set
        {
            if (isServer) 
            {
                connection = value;
            }
        }
       
    }
    void Awake()
    {
         if (!created)
        {
            // Đảm bảo chỉ có một instance của prefab player được giữ lại sau khi chuyển scene
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            // Nếu đã có một instance khác tồn tại, hủy bỏ instance hiện tại
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    override public void OnStartClient()
    {
        if (isLocalPlayer && localPlayer == null)
        {
            localPlayer = this;
            System.Random random = new System.Random();
            string letters = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < 5; i++)
            {
                int randomIndex = random.Next(0, letters.Length);
                Name += letters[randomIndex];
            }
            StartServer(Name);
        }
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        /*
            Set invidual informatiof player
        */
        // random name
       
        connection = connectionToClient;

        
    }
    [Command]
    void StartServer(string name)
    {
        if (ServerManager.players.Count < 5)
        {
            /*
                Spawn UI in lobby when you join room
            */
            // Spawn all member already in room
            if(ServerManager.players.Count > 0)
            {
                List<InformMeber> memberNames= new List<InformMeber>(); 
                foreach(var mem in ServerManager.room.members)
                {
                    memberNames.Add(new InformMeber(mem.name, mem.isready, mem.color));
                }
                string json_names = JsonConvert.SerializeObject(memberNames);
                Tag_ResultJoinRoom(true,json_names);
            }
            //Spanw you to all member in room and you
            Rpc_NewMemberToAll(name);
            ServerManager.instance.AddNewMember(new Member(1,1,Name),this);
            
            Index = (byte)ServerManager.players.Count;
            if (Index == 1)
            {
                Tag_SetStartBtn();
            }
        }
        else
        {
            Tag_ResultJoinRoom(false,null);
        }
    }
    /*
        join game
    */
    //RPC spawn new member join room
    [TargetRpc]
    void Tag_SetStartBtn()
    {
        UIManager.instance.btn_start.gameObject.SetActive(true);

    }
    [ClientRpc]
    public void Rpc_NewMemberToAll(string name)
    {
        InformMeber mem = new InformMeber(name,false, 0);
        UIManager.instance.Spawn_Player_Ui_Lobby(mem);
            Debug.Log("here"+Index);
    }
    [TargetRpc]
    void Tag_ResultJoinRoom(bool isSucess,string json_members)
    {
        if (isSucess)
        {
            List<InformMeber> members = JsonConvert.DeserializeObject<List<InformMeber>>(json_members);
            foreach (var mem in members)
            UIManager.instance.Spawn_Player_Ui_Lobby(mem);
        }
        else
            UIManager.instance.Announc("Room is full or started", true);
    }
    /*
        Chossee collor
    */
    public void UI_Color_btn()
    {
        cmd_Color_btn();
    }
    [Command]
    void cmd_Color_btn()
    {
        List<byte> colorss = ServerManager.instance.Color_btn();
        string colors = JsonConvert.SerializeObject(colorss);
        Targ_Color_btn(colors);
    }
    [TargetRpc]
    void Targ_Color_btn(string colors)
    {
        UIManager.instance.Colors(colors);
    }
    /*
        Ready
    */
    [Command]
    public void cmdReady(byte color)
    {
        if (ServerManager.instance.Ready(Index, color, out bool all_ready))
        {
            Rpc_MemberReady(index, color);
            // Det Start button 
            if (all_ready)
            {
                Tag_alReady(ServerManager.players[1]);
            }
        }
        else
        {
            Color_WasChosse();
        }
    }
    [TargetRpc]
    void Tag_alReady(NetworkConnection connection)
    {
        UIManager.instance.btn_start.interactable = true;
    }
    [ClientRpc]
    void Rpc_MemberReady(byte index,byte color)
    {
        UIManager.instance.Updare_ready(index,color);
    }
    [TargetRpc]
    void Color_WasChosse()
    {
        UIManager.instance.Fail_Ready();
    }
    /*
        Start game
    */
    public void StartGame()
    {
        cmd_StartGame();
    }
    [Command]
    void cmd_StartGame()
    {
        if (ServerManager.instance.StartGame())
        {
            Instantiate(ServerManager.instance.PrefabManager);
            Rpc_StartGame();
        }
        else
            Debug.Log("fail to startGame");
    }
    [ClientRpc]
    void Rpc_StartGame()
    {
        ColorManager.color = 0;
        SceneManager.LoadScene(1);
        if (index == 0)
        {
            Debug.Log("fuc");
            StartGames();
        }
    }
    [Command]
    public void StartGames()
    {
        if (index == 1)
        {
            PrefabManager.instance.SpawnMemberInGame();
            List<List<byte>> Grounds = ServerManager.SeperateGround();
            Room room = ServerManager.room;
            Debug.Log(ServerManager.players.Count);
            foreach(var player in ServerManager.players)
            {
                Tag_GroundSeperate(player.Value, Grounds[player.Key-1]);
                Spawn_store(player.Key);
            }
        }
        
    }
    [Server]
    public void Spawn_store(byte index)
    {
        Room room = ServerManager.room;
        byte i = 0;
        foreach(byte st in room.store_seperate[index-1])
        {
            float x = 0; float z = 0;
            switch (i)
            {
                case 0: 
                    x = -2; z = 0.6f; break;
                case 1: 
                    x = -1; z = 0.6f; break;
                case 2: 
                    x = 0; z = 0.6f; break;
                case 3: 
                    x = 1; z = 0.6f; break;
                case 4: 
                    x = -2; z = -0.5f; break;
                case 5: 
                    x = -1; z = -0.5f; break;
                case 6: 
                    x = 0; z = -0.5f; break;
            }
            i++;
            GameObject Mark_obj = Instantiate(PrefabManager.instance.storeCard[st],PrefabManager.instance.posSpawnMembers[index-1].position+new Vector3(x,3,z), Quaternion.Euler(0,GameManager.Angle_Spawn(index),0));
            //Mark_obj.GetComponent<NetworkMatch>().matchId = Player.localPlayer.network_match.matchId;
            NetworkServer.Spawn(Mark_obj,connectionToClient);
        }
        if (index == room.store_seperate.Count)
            room.store_seperate.Clear();
    }
    [TargetRpc]
    void Tag_GroundSeperate(NetworkConnection con, List<byte> ground)
    {
        // Debug.Log("yesss sirrr");
        GamUI.instance.SetGroundChosse(ground);
    } 
    
    
}
