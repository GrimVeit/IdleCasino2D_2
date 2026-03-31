using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffSpawnerPresenter : ISpawnerStaffProvider, ISpawnerStaffListener
{
    private readonly StaffSpawnerModel _model;
    private readonly StaffSpawnerView _view;

    public StaffSpawnerPresenter(StaffSpawnerModel model, StaffSpawnerView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _model.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _model.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnSpawnVisitor += _model.CreateStaff;

        _model.OnSpawnPrefab += _view.SpawnVisitor;
    }

    private void DeactivateEvents()
    {
        _view.OnSpawnVisitor -= _model.CreateStaff;

        _model.OnSpawnPrefab -= _view.SpawnVisitor;
    }

    #region Output

    public event Action<IStaff> OnAddStaff
    {
        add => _model.OnAddStaff += value;
        remove => _model.OnAddStaff -= value;
    }

    #endregion

    #region Input

    public void SetStaff(ICasinoEntityStaff casinoEntity, StaffType type, int skinId) => _model.SpawnStaff(casinoEntity, type, skinId);

    public void Save() => _model.Save();

    #endregion
}

public interface ISpawnerStaffProvider
{
    public void SetStaff(ICasinoEntityStaff casinoEntity, StaffType type, int skinId);
}

public interface ISpawnerStaffListener
{
    public event Action<IStaff> OnAddStaff;
}
