using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class PieceGameObject : MonoBehaviour
{
    protected int _owner;
    protected string sessionId;
    public bool IsOwner()
    {
        return GameMaster.sessionId == sessionId;
    }
    public abstract void OnMouseClick();
}
