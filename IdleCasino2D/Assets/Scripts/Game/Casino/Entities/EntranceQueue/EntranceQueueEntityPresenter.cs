using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceQueueEntityPresenter : ICasinoEntityInfo, ICasinoEntityVisitorTraffic, ICasinoEntityVisitorClickListener
{
    private readonly EntranceQueueEntityModel _model;

    public EntranceQueueEntityPresenter(EntranceQueueEntityModel model)
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
    public CasinoEntityType CasinoEntityType => CasinoEntityType.EntranceQueue;
    public bool IsOpen => true;
    public bool CanJoin => _model.CanJoin;
    public bool IsGameRunning => false;

    #endregion

    #region VISITOR CLICK

    public event Action<IVisitor> OnVisitorClick
    {
        add => _model.OnClickVisitor += value;
        remove => _model.OnClickVisitor -= value;
    }

    #endregion
}
