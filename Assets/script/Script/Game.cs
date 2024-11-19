
using System.Collections;
using System.Collections.Generic;
using Mirror;
using System.Linq;
// using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using Newtonsoft.Json;

public class Game : NetworkBehaviour
{
    // Start is called before the first frame update
    public static Game local;
    public GameObject Storeobj;
    public static GameObject tile;
    [SyncVar]
    public byte own = 0;
    public uint Id_room = 0; 
    public override void OnStartAuthority()
    {
        if (local == null)
        {
            local = this;
        }
    }
    [SerializeField] Transform position_BowlMark;
    public void Spawn_BowlMark(byte color, NetworkConnection conn)
    {
        GameObject bowl_mark = Instantiate(PrefabManager.instance.Bowl_marks[color-1],position_BowlMark.position,Quaternion.identity);
        NetworkServer.Spawn(bowl_mark,conn);
    }
    /*
        Chosse ground pharse
    */

    /*
        Spawn store card
    */

    [Command]
    public void Commit_chosse_gound(int index, List<byte> Ground_return)
    {
        if (ServerManager.instance.Commit_ground_chosse(index,Ground_return, out bool all_ready))
        {
            if (all_ready) // all member commit their plot
            {
                //Spawn all mark
                // Rpc_Spawn_Mark_OnGround();
                byte i = 1;
                Room room = ServerManager.room;
                foreach(var playerCon in ServerManager.players)
                {
                    Member mem = room.members[i-1];
                    GameObject mark = PrefabManager.instance.markprebs[mem.color-1];
                    foreach(byte gr in mem.ground_seperate)
                    {
                        Tile tilee = tile.transform.GetChild(gr-1).GetChild(1).GetComponent<Tile>();
                        tilee.SetOwn(i);
                        tilee.isMark = true;
                        GameObject markk = Instantiate(mark,tile.transform.GetChild(gr-1).position + new Vector3(0,1,0),Quaternion.identity);
                        markk.GetComponent<Mark>().own = i;
                        NetworkServer.Spawn(markk,playerCon.Value);
                    }
                    i++;
                }
                Rpc_AllCommitGround();
            }
            else
            {
               // Debug.Log("Not all ready");
            }
            Rpc_MemeberCommitgrounds(index);
        }
        else
        {
            GamUI.instance.FailCommitGround();
        }
    }
    [ClientRpc]
    void Rpc_AllCommitGround()
    {
        // local.Cmd_Spawn_Mark_Commit_Ground(Player.localPlayer.Index,Player.localPlayer.room.members[Player.localPlayer.index-1].color);
        GamUI.instance.All_Commit_Ground();    
    }
    [ClientRpc]
    void Rpc_MemeberCommitgrounds(int index)
    {
        GamUI.instance.MemberCommitGround(index);
    } 
    [Server]
    public void Cmd_Spawn_Mark_Commit_Ground(uint id_room,byte index,byte color )
    {   
         //Room room = ServerManager.instance.GetRoomById(id_room);
       //
    }

    [Command]
    public void Cmd_Exchangebtn_click()
    {
        // StartCoroutine(exchangeTime());
        ServerManager.room.members[own-1].isExchange = true;
        Exchangebtn_res();
    }
    [TargetRpc]
    void Exchangebtn_res()
    {
        GamUI.instance.Exchangebtn_res();
    }
    [Command]
     public void Cmd_DoneExchangebtn_click()
    {
        ServerManager.room.members[own-1].isExchange = false;
        bool result = true;
        List<MarkTranfer> listchange = new List<MarkTranfer>();
        foreach(MarkTranfer m in ServerManager.transfer.Values)
        {
            m.Print();
            if (m.fromIndex == own || m.toIndex == own)
            {
                if (m.fromIndex == own)
                {
                    if (m.isReplace == false)
                    {
                        result = false;
                        break;
                    }
                }
                listchange.Add(m);
            }
        }
        if (result)
        {
            string listchangejs = JsonConvert.SerializeObject(listchange);
            List<byte> distinctIndex = listchange.Select(mt => mt.fromIndex).Union(listchange.Select(mt => mt.toIndex)).Distinct().ToList();
            foreach (var index in distinctIndex)
            {
                Debug.Log(index);
                if (index != own)
                {
                    Tag_ConfirmExchange(ServerManager.players[(byte)(index)], listchangejs);
                }
            }
            
        }
        Tag_DoneExchange(result);
    }
    [TargetRpc]
    void Tag_DoneExchange(bool result)
    {
        GamUI.instance.DoneExchangebtn_res(result);
    }
    [TargetRpc]
    void Tag_ConfirmExchange(NetworkConnection conn, string listChangejson)
    {
        List<MarkTranfer> listchangejs = JsonConvert.DeserializeObject<List<MarkTranfer>>(listChangejson);
        GamUI.instance.Announc(listChangejson, false);
    }

}
