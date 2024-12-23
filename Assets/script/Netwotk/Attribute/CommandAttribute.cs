using System;
using UnityEngine;
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandAttribute : Attribute
{
    public void Work(object agrs)
    {
        RoomController.room.Send("broadcast", agrs);
    }
}
