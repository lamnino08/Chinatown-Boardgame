using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Mirror;
using System;
using System.Linq;
public class MarkPrevious
{
    public int numtile;
    public Vector3 Position;
}
public class Mark : NetworkBehaviour
{
    [SyncVar]
    public bool isDragging = false;
    Tile start_tile;
    Vector3 start_pos;
    GameObject targetobj;
    GameObject first_tile;
    bool is_FirstTimeMove_inExchangeTime = true;
    int layerMask = 0;
    public bool isMarker = false;
    [SerializeField] public byte own;
    public override void OnStartServer()
    {
        base.OnStartServer();
        layerMask = ~(1 << gameObject.layer);
    }
    public void OnMouseDown()
    {
        StartDrag();
    }
    [Command]
    public void StartDrag()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        Rpc_Kinematic(true);
        
        bool is_exchangeTime = ServerManager.room.members[own-1].isExchange;

        RaycastHit rayhit;
        Physics.Raycast(transform.position, -transform.up, out rayhit,Mathf.Infinity, layerMask);
        if (rayhit.collider == null || rayhit.collider != null && !rayhit.collider.CompareTag("tile"))
        {
            isDragging = true;
            if (is_exchangeTime == false)
            {
                Debug.Log("It's not in exchange time but this mark not on the tile");
            } else 
            {
                start_pos = transform.position;
                Debug.Log("Start drag without tile under mark");
            }
            
        }
        // Get start Tile
        else if (rayhit.collider != null && rayhit.collider.CompareTag("tile"))
        {
            // Debug.Log(rayhit.collider.gameObject.name);
            targetobj = rayhit.collider.gameObject;
            start_tile = targetobj.GetComponent<Tile>();
            start_tile.Cmd_HightLight(true);
            start_pos = transform.position;
            isDragging = true;
            ServerManager.room.members.Sum(element => element.color);

            // if in the exchange time then storage the first tile of the mark
            if (is_exchangeTime)
            {
                if (is_FirstTimeMove_inExchangeTime)
                {
                    first_tile = targetobj;
                    first_tile.GetComponent<Tile>().isMark = false;
                    is_FirstTimeMove_inExchangeTime = false;
                    ServerManager.transfer.Add(gameObject, new MarkTranfer(own, 0,start_tile.tile, 0));
                }
            } 
        }
        else
        {
            Debug.Log("this state in not handled");
        }
               
    }
    [ClientRpc]
    void Rpc_Kinematic(bool iss)
    {
        GetComponent<Rigidbody>().isKinematic = iss;
    }
    /// <summary>
    /// Get position of mouse in the real world 
    /// </summary>
    /// <returns>Vector3</returns>
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
        Vector3 worldPostion = Camera.main.ScreenToWorldPoint(position);
        return worldPostion;
    }
    
    public void Update()
    {
        if (isClient)
        if (isDragging)
        {
            Cmd_Check_move(GetMouseWorldPosition());
        }
    }

    [Command]
    void Cmd_Check_move(Vector3 worldPostion)
    {
        if (isDragging)
        {
            Move_Server(worldPostion);
        }
    }

    /// <summary>
    /// Move i  client
    /// </summary>
    /// <param name="worldPostion"></param>
    [Server]
    void Move_Server(Vector3 worldPostion)
    {
            if (worldPostion.z>-144.7f && worldPostion.z <-124.5 && worldPostion.x <19 && worldPostion.x > -8)
            {
                transform.position = new Vector3(worldPostion.x, 1.5f, worldPostion.z);
                Checck();
            }
            Rpc_move(worldPostion);
    }
    /// <summary>
    /// Move i  client
    /// </summary>
    /// <param name="worldPostion"></param>
    [ClientRpc]
    void Rpc_move(Vector3 worldPostion)
    {
        if (worldPostion.z>-144.7f && worldPostion.z <-124.5 && worldPostion.x <19 && worldPostion.x > -8)
            transform.position = new Vector3(worldPostion.x, 1.5f, worldPostion.z);
    }

    /// <summary>
    /// Logic when move a mark 
    /// </summary>
    [Server]
    public void Checck()
    {
        RaycastHit[] rayhits = Physics.RaycastAll(transform.position, -transform.up,Mathf.Infinity, layerMask);

        RaycastHit rayhit = new RaycastHit();

        if (rayhits.Length > 0)
            rayhit = rayhits[rayhits.Length-1];
        // Debug.Log(rayhit.collider.gameObject.name);

        // If there no get collider
        if (rayhit.collider == null || !rayhit.collider.CompareTag("tile"))
        {
           
            {
                if (targetobj != null && targetobj.CompareTag("tile"))
                {
                    targetobj.GetComponent<Tile>().Cmd_HightLight(false);
                    targetobj = null;
                }
            }
            
        }
        // Drag to another gamobject
        else if (rayhit.collider != null )
        {
            GameObject rayhitt = rayhit.collider.gameObject;
            // if coliider is different previous tagret
            if (targetobj != null && targetobj.CompareTag("tile"))
            {
                // drag on to another tile 
                
                if (rayhitt != targetobj)
                {
                    // un Hightligth previouse tile
                    Tile previoustarget = targetobj.GetComponent<Tile>();
                    previoustarget.Cmd_HightLight(false);

                    // targetobj tile is collider
                    //highlight target obj
                    
                    if (rayhitt.CompareTag("tile"))
                    {
                        targetobj = rayhitt;
                        Tile tile = targetobj.GetComponent<Tile>();
                        //if the tile in not marked then highlight it
                        if (!tile.isMark)
                            tile.Cmd_HightLight(true);
                    }
                }
                
            } else
            {
                if (rayhitt.CompareTag("tile"))
                {
                    targetobj = rayhitt;
                    Tile tile = targetobj.GetComponent<Tile>();
                    if (!tile.isMark)
                    {
                        tile.Cmd_HightLight(true);
                    }
                }
            }
        }
    }

    public void OnMouseUp()
    {
        Cmd_MouseUP();
    }
    [Command]
    void Cmd_MouseUP()
    {
        MouseUP();
    }
    [Server]
    public void MouseUP()
    {
        
        bool is_exchangeTime = ServerManager.room.members[own-1].isExchange;
        if (is_exchangeTime)
        {
            isDragging = false;
            transform.position = transform.position + new Vector3(0,-0.25f,0);
            Rpx_MouseUp(this.transform.position);

            if (targetobj != null && targetobj.CompareTag("tile"))
            {
                Tile tile = targetobj.GetComponent<Tile>();
                if (tile.own == 0 || !tile.isMark) 
                {  
                    transform.position = targetobj.transform.position + new Vector3(0, .5f, 0);
                    Rpx_MouseUp(targetobj.transform.position + new Vector3(0,.5f,0));
                    tile.Cmd_HightLight(false);

                    // have mark in tile
                    tile.isMark = true;
                        if (first_tile != null && tile.tile == first_tile.GetComponent<Tile>().tile) // this mark back to the same own
                        {
                            ServerManager.transfer.Remove(gameObject);
                            is_FirstTimeMove_inExchangeTime = true;
                        }
                        else
                        {
                            ServerManager.transfer[gameObject].toTile = tile.tile;
                            if (tile.own != 0) // && tile.own != own
                            {
                                ServerManager.tileNewOwn(tile.tile);
                                ServerManager.transfer[gameObject].toIndex = tile.own;
                            }
                        }
                }
                else
                {
                    if(isMarker)
                    {
                        Rpc_Destroythis();
                        Destroy(gameObject);
                    }
                    else
                    {
                        transform.position = Start_Pos(start_pos);
                        Rpx_MouseUp(transform.position);
                    }
                } 
            }
            else if (targetobj == null || targetobj != null && !targetobj.CompareTag("tile") )
            {
                if (isMarker )
                {
                    Rpc_Destroythis();
                    Destroy(gameObject);
                } else
                {
                    if (targetobj != null && targetobj.CompareTag("bowl_mark"))
                    {
                      
                        transform.position = Start_Pos(start_pos);
                        Rpx_MouseUp(transform.position);
                    }
                    else
                    {
                        transform.position = transform.position + new Vector3(0,-.25f,0);
                        Rpx_MouseUp(transform.position);
                    }
                }
                
            }
            start_tile = null;
            targetobj = null;
        } else
        {
            isDragging = false;
            GetComponent<Rigidbody>().isKinematic = false;
            // if coliider is tile
            if (targetobj != null && targetobj.CompareTag("tile"))
            {
                Tile tile = targetobj.GetComponent<Tile>();
                // if (tile.own == own)
                // {
                //     if (tile.isMark)
                //     {
                //         transform.position = Start_Pos(start_pos);
                //         Rpx_MouseUp(transform.position);
                //     }
                //     else
                //     {
                //         transform.position = targetobj.transform.position + new Vector3(0, .5f, 0);
                //         Rpx_MouseUp(targetobj.transform.position + new Vector3(0,.5f,0));
                //         tile.Cmd_HightLight(false);
                //     }
                // }
                // else
                // {
                    tile.Cmd_HightLight(false);
                    transform.position = Start_Pos(start_pos);
                    Rpx_MouseUp(transform.position);
                // }
            }
            else
            {
                transform.position = Start_Pos(start_pos);
                Rpx_MouseUp(transform.position);
            }
            start_tile = null;
            targetobj = null;
        }
        
    //     Tile start_tile;
    // Vector3 start_pos;
    // GameObject targetobj;
    // GameObject first_tile;
    }

    Vector3 Start_Pos(Vector3 start)
    {
        return new Vector3(start.x, 0.8f, start.z);
    }
    [ClientRpc]
    void Rpc_Destroythis()
    {
        Destroy(gameObject);
    }
    [ClientRpc]
    void Rpx_MouseUp(Vector3 pos)
    {
        GetComponent<Rigidbody>().isKinematic = false;
        transform.position = pos;
    }
    [ClientRpc]
    public void Free_rigi()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
