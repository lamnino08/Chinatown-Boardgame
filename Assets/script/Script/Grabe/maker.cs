using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Security.Principal;

public class maker : NetworkBehaviour
{
    [SerializeField] GameObject mark;
    bool isSpawn = true;
    public void OnMouseDown()
    {
        Debug.Log("click marker");
        Cmd_MouseDownMakrer(Player.localPlayer.Index);
    }

    [Command]
    void Cmd_MouseDownMakrer(byte index)
    {
        Member mem = ServerManager.room.members[index-1];
        if (mem.isExchange)
        {
            mark = Instantiate(PrefabManager.instance.markprebs[mem.color-1], transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
            Mark markscpt = mark.GetComponent<Mark>();
            markscpt.isMarker = true;
            markscpt.isDragging = true;
            markscpt.own = index;
            NetworkServer.Spawn(mark, ServerManager.players[(byte)(index)]);
            markscpt.GetComponent<Rigidbody>().isKinematic = true;
            markscpt.Free_rigi();

            ServerManager.transfer.Add(mark, new MarkTranfer(index, 0, 0, 0));
        }
    }

    public void OnMouseUp()
    {
        MouseUp();
    }
    [Command]
    void MouseUp()
    {
        if (mark)
        {
            mark.GetComponent<Mark>().MouseUP();
        }
    }
}
