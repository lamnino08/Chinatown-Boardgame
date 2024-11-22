using Mirror;


public class GameManager : NetworkBehaviour
{
    void Start()
    {
        RoomServerManager.instance.NewYear();
    }
}
