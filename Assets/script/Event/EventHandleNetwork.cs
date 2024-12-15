using UnityEngine;
using System.Collections.Generic;

public static class EventHandlerNetwork
{
    private static Dictionary<string, System.Action<object>> eventHandlers = new Dictionary<string, System.Action<object>>();

    // Đăng ký handler cho một sự kiện
    public static void Register(string eventName, System.Action<object> handler)
    {
        if (!eventHandlers.ContainsKey(eventName))
        {
            eventHandlers[eventName] = handler;
        }
        else
        {
            eventHandlers[eventName] += handler;
        }
    }

    // Hủy đăng ký handler cho một sự kiện
    public static void Unregister(string eventName, System.Action<object> handler)
    {
        if (eventHandlers.ContainsKey(eventName))
        {
            eventHandlers[eventName] -= handler;
            if (eventHandlers[eventName] == null)
            {
                eventHandlers.Remove(eventName);
            }
        }
    }

    // Gọi handler cho một sự kiện
    public static void Trigger(string eventName, object data)
    {
        if (eventHandlers.ContainsKey(eventName))
        {
            eventHandlers[eventName]?.Invoke(data);
        }
        else
        {
            Debug.LogWarning($"No handler registered for event: {eventName}");
        }
    }
}
