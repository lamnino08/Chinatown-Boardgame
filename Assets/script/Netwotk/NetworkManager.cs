using System.Collections.Generic;
using UnityEngine;
using Colyseus;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public static string SessionId { get; private set; }

    [SerializeField]
    [Tooltip("Name and gameobject are allowed spawn through network")]
    private List<NetworkedObjectEntry> networkedObjectList = new List<NetworkedObjectEntry>();

    [SerializeField] private Dictionary<string, GameObject> networkedObjects = new Dictionary<string, GameObject>();
    // private Dictionary<string, Game

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        
        // Pure list gameobject to dictionary
        foreach (var entry in networkedObjectList)
        {
            networkedObjects.Add(entry.name, entry.value);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnSpawnObject(SpawnMessage message)
    {
        if (networkedObjects.TryGetValue(message.name, out GameObject prefab))
        {
            Quaternion quaternion = message.quaternion != null ? quaternion = message.quaternion : prefab.transform.rotation;

            GameObject obj = Instantiate(prefab, message.position, quaternion);
            NetworkBehaviour networkBehaviour = obj.GetComponent<NetworkBehaviour>();
            // networkBehaviour.OnStartSpawn(message.id, message.sessionIdOwner);
        } else
        {
            Debug.LogError($"[Network manager] Require spawn a gameobject have been not registered name {message.name}");
            return;
        }
        

    }

    // private void OnDespawnObject(string id)
    // {
    //     if (networkedObjects.TryGetValue(id, out GameObject obj))
    //     {
    //         Destroy(obj);
    //         networkedObjects.Remove(id);
    //     }
    // }

    // public void SpawnObject(string prefabName, Vector3 position, string owner)
    // {
    //     room.Send("spawn", new { prefabName, position, owner });
    // }

    // public void DespawnObject(string id)
    // {
    //     room.Send("despawn", id);
    // }
}
