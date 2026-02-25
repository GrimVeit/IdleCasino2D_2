using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PokerEntityPresenter : ICasinoEntityInfo, ICasinoEntityVisitorTraffic, ICasinoEntityProfit
{
    private readonly PokerEntityModel _model;

    public PokerEntityPresenter(PokerEntityModel model)
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

    public void ActivateEntityInteractive() => _model.ActivateManualInteractive();
    public void DeactivateEntityInteractive() => _model.DeactivateManualInteractive();



    public void OpenEntity() => _model.OpenEntity();
    public void CloseEntity() => _model.CloseEntity();
    public void SetDealer(IDealer newDealer) => _model.SetDealer(newDealer);

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
    public CasinoEntityType CasinoEntityType => CasinoEntityType.Poker;

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
