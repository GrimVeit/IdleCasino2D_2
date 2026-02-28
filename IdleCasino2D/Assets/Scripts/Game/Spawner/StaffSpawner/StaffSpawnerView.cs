using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffSpawnerView : View
{
    [Header("Staff Prefabs")]
    [SerializeField] private DealerView dealerPrefab;

    [SerializeField] private Transform transformParentVisitors;

    public void SpawnVisitor(StaffType type)
    {
        IStaffView viewInstance = type switch
        {
            StaffType.Croupier => Instantiate(dealerPrefab, transformParentVisitors),
            _ => throw new Exception($"No prefab for {type}")
        };

        OnSpawnVisitor?.Invoke(type, viewInstance);
    }

    #region Output

    public event Action<StaffType, IStaffView> OnSpawnVisitor;

    #endregion
}
