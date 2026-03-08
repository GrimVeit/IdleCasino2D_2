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

    [Header("Skins")]
    [SerializeField] private string[] dealerSkins;     // cr1, cr2, cr3
    [SerializeField] private string[] bartenderSkins;  // br1, br2
    [SerializeField] private string[] songstressSkins; // sg1, sg2
    [SerializeField] private string[] hostessSkins;    // hs1, hs2

    [SerializeField] private Transform transformParentVisitors;

    public void SpawnVisitor(StaffType type, int skinId)
    {
        Debug.Log("SPAWN STAFF");

        IStaffView viewInstance = type switch
        {
            StaffType.Croupier => Instantiate(dealerPrefab, transformParentVisitors),
            StaffType.Bartender => Instantiate(bartenderPrefab, transformParentVisitors),
            StaffType.Songstress => Instantiate(songstressPrefab, transformParentVisitors),
            StaffType.Hostess => Instantiate(hostessPrefab, transformParentVisitors),
            _ => throw new Exception($"No prefab for {type}")
        };

        string skinName = GetSkin(type, skinId);

        viewInstance.SetSkin(skinName);

        OnSpawnVisitor?.Invoke(type, viewInstance);
    }

    private string GetSkin(StaffType type, int id)
    {
        string[] skins = type switch
        {
            StaffType.Croupier => dealerSkins,
            StaffType.Bartender => bartenderSkins,
            StaffType.Songstress => songstressSkins,
            StaffType.Hostess => hostessSkins,
            _ => throw new Exception($"No skins for {type}")
        };

        if (id < 0 || id >= skins.Length)
            id = 0;

        return skins[id];
    }

    #region Output

    public event Action<StaffType, IStaffView> OnSpawnVisitor;

    #endregion
}
