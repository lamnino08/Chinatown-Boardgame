using UnityEngine;
using Steamworks;

public class SteamManager : MonoBehaviour
{ 
    public static bool Initialized { get; private set; }

    static SteamManager()
    {
        try
        {
            if (!SteamAPI.Init())
            {
                Debug.LogError("SteamAPI_Init() failed.");
                Initialized = false;
                return;
            }

            Debug.Log("Init steam APi successful");
            Initialized = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"SteamManager initialization error: {e.Message}");
            Initialized = false;
        }
    }
}
