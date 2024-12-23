using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class NetworkWorker
{
    private readonly NetworkBehaviour target;
    private readonly Dictionary<string, MethodInfo> methodCache = new Dictionary<string, MethodInfo>();

    public NetworkWorker(NetworkBehaviour target)
    {
        this.target = target;

        var methods = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var method in methods)
        {
            if (method.GetCustomAttribute<CommandAttribute>() != null)
            {
                methodCache[method.Name] = method;
            }
        }
    }

    public void Invoke(string methodName, params object[] args)
    {
        if (methodCache.TryGetValue(methodName, out MethodInfo method))
        {
            var attribute = method.GetCustomAttribute<CommandAttribute>();

            var message = new
            {
                id = target.id, // ID của đối tượng mạng
                args = args                     // Tham số
            };

            attribute.Work(message);
        }
        else
        {
            Debug.LogError("You called a method that is not registered as a [Command].");
        }
    }
}
