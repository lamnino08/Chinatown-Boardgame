using Mirror;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    void Start()
    {
        GameMaster.gameManager = this;
    }
}
