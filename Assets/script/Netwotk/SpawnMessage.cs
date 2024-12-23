using UnityEngine;

[System.Serializable]
public class SpawnMessage
{
    public string id;
    public string name;       
    public Vector3 position;
    public Quaternion quaternion;
    public string sessionIdOwner;      
}
