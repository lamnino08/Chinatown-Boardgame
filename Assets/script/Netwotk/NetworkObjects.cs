using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObjects 
{
     // Singleton để truy cập dễ dàng
    private static NetworkObjects _instance;
    public static NetworkObjects Instance => _instance ??= new NetworkObjects();

    // Dictionary lưu trữ các GameObject
    private readonly Dictionary<string, NetworkBehaviour> networkedObjects = new Dictionary<string, NetworkBehaviour>();

    /// <summary>
    /// Đăng ký một GameObject với ID duy nhất.
    /// </summary>
    public void Register(string id, NetworkBehaviour gameObject)
    {
        if (networkedObjects.ContainsKey(id))
        {
            Debug.LogError($"GameObject with ID {id} is already registered.");
            return;
        }

        networkedObjects[id] = gameObject;
        Debug.Log($"GameObject registered: {id}");
    }

    /// <summary>
    /// Xóa một GameObject khỏi quản lý.
    /// </summary>
    public void Unregister(string id)
    {
        if (!networkedObjects.ContainsKey(id))
        {
            Debug.LogWarning($"GameObject with ID {id} does not exist.");
            return;
        }

        networkedObjects.Remove(id);
        Debug.Log($"GameObject unregistered: {id}");
    }

    /// <summary>
    /// Truy xuất GameObject bằng ID.
    /// </summary>
    public NetworkBehaviour Get(string id)
    {
        if (networkedObjects.TryGetValue(id, out NetworkBehaviour obj))
        {
            return obj;
        }

        Debug.LogWarning($"GameObject with ID {id} not found.");
        return null;
    }

    /// <summary>
    /// Kiểm tra xem ID có tồn tại không.
    /// </summary>
    public bool Contains(string id)
    {
        return networkedObjects.ContainsKey(id);
    }
}
