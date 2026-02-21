using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PokerEntityPresenter : ICasinoEntity
{
    private readonly PokerEntityModel _model;

    public PokerEntityPresenter(PokerEntityModel model)
    {
        _model = model;
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
        _model.OnVisitorRealised += RealiseVisitor;
    }

    private void DeactivateEvents()
    {
        _model.OnVisitorRealised -= RealiseVisitor;
    }

    public void ActivateManualInteractive() => _model.ActivateManualInteractive();
    public void DeactivateManualInteractive() => _model.DeactivateManualInteractive();

    public void AddVisitor(IVisitor visitor) => _model.AddVisitor(visitor);

    public void RemoveVisitor(IVisitor visitor) => _model.RemoveVisitor(visitor);



    public void OpenEntity() => _model.OpenEntity();
    public void CloseEntity() => _model.CloseEntity();
    public void SetDealer(IDealer newDealer) => _model.SetDealer(newDealer);

    #region Output

    public event Action<IVisitor, ICasinoEntity> OnVisitorRealised;

    private void RealiseVisitor(IVisitor visitor)
    {
        OnVisitorRealised?.Invoke(visitor, this);
    }

    #endregion

    #region Input
    public CasinoEntityType CasinoEntityType => CasinoEntityType.Poker;

    public int MaxSeats => _model.MaxSeats;

    public int OccupiedSeats => _model.OccupiedSeats;

    public bool HasFreeSeats => _model.HasFreeSeats;

    public bool CanJoin => _model.CanJoin;

    #endregion
}
