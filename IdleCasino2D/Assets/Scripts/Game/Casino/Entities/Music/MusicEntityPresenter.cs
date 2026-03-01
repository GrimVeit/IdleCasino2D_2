using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEntityPresenter :
    ICasinoEntityInfo,
    ICasinoEntityVisitorTraffic,
    ICasinoEntityProfit,
    ICasinoEntityStaff
{
    private readonly MusicEntityModel _model;

    public MusicEntityPresenter(MusicEntityModel model)
    {
        _model = model;
    }

    public void Initialize() => _model.Initialize();
    public void Dispose() => _model.Dispose();

    #region STAFF

    public StaffType PersonalType => StaffType.Songstress;
    public int CountStaffNeed => 1;
    public int CountStaff => _model.CountStaff;
    public void SetStaff(IStaff staff) => _model.SetStaff(staff);

    #endregion

    #region INFO

    public CasinoEntityType CasinoEntityType => CasinoEntityType.Music;
    public bool IsOpen => _model.IsOpen;
    public bool CanJoin => _model.CanJoin;
    public bool IsGameRunning => false;

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

    #region PROFIT

    public event Action<Vector3, int> OnAddCoins
    {
        add => _model.OnAddCoins += value;
        remove => _model.OnAddCoins -= value;
    }

    #endregion
}
