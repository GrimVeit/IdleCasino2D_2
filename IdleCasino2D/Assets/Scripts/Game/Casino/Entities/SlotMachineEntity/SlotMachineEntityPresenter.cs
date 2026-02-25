using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineEntityPresenter : ICasinoEntityInfo, ICasinoEntityVisitorTraffic, ICasinoEntityProfit
{
    private readonly SlotMachineEntityModel _model;

    public SlotMachineEntityPresenter(SlotMachineEntityModel model)
    {
        _model = model;
    }

    public void Initialize()
    {
        _model.Initialize();
    }

    public void Dispose()
    {
        _model.Dispose();
    }



    public void Open() => _model.OpenEntity();
    public void Close() => _model.CloseEntity();



    public void ActivateEntityInteractive()
    {

    }

    public void DeactivateEntityInteractive()
    {

    }

    public void SetDealer(IDealer newDealer)
    {

    }

    #region Output

    public event Action<IVisitor> OnVisitorRealised
    {
        add => _model.OnVisitorRealised += value;
        remove => _model.OnVisitorRealised -= value;
    }

    public void AddVisitor(IVisitor visitor) => _model.AddVisitor(visitor);

    public void RemoveVisitor(IVisitor visitor) => _model.RemoveVisitor(visitor);

    #endregion

    #region Input
    public CasinoEntityType CasinoEntityType => CasinoEntityType.Slot;

    public bool CanJoin => _model.CanJoin;

    public bool IsGameRunning => _model.IsGameRunning;

    #endregion

    #region PROFIT

    public event Action<Vector3, int> OnAddCoins
    {
        add => _model.OnAddCoins += value;
        remove => _model.OnAddCoins -= value;
    }

    #endregion
}
