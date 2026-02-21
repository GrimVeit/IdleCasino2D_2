using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapOrderModel
{
    private readonly Room[] _rooms;

    private readonly List<ISortable> _visitors;
    private readonly List<ISortable> _staffs;

    private IEnumerator cycle;

    public MapOrderModel(Room[] rooms, List<ISortable> visitors)
    {
        _rooms = rooms;
        _visitors = visitors;
    }

    public void Initialize()
    {
        for (int i = 0; i < _rooms.Length; i++)
        {
            _rooms[i].Initialize();
        }

        if(cycle != null)
        {
            Coroutines.Stop(cycle);
            cycle = null;
        }

        cycle = CycleCoro();
        Coroutines.Start(cycle);
    }

    public void Dispose()
    {
        if (cycle != null)
        {
            Coroutines.Stop(cycle);
            cycle = null;
        }
    }

    private IEnumerator CycleCoro()
    {
        AllSorting();

        yield return null;
    }

    private void AllSorting()
    {
        foreach (var room in _rooms)
        {
            List<ISortable> allObjects = new List<ISortable>();

            if (room.staticObjects != null)
            {
                foreach (var sr in room.staticObjects)
                {
                    allObjects.Add(sr);
                }
            }

            foreach (var visitor in _visitors)
            {
                if (room.IsInside(visitor.Position))
                    allObjects.Add(visitor);
            }

            allObjects.Sort((a, b) => a.Position.y.CompareTo(b.Position.y));
            int order = room.orderMax;
            foreach (var obj in allObjects)
            {
                obj.SetOrder(order);
                order--;
                if (order < room.orderMin) order = room.orderMin;
            }
        }
    }
}
