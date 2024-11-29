using System;
using System.Collections.Generic;

public class EventBus
{
    private static readonly Dictionary<Type, Delegate> _eventListeners = new Dictionary<Type, Delegate>();

    //Subcribe
    public static void Subscribe<T>(Action<T> listener)
    {
        if (_eventListeners.ContainsKey(typeof(T)))
        {
            _eventListeners[typeof(T)] = Delegate.Combine(_eventListeners[typeof(T)], listener);
        }
        else
        {
            _eventListeners.Add(typeof(T), listener);
        }
    } 

    // Unsubscribe from an event
    public static void Unsubscribe<T>(Action<T> listener)
    {
        if (_eventListeners.ContainsKey(typeof(T)))
        {
            _eventListeners[typeof(T)] = Delegate.Remove(_eventListeners[typeof(T)], listener);
        }
    }

    // Notificate an event
    public static void Notificate<T>(T eventData)
    {
        if (_eventListeners.ContainsKey(typeof(T)))
        {
            var eventListener = _eventListeners[typeof(T)] as Action<T>;
            eventListener?.Invoke(eventData);
        }
    }
}
