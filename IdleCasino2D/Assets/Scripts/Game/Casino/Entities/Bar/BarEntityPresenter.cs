using System;
using UnityEngine;

public class BarEntityPresenter : ICasinoEntityInfo, ICasinoEntityVisitorTraffic, ICasinoEntityProfit, ICasinoEntityStaff
{
    private readonly BarEntityModel _model;

    public BarEntityPresenter(BarEntityModel model)
    {
        _model = model;
    }

    public void Initialize() => _model.Initialize();
    public void Dispose() => _model.Dispose();

    // ======================== STAFF ========================
    public StaffType PersonalType => StaffType.Bartender;
    public int CountStaffNeed => 1;
    public int CountStaff => _model.CountStaff;
    public void SetStaff(IStaff staff) => _model.SetStaff(staff);

    // ======================== VISITOR TRAFFIC ========================
    public event Action<IVisitor> OnVisitorRealised
    {
        add => _model.OnVisitorRealised += value;
        remove => _model.OnVisitorRealised -= value;
    }

    public void AddVisitor(IVisitor visitor) => _model.AddVisitor(visitor);
    public void RemoveVisitor(IVisitor visitor) => _model.RemoveVisitor(visitor);

    // ======================== INFO ========================
    public CasinoEntityType CasinoEntityType => CasinoEntityType.Bar;
    public bool IsOpen => _model.IsOpen;
    public bool CanJoin => _model.CanJoin;
    public bool IsGameRunning => false;

    // ======================== PROFIT ========================
    public event Action<Vector3, int> OnAddCoins
    {
        add => _model.OnAddCoins += value;
        remove => _model.OnAddCoins -= value;
    }
}
