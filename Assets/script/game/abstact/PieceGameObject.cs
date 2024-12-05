using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public abstract class PieceGameObject : NetworkBehaviour
{
    public abstract void OnMouseClick();
}
