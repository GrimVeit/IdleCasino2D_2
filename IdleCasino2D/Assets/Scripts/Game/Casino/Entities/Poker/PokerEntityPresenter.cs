using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PokerEntityPresenter : ICasinoEntityInfo, ICasinoEntityActivator, ICasinoEntityVisitorTraffic, ICasinoEntityProfit, ICasinoEntitySpotClickListener, ICasinoEntityManual, ICasinoEntityStaff, ICasinoEntityInteractiveProvider
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

    public void ActivateEntityInteractive() => _model.ActivateEntityInteractive();
    public void DeactivateEntityInteractive() => _model.DeactivateEntityInteractive();


    #region STAFF

    public StaffType PersonalType => StaffType.Croupier;
    public int CountStaffNeed => 1;
    public int CountStaff => _model.CountStaff;
    public void SetStaff(IStaff staff) => _model.SetStaff(staff);

    #endregion

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
    public CasinoEntityType CasinoEntityType => CasinoEntityType.Poker;
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
