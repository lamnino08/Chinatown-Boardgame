using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Networking : NetworkManager
{
    [Header("Client")]
    [SerializeField] GameObject Enventsystem;
    [SerializeField] GameObject Canvas;
    public override void OnClientConnect()
    {
        base.OnClientConnect();
    }
}
