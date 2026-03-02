using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICasinoEntityInfo
{
    CasinoEntityType CasinoEntityType { get; }
    bool IsOpen { get; }
    bool CanJoin { get; }
    bool IsGameRunning {  get; }
}

public interface ICasinoEntityActivator
{
    void Open();
    void Close();
}

public interface ICasinoEntityManual
{
    public void ManualStartGame();
}

public interface ICasinoEntityInteractiveProvider
{
    public void ActivateEntityInteractive();
    public void DeactivateEntityInteractive();
}

public interface ICasinoEntityVisitorTraffic
{
    void AddVisitor(IVisitor visitor);
    void RemoveVisitor(IVisitor visitor);

    event Action<IVisitor> OnVisitorRealised;
}

public interface ICasinoEntityProfit
{
    public event Action<Vector3, int> OnAddCoins;
}

public interface ICasinoEntityStaff
{
    public StaffType PersonalType { get; }
    public int CountStaffNeed { get; }
    public int CountStaff { get; }
    public void SetStaff(IStaff stuff);
}

public interface ICasinoEntityHighlightProvider
{
    public void ActivateHighlight();
    public void DeactivateHighlight();
}

public interface ICasinoEntitySpotClickListener
{
    public event Action OnSpotClick;
}

public interface ICasinoEntityVisitorClickListener
{
    public event Action<IVisitor> OnVisitorClick;
}
