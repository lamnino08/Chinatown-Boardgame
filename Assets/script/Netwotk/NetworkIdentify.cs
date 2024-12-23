using UnityEngine;

[RequireComponent(typeof(NetworkBehaviour))]
public class NetworkIdentity : MonoBehaviour
{
    public string sessionIdOwner { get; private set; }
    public string id { get; private set; }
    private NetworkBehaviour networkBehaviour;

    /// <summary>
    /// Khởi tạo NetworkIdentity
    /// </summary>
    public void Init(string id, string sessionIdOwner)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("Cannot initialize NetworkIdentity: ID is null or empty.");
            return;
        }

        this.id = id;
        this.sessionIdOwner = sessionIdOwner;

        networkBehaviour = GetComponent<NetworkBehaviour>();
        NetworkObjects.Instance.Register(id, networkBehaviour);
    }

    /// <summary>
    /// Kiểm tra xem đối tượng có thuộc quyền sở hữu của client hiện tại không
    /// </summary>
    public bool IsOwner()
    {
        return sessionIdOwner == null || sessionIdOwner == NetworkManager.SessionId;
    }
}
