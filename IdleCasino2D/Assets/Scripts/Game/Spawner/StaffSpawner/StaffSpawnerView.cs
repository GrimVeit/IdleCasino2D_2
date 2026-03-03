using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffSpawnerView : View
{
    [Header("Staff Prefabs")]
    [SerializeField] private DealerView dealerPrefab;
    [SerializeField] private BartenderView bartenderPrefab;
    [SerializeField] private SongstressView songstressPrefab;
    [SerializeField] private HostessView hostessPrefab;

    [SerializeField] private Transform transformParentVisitors;

    public void SpawnVisitor(StaffType type)
    {
        IStaffView viewInstance = type switch
        {
            StaffType.Croupier => Instantiate(dealerPrefab, transformParentVisitors),
            StaffType.Bartender => Instantiate(bartenderPrefab, transformParentVisitors),
            StaffType.Songstress => Instantiate(songstressPrefab, transformParentVisitors),
            StaffType.Hostess => Instantiate(hostessPrefab, transformParentVisitors),
            _ => throw new Exception($"No prefab for {type}")
        };

        OnSpawnVisitor?.Invoke(type, viewInstance);
    }

    #region Output

    public event Action<StaffType, IStaffView> OnSpawnVisitor;

    #endregion
}
