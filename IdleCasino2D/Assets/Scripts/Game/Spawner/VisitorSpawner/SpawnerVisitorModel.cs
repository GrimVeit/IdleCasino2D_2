using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerVisitorModel
{
    public int CountVisitors => _visitors.Count;

    private readonly List<List<CasinoEntityType>> routesVisitor = new()
    {
        //new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Wheel, CasinoEntityType.Exit },
        new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Wheel, CasinoEntityType.Music, CasinoEntityType.Exit },
        //new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Wheel, CasinoEntityType.Bar, CasinoEntityType.Exit },

        //new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Slot, CasinoEntityType.Exit },
        new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Slot, CasinoEntityType.Music, CasinoEntityType.Exit },
        //new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Slot, CasinoEntityType.Bar, CasinoEntityType.Exit },

        //new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Poker, CasinoEntityType.Exit },
        new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Poker, CasinoEntityType.Music, CasinoEntityType.Exit },
        //new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Poker, CasinoEntityType.Bar, CasinoEntityType.Exit },

        //new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Roulette, CasinoEntityType.Exit },
        new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Roulette, CasinoEntityType.Music, CasinoEntityType.Exit },
        //new List<CasinoEntityType> {CasinoEntityType.EntranceQueue, CasinoEntityType.Roulette, CasinoEntityType.Bar, CasinoEntityType.Exit }
    };

    private readonly List<IVisitor> _visitors = new List<IVisitor>();

    public void AddVisitor(IVisitor visitor)
    {
        _visitors.Add(visitor);

        OnAddVisitor?.Invoke(visitor);
    }

    public void RemoveVisitor(IVisitor visitor)
    {
        _visitors.Remove(visitor);

        OnRemoveVisitor?.Invoke(visitor);
    }



    public void SpawnVisitor()
    {
        OnSpawnVisitor?.Invoke(routesVisitor[Random.Range(0, routesVisitor.Count)]);
    }

    public void DestroyVisitor(IVisitor visitor)
    {
        OnDestroyVisitor?.Invoke(visitor);
    }

    #region Output

    public event Action<List<CasinoEntityType>> OnSpawnVisitor;
    public event Action<IVisitor> OnDestroyVisitor;

    public event Action<IVisitor> OnAddVisitor;
    public event Action<IVisitor> OnRemoveVisitor;

    #endregion
}
