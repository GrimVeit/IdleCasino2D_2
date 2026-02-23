using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOrderView : View
{
    [SerializeField] private List<Room> rooms = new List<Room>();

    public void Initialize()
    {
        OnRoomAdded?.Invoke(rooms);
    }

    public void Dispose()
    {

    }

    #region Output

    public event Action<List<Room>> OnRoomAdded;

    #endregion
}
