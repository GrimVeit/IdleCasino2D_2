using System.Collections;
using System.Collections.Generic;

public class MapOrderModel
{
    private readonly Room[] _rooms;

    private readonly List<ISortable> _visitors;
    private readonly List<ISortable> _staffs;

    private readonly ISpawnerVisitorListener _spawnerVisitorListener;
    private IEnumerator cycle;

    public MapOrderModel(Room[] rooms, ISpawnerVisitorListener spawnerVisitorListener)
    {
        _rooms = rooms;
        _spawnerVisitorListener = spawnerVisitorListener;

        _spawnerVisitorListener.OnAddVisitor += AddVisitor;
        _spawnerVisitorListener.OnRemoveVisitor += RemoveVisitor;
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
        _spawnerVisitorListener.OnAddVisitor -= AddVisitor;
        _spawnerVisitorListener.OnRemoveVisitor -= RemoveVisitor;

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

    #region EVENTS

    private void AddVisitor(IVisitor visitor)
    {
        _visitors.Add(visitor);
    }

    private void RemoveVisitor(IVisitor visitor)
    {
        _visitors.Remove(visitor);
    }

    #endregion
}
