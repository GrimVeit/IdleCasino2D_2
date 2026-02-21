using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineEntityPresenter : ICasinoEntity
{
    private readonly SlotMachineEntityModel _model;

    public SlotMachineEntityPresenter(SlotMachineEntityModel model)
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

    public void AddVisitor(IVisitor visitor) => _model.AddVisitor(visitor);

    public void RemoveVisitor(IVisitor visitor) => _model.RemoveVisitor(visitor);



    public void Open() => _model.OpenEntity();
    public void Close() => _model.CloseEntity();



    public void ActivateManualInteractive()
    {

    }

    public void DeactivateManualInteractive()
    {

    }

    public void SetDealer(IDealer newDealer)
    {

    }

    #region Output

    public event Action<IVisitor, ICasinoEntity> OnVisitorRealised;

    private void RealiseVisitor(IVisitor visitor)
    {
        OnVisitorRealised?.Invoke(visitor, this);
    }

    #endregion

    #region Input
    public CasinoEntityType CasinoEntityType => CasinoEntityType.Slot;

    public int MaxSeats => _model.MaxSeats;

    public int OccupiedSeats => _model.OccupiedSeats;

    public bool HasFreeSeats => _model.HasFreeSeats;

    public bool CanJoin => _model.CanJoin;

    #endregion
}
