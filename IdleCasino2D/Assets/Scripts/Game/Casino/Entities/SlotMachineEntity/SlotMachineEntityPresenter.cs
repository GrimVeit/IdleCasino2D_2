using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachineEntityPresenter : ICasinoEntityInfo, ICasinoEntityActivator, ICasinoEntityInteractiveProvider, ICasinoEntityVisitorTraffic, ICasinoEntityProfit, ICasinoEntitySpotClickListener, ICasinoEntityManual
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



    public void ActivateEntityInteractive() => _model.ActivateEntityInteractive();

    public void DeactivateEntityInteractive() => _model.DeactivateEntityInteractive();

    public void SetDealer(IDealer newDealer)
    {

    }

    #region ACTIVATOR

    public void Open() => _model.Open();
    public void Close() => _model.Close();

    #endregion

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
    public CasinoEntityType CasinoEntityType => CasinoEntityType.Slot;
    public bool IsOpen => _model.IsOpen;
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

    #region SPOT CLICK LISTENER

    public event Action OnSpotClick
    {
        add => _model.OnSpotClick += value;
        remove => _model.OnSpotClick -= value;
    }

    #endregion

    #region MANUAL

    public void ManualStartGame() => _model.ManualStartGame();

    #endregion
}
