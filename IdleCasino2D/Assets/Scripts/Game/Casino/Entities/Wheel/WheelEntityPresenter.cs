using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WheelEntityPresenter : ICasinoEntityInfo, ICasinoEntityVisitorTraffic, ICasinoEntityProfit
{
    private readonly WheelEntityModel _model;

    public WheelEntityPresenter(WheelEntityModel model)
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

    #region VISITOR TRAFFIC

    public event Action<IVisitor> OnVisitorRealised
    {
        add => _model.OnVisitorRealised += value;
        remove => _model.OnVisitorRealised -= value;
    }

    public void AddVisitor(IVisitor visitor) => _model.AddVisitor(visitor);

    public void RemoveVisitor(IVisitor visitor) => _model.RemoveVisitor(visitor);

    #endregion

    #region INFO
    public CasinoEntityType CasinoEntityType => CasinoEntityType.Wheel;

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
